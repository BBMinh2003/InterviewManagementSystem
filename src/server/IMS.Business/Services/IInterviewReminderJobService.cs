using System;
using IMS.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IMS.Business.Services;

public interface IInterviewReminderJobService
{
    Task SendInterviewRemindersAsync();
}

public class InterviewReminderJobService : IInterviewReminderJobService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<InterviewReminderJobService> _logger;

    public InterviewReminderJobService(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<InterviewReminderJobService> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task SendInterviewRemindersAsync()
    {
        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        var interviews = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.RecruiterOwner)
            .Where(x => x.InterviewDate == DateOnly.FromDateTime(tomorrow))
            .ToListAsync();

        foreach (var interview in interviews)
        {
            if (string.IsNullOrEmpty(interview.Candidate?.Email))
            {
                _logger.LogWarning($"Interview {interview.Id}: Candidate email is missing.");
                continue;
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

            await _emailService.SendEmailWithTemplateAsync(
                interview.Candidate.Email,
                "Upcoming Interview Reminder",
                "InterviewScheduleEmail",
                emailModel
            );

            _logger.LogInformation($"Interview {interview.Id}: Reminder email sent to {interview.Candidate.Email}.");
        }
    }
}