using System;
using IMS.Models.Common;

namespace IMS.Business.ViewModels;

public class InterviewViewModel 
{
    public required Guid Id {get; set;}
    public required string CandidateName { get; set; }

    public required string JobName { get; set; }

    public required string RecruiterOwnerName { get; set; }

    public required string Title { get; set; }

    public string? Note { get; set; }

    public string? Location { get; set; }

    public string? MeetingUrl { get; set; }

    public string? Result { get; set; }

    public InterviewStatus Status { get; set; }

    public TimeOnly StartAt { get; set; }
    public TimeOnly EndAt { get; set; }
    public DateOnly InterviewDate { get; set; }
    public virtual ICollection<InterviewerInterviewViewModel> Interviewers { get; set; } = [];
}

public class InterviewerInterviewViewModel 
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
}