using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Extensions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Job;

public class JobSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<JobSearchQuery, PaginatedResult<JobViewModel>>
{
    public async Task<PaginatedResult<JobViewModel>> Handle(
        JobSearchQuery request,
        CancellationToken cancellationToken)
    {
        // Tao query
        var query = _unitOfWork.JobRepository.GetQuery().Where(c => !c.IsDeleted);

        // Check keyword not null or empty, then filter
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => x.Title.Contains(request.Keyword)
                || x.Description!.Contains(request.Keyword)
                || x.WorkingAddress!.Contains(request.Keyword));
        }

        // Check keyword not null or empty, then filter
        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        // Dem so luong
        int total = await query.CountAsync(cancellationToken);

        // Sap xep
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(x => x.Title);
        }

        // Lay du lieu
        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .Include(c => c.JobBenefits)
            .ThenInclude(cs => cs.Benefit)
            .Include(c => c.JobLevels)
            .ThenInclude(c => c.Level)
            .Include(c => c.JobSkills)
            .ThenInclude(cs => cs.Skill)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .ToListAsync(cancellationToken);

        // Chuyen du lieu sang view model
        var viewModels = _mapper.Map<IEnumerable<JobViewModel>>(items);

        // Tra ve ket qua
        return new PaginatedResult<JobViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}