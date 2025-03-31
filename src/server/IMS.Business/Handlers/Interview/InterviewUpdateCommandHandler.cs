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

        var currentInterviewerIds = entity.Interviewers.Select(i => i.UserId).ToList();

        var newInterviewerIds = request.InterviewerIds;

        var interviewersToAdd = newInterviewerIds.Except(currentInterviewerIds)
            .Select(userId => new IntervewerInterview { UserId = userId, InterviewId = entity.Id })
            .ToList();

        var interviewersToRemove = entity.Interviewers
            .Where(i => !newInterviewerIds.Contains(i.UserId))
            .ToList();

        foreach (var interviewer in interviewersToRemove)
        {
            entity.Interviewers.Remove(interviewer);
        }

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


        entity.Status = InterviewStatus.Invited;
        _unitOfWork.InterviewRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();


        var updatedEntity = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .Include(x => x.Interviewers)
            .ThenInclude(i => i.User)
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Interview with {entity.Id} is not found");

        return _mapper.Map<InterviewViewModel>(updatedEntity);
    }
}