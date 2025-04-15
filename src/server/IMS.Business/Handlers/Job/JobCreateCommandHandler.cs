using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class JobCreateCommandHandler :
    BaseHandler,
    IRequestHandler<JobCreateCommand, JobViewModel>
{
    public JobCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<JobViewModel> Handle(
        JobCreateCommand request, CancellationToken cancellationToken)
    {
        // Ánh xạ từ Command sang Entity Job
        var entity = _mapper.Map<Models.Common.Job>(request);

        // Thêm Job vào DB
        _unitOfWork.JobRepository.Add(entity);
        await _unitOfWork.SaveChangesAsync();

        // Lấy lại Job đã tạo kèm navigation properties
        var createdEntity = await _unitOfWork.JobRepository.GetQuery()
            .Include(x => x.JobSkills).ThenInclude(js => js.Skill)
            .Include(x => x.JobLevels).ThenInclude(jl => jl.Level)
            .Include(x => x.JobBenefits).ThenInclude(jb => jb.Benefit)
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Job with ID {entity.Id} is not found");

        return _mapper.Map<JobViewModel>(createdEntity);
    }
}
