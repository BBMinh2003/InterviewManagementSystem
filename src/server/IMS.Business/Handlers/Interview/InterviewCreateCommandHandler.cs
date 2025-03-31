using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<InterviewCreateCommand, InterviewViewModel>
{
    public async Task<InterviewViewModel> Handle(InterviewCreateCommand request, CancellationToken cancellationToken)
    {
        var existedInterview = await _unitOfWork.InterviewRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.JobId == request.JobId && x.StartAt == request.StartAt);

        if (existedInterview != null)
        {
            throw new ResourceUniqueException($"An interview for this candidate and job at the specified time already exists.");
        }

        var entity = new Interview
        {
            CandidateId = request.CandidateId,
            JobId = request.JobId,
            RecruiterOwnerId = request.RecruiterOwnerId,
            Title = request.Title,
            Note = request.Note,
            Location = request.Location,
            MeetingUrl = request.MeetingUrl,
            Result = request.Result,
            Status = InterviewStatus.New,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            InterviewDate = request.InterviewDate
        };

        _unitOfWork.InterviewRepository.Add(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to save interview.");
        }

        entity.Interviewers = request.InterviewerIds
            .Select(interviewerId => new IntervewerInterview { InterviewId = entity.Id, UserId = interviewerId })
            .ToList();

        await _unitOfWork.SaveChangesAsync();

        var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(request.CandidateId);
        if (candidate == null)
        {
            throw new ResourceNotFoundException($"Candidate with ID {request.CandidateId} not found.");
        }

        candidate.Status = CandidateStatus.WaitingForInterview;
        await _unitOfWork.SaveChangesAsync();

        var createdEntity = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Job)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Interviewers)
            .ThenInclude(ii => ii.User)
            .FirstOrDefaultAsync(x => x.Id == entity.Id)
            ?? throw new ResourceNotFoundException($"Interview with {entity.Id} is not found");

        return _mapper.Map<InterviewViewModel>(createdEntity);
    }
}
