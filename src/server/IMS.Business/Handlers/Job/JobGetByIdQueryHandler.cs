using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Job;

public class JobGetByIdQueryHandler : BaseHandler, IRequestHandler<JobGetByIdQuery, JobViewModel>
{
    public JobGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
    public async Task<JobViewModel> Handle(JobGetByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork.JobRepository.GetQuery()
            .Include(j => j.JobSkills)
            .ThenInclude(js=>js.Skill)
            .Include(j => j.JobLevels)
            .ThenInclude(js=>js.Level)
            .Include(j => j.JobBenefits)
            .ThenInclude(js=>js.Benefit)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Job not found");

        return _mapper.Map<JobViewModel>(job);
    }
}