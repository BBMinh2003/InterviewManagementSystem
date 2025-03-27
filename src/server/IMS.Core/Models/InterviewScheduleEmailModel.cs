using System;

namespace IMS.Core.Models;

public class InterviewScheduleEmailModel
{
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; }  = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string CandidatePosition { get; set; } = string.Empty;
    public string RecruiterEmail { get; set; } = string.Empty;
    public string ScheduleUrl { get; set; } = string.Empty;
    public string MeetingId { get; set; } = string.Empty;
    public byte[] CvAttachment { get; set; } = Array.Empty<byte>();
    public string CvFileName { get; set; } = string.Empty;
}
