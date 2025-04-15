using IMS.Business.Services;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IMS.Business.Handlers;

public class InterviewSendReminderCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, AzureStorageService azureStorageService) :
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
            throw new ResourceNotFoundException("Candidate email is missing.");
        }

        var job = await unitOfWork.JobRepository.GetQuery()
            .FirstOrDefaultAsync(j => j.Id == interview.JobId, cancellationToken);

        var emailModel = new
        {
            StartTime = interview.StartAt,
            EndTime = interview.EndAt,
            CandidateName = interview.Candidate.Name,
            CandidatePosition = job?.Title ?? "N/A",
            RecruiterEmail = interview.RecruiterOwner?.Email ?? "N/A",
            ScheduleUrl = $"http://localhost:4200/interview/interview-detail/{interview.Id}",
            MeetingId = interview.MeetingUrl ?? "N/A"
        };

        var fileUrl = azureStorageService.GetFileUrl(interview.Candidate.CV_Attachment);
        var localFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(interview.Candidate.CV_Attachment));

        using (var httpClient = new HttpClient())
        using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(localFilePath);
            await stream.CopyToAsync(fileStream);
        }

        try
        {
            await emailService.SendEmailWithTemplateAndAttachmentAsync(
                interview.Candidate.Email,
                "Upcoming Interview Reminder",
                "InterviewScheduleEmail",
                emailModel,
                localFilePath);
        }
        finally
        {
            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
        }

        interview.Status = InterviewStatus.Invited;
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}
