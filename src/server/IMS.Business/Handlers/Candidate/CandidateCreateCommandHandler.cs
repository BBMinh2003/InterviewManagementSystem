using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CandidateCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<CandidateCreateCommand, CandidateViewModel>
{
    public async Task<CandidateViewModel> Handle(
        CandidateCreateCommand request, CancellationToken cancellationToken)
    {
        var existedCandidate = await _unitOfWork.CandidateRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (existedCandidate != null)
        {
            throw new ResourceUniqueException($"Candidate with email {request.Email} already exists.");
        }

        var entity = new Candidate
        {
            Name = request.Name,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            Phone = request.Phone,
            Gender = request.Gender,
            CV_Attachment = request.CV_Attachment,
            Note = request.Note,
            Status = request.Status,
            YearOfExperience = request.YearOfExperience,
            HighestLevel = request.HighestLevel,
            PositionId = request.PositionId,
            RecruiterOwnerId = request.RecruiterOwnerId,
        };

        _unitOfWork.CandidateRepository.Add(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to save candidate.");
        }

        // Thêm CandidateSkills sau khi entity đã được lưu
        entity.CandidateSkills = request.CandidateSkillIds
            .Select(skillId => new CandidateSkill { SkillId = skillId, CandidateId = entity.Id })
            .ToList();

        await _unitOfWork.SaveChangesAsync();

        var createdEntity = await _unitOfWork.CandidateRepository.GetQuery()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .FirstOrDefaultAsync(x => x.Id == entity.Id) 
            ?? throw new ResourceNotFoundException($"Candidate with {entity.Id} is not found");

        return _mapper.Map<CandidateViewModel>(createdEntity);
    }

    
}
