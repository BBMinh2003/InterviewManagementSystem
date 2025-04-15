using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CandidateUpdateCommandHandler :
    BaseHandler,
    IRequestHandler<CandidateUpdateCommand, CandidateViewModel>
{
    private readonly AzureStorageService _storageService;

    public CandidateUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, AzureStorageService storageService) : base(unitOfWork, mapper)
    {
        _storageService = storageService;
    }

    public async Task<CandidateViewModel> Handle(
    CandidateUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CandidateRepository.GetQuery()
            .Include(x => x.CandidateSkills)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Candidate with {request.Id} is not found");

        _mapper.Map(request, entity);

        // Upload CV lên Azure
        if (request.CvFile != null)
        {
            string fileName = $"{Guid.NewGuid()}_{request.CvFile.FileName}";
            entity.CV_Attachment = fileName;
            using var stream = new MemoryStream();
            await request.CvFile.CopyToAsync(stream);
            stream.Position = 0;
            await _storageService.UploadFileAsync(stream, fileName);
        }

        // Lấy danh sách SkillId hiện tại
        var currentSkillIds = entity.CandidateSkills.Select(cs => cs.SkillId).ToList();

        // Lấy danh sách SkillId mới từ request
        var newSkillIds = request.CandidateSkillIds;

        // Xác định kỹ năng cần thêm mới
        var skillsToAdd = newSkillIds.Except(currentSkillIds)
            .Select(skillId => new CandidateSkill { SkillId = skillId, CandidateId = entity.Id })
            .ToList();

        // Xác định kỹ năng cần xóa
        var skillsToRemove = entity.CandidateSkills
            .Where(cs => !newSkillIds.Contains(cs.SkillId))
            .ToList();

        // Xóa các kỹ năng không còn tồn tại
        foreach (var skill in skillsToRemove)
        {
            entity.CandidateSkills.Remove(skill);
        }

        // Chỉ thêm kỹ năng mới nếu nó chưa tồn tại
        foreach (var skill in skillsToAdd)
        {
            if (!entity.CandidateSkills.Any(cs => cs.SkillId == skill.SkillId))
            {
                entity.CandidateSkills.Add(skill);
            }
        }

        _unitOfWork.CandidateRepository.Update(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to update candidate.");
        }



        var updatedEntity = await _unitOfWork.CandidateRepository.GetQuery()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .Include(x => x.CandidateSkills) // Đảm bảo load lại danh sách kỹ năng
            .ThenInclude(cs => cs.Skill) // Load thông tin chi tiết nếu cần
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Candidate with {entity.Id} is not found");

        return _mapper.Map<CandidateViewModel>(updatedEntity);
    }

}