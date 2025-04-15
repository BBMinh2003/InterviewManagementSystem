using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Job;

public class JobGetAllQueryHandler : BaseHandler,
    IRequestHandler<JobGetAllQuery, IEnumerable<JobViewModel>>
{
    public JobGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<JobViewModel>> Handle(JobGetAllQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _unitOfWork.JobRepository
        .GetQuery()
        .Where(c => !c.IsDeleted)
        .Include(c => c.JobLevels).ThenInclude(jl => jl.Level)
        .Include(c => c.JobBenefits).ThenInclude(jb => jb.Benefit)
        .Include(c => c.JobSkills)
        .ThenInclude(cs => cs.Skill)
        .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<JobViewModel>>(jobs);
    }
}
