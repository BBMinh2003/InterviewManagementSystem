using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<InterviewUpdateCommand, InterviewViewModel>
{
    public async Task<InterviewViewModel> Handle(
        InterviewUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Job)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Interviewers)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Interview with {request.Id} is not found");

        _mapper.Map(request, entity);

        // Lấy danh sách InterviewerId hiện tại
        var currentInterviewerIds = entity.Interviewers.Select(i => i.UserId).ToList();

        // Lấy danh sách InterviewerId mới từ request
        var newInterviewerIds = request.InterviewerIds;

        // Xác định những người phỏng vấn cần thêm mới
        var interviewersToAdd = newInterviewerIds.Except(currentInterviewerIds)
            .Select(userId => new IntervewerInterview { UserId = userId, InterviewId = entity.Id })
            .ToList();

        // Xác định những người phỏng vấn cần xóa
        var interviewersToRemove = entity.Interviewers
            .Where(i => !newInterviewerIds.Contains(i.UserId))
            .ToList();

        // Xóa những người phỏng vấn không còn trong danh sách
        foreach (var interviewer in interviewersToRemove)
        {
            entity.Interviewers.Remove(interviewer);
        }

        // Chỉ thêm những người phỏng vấn mới nếu họ chưa tồn tại
        foreach (var interviewer in interviewersToAdd)
        {
            if (!entity.Interviewers.Any(i => i.UserId == interviewer.UserId))
            {
                entity.Interviewers.Add(interviewer);
            }
        }

        _unitOfWork.InterviewRepository.Update(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to update interview.");
        }

        var updatedEntity = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .Include(x => x.Interviewers) // Đảm bảo load lại danh sách người phỏng vấn
            .ThenInclude(i => i.User) // Load thông tin chi tiết nếu cần
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Interview with {entity.Id} is not found");

        return _mapper.Map<InterviewViewModel>(updatedEntity);
    }
}