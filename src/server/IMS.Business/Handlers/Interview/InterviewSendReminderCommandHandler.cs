using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSendReminderCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService) :
        IRequestHandler<InterviewSendReminderCommand, bool>
{
    public async Task<bool> Handle(InterviewSendReminderCommand request, CancellationToken cancellationToken)
    {
        var interview = await unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.RecruiterOwner) 
            .FirstOrDefaultAsync(x => x.Id == request.InterviewId, cancellationToken)
            ?? throw new ResourceNotFoundException($"Interview with ID {request.InterviewId} not found");

        if (string.IsNullOrEmpty(interview.Candidate?.Email))
        {
            throw new Exception("Candidate email is missing.");
        }

        var emailModel = new
        {
            StartTime = interview.StartAt,
            EndTime = interview.EndAt,
            CandidateName = interview.Candidate.Name,
            CandidatePosition = interview.Job?.Title ?? "N/A",
            RecruiterEmail = interview.RecruiterOwner?.Email ?? "N/A",
            ScheduleUrl = $"http://localhost:5108/api/Interview/{interview.Id}",
            MeetingId = interview.MeetingUrl ?? "N/A"
        };

        await emailService.SendEmailWithTemplateAsync(
            interview.Candidate.Email, 
            "Upcoming Interview Reminder",
            "InterviewScheduleEmail", 
            emailModel);

        return true;
    }
}
