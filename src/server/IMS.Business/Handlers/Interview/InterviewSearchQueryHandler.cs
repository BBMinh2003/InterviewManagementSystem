using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Extensions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<InterviewSearchQuery, PaginatedResult<InterviewViewModel>>
{
    public async Task<PaginatedResult<InterviewViewModel>> Handle(
        InterviewSearchQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Job)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Interviewers)
                .ThenInclude(i => i.User!)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => x.Title.Contains(request.Keyword)
                || x.Note!.Contains(request.Keyword)
                || x.Location!.Contains(request.Keyword)
                || x.MeetingUrl!.Contains(request.Keyword));
        }

        if (request.Status.HasValue) 
        {
            query = query.Where(x => x.Status == request.Status);
        }

        if (request.InterviewerId.HasValue && request.InterviewerId != Guid.Empty) 
        {
            query = query.Where(x => x.Interviewers.Any(i => i.UserId == request.InterviewerId));
        }

        int total = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(x => x.InterviewDate).ThenBy(x => x.StartAt);
        }

        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var viewModels = _mapper.Map<IEnumerable<InterviewViewModel>>(items);

        return new PaginatedResult<InterviewViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}