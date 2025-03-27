using System;
using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSendReminderCommand : IRequest<bool>
{
    public Guid InterviewId { get; set; }
}

public class InterviewSendReminderCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService) :
        IRequestHandler<InterviewSendReminderCommand, bool>
{
    public async Task<bool> Handle(InterviewSendReminderCommand request, CancellationToken cancellationToken)
    {
        // Tìm cuộc phỏng vấn theo ID
        var interview = await unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .FirstOrDefaultAsync(x => x.Id == request.InterviewId, cancellationToken)
            ?? throw new ResourceNotFoundException($"Interview with ID {request.InterviewId} not found");

        // Kiểm tra xem ứng viên có email không
        if (string.IsNullOrEmpty(interview.Candidate?.Email))
        {
            throw new Exception("Candidate email is missing.");
        }

        // Gửi email
        var subject = "Upcoming Interview Reminder";
        var message = $@"
                <p>Dear {interview.Candidate.Name},</p>
                <p>This is a reminder for your upcoming interview.</p>
                <p><strong>Title:</strong> {interview.Title}</p>
                <p><strong>Date:</strong> {interview.InterviewDate:yyyy-MM-dd}</p>
                <p><strong>Time:</strong> {interview.StartAt} - {interview.EndAt}</p>
                <p><strong>Location:</strong> {interview.Location}</p>
                <p>Click <a href='{interview.MeetingUrl}'>here</a> to join the interview.</p>
                <p>Best regards,</p>
                <p>HR Team</p>";

        await emailService.SendEmailAsync(interview.Candidate.Email, subject, message);

        return true;
    }
}
