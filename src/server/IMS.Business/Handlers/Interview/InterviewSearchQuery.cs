using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Extensions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using IMS.Models.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSearchQuery : BaseSearchQuery<InterviewViewModel>
{
    public InterviewStatus? Status { get; set; }

    public Guid InterviewerId { get; set; }
}

public class InterviewSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<InterviewSearchQuery, PaginatedResult<InterviewViewModel>>
{
    public async Task<PaginatedResult<InterviewViewModel>> Handle(
        InterviewSearchQuery request,
        CancellationToken cancellationToken)
    {
        // Tạo truy vấn ban đầu
        var query = _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Interviewers)
                .ThenInclude(i => i.User!) // Load danh sách Interviewers
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .AsQueryable();

        // Lọc theo từ khóa nếu có
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => x.Title.Contains(request.Keyword)
                || x.Note!.Contains(request.Keyword)
                || x.Location!.Contains(request.Keyword)
                || x.MeetingUrl!.Contains(request.Keyword));
        }

        // Lọc theo Status nếu có
        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        // Lọc theo InterviewerId nếu có
        if (request.InterviewerId != Guid.Empty)
        {
            query = query.Where(x => x.Interviewers.Any(i => i.UserId == request.InterviewerId));
        }

        // Đếm tổng số lượng kết quả
        int total = await query.CountAsync(cancellationToken);

        // Sắp xếp kết quả
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(x => x.InterviewDate).ThenBy(x => x.StartAt);
        }

        // Lấy danh sách phân trang
        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Chuyển đổi sang ViewModel
        var viewModels = _mapper.Map<IEnumerable<InterviewViewModel>>(items);

        // Trả về kết quả phân trang
        return new PaginatedResult<InterviewViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}