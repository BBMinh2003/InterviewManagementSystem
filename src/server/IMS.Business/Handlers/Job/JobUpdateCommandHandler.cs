using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class JobUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : 
    BaseHandler(unitOfWork, mapper), IRequestHandler<JobUpdateCommand, JobViewModel>
{
    public async Task<JobViewModel> Handle(JobUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.JobRepository.GetQuery()
            .Include(x => x.JobSkills)
            .Include(x => x.JobBenefits)
            .Include(x => x.JobLevels)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Job with {request.Id} is not found");

        _mapper.Map(request, entity);

        // Cập nhật JobSkills
        UpdateJobSkills(entity, request.JobSkills);
        
        // Cập nhật JobBenefits
        UpdateJobBenefits(entity, request.JobBenefits);
        
        // Cập nhật JobLevels
        UpdateJobLevels(entity, request.JobLevels);
        
        _unitOfWork.JobRepository.Update(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to update job.");
        }

        var updatedEntity = await _unitOfWork.JobRepository.GetQuery()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .Include(x => x.JobSkills)
            .ThenInclude(cs => cs.Skill)
            .Include(x => x.JobBenefits)
            .ThenInclude(cx => cx.Benefit)
            .Include(x => x.JobLevels)
            .ThenInclude(cx => cx.Level)
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Job with {entity.Id} is not found");

        return _mapper.Map<JobViewModel>(updatedEntity);
    }

    private void UpdateJobSkills(Models.Common.Job entity, List<Guid> newSkillIds)
    {
        var currentSkillIds = entity.JobSkills.Select(cs => cs.SkillId).ToList();
        var skillsToAdd = newSkillIds.Except(currentSkillIds)
            .Select(skillId => new JobSkill { SkillId = skillId, JobId = entity.Id }).ToList();
        var skillsToRemove = entity.JobSkills.Where(cs => !newSkillIds.Contains(cs.SkillId)).ToList();

        foreach (var skill in skillsToRemove)
        {
            entity.JobSkills.Remove(skill);
        }
        foreach (var skill in skillsToAdd)
        {
            entity.JobSkills.Add(skill);
        }
    }

    private void UpdateJobBenefits(Models.Common.Job entity, List<Guid> newBenefitIds)
    {
        var currentBenefitIds = entity.JobBenefits.Select(cb => cb.BenefitId).ToList();
        var benefitsToAdd = newBenefitIds.Except(currentBenefitIds)
            .Select(benefitId => new JobBenefit { BenefitId = benefitId, JobId = entity.Id }).ToList();
        var benefitsToRemove = entity.JobBenefits.Where(cb => !newBenefitIds.Contains(cb.BenefitId)).ToList();

        foreach (var benefit in benefitsToRemove)
        {
            entity.JobBenefits.Remove(benefit);
        }
        foreach (var benefit in benefitsToAdd)
        {
            entity.JobBenefits.Add(benefit);
        }
    }

    private void UpdateJobLevels(Models.Common.Job entity, List<Guid> newLevelIds)
    {
        var currentLevelIds = entity.JobLevels.Select(cl => cl.LevelId).ToList();
        var levelsToAdd = newLevelIds.Except(currentLevelIds)
            .Select(levelId => new JobLevel { LevelId = levelId, JobId = entity.Id }).ToList();
        var levelsToRemove = entity.JobLevels.Where(cl => !newLevelIds.Contains(cl.LevelId)).ToList();

        foreach (var level in levelsToRemove)
        {
            entity.JobLevels.Remove(level);
        }
        foreach (var level in levelsToAdd)
        {
            entity.JobLevels.Add(level);
        }
    }
}
