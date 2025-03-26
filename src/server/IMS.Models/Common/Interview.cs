using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;
using IMS.Core.Enums;


namespace IMS.Models.Common;

[Table("Interviews", Schema = "Common")]
public class Interview : BaseEntity, IBaseEntity
{
    [ForeignKey(nameof(Candidate))]
    public Guid CandidateId { get; set; }

    public Candidate? Candidate { get; set; }

    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(RecruiterOwner))]
    public Guid RecruiterOwnerId { get; set; }
    public User? RecruiterOwner { get; set; }

    [Required]
    [StringLength(255)]
    public required string Title { get; set; }

    public string? Note { get; set; }

    [StringLength(255)]
    public string? Location { get; set; }

    [StringLength(255)]
    public string? MeetingUrl { get; set; }

    public string? Result { get; set; }

    public InterviewStatus Status { get; set; }

    public TimeOnly StartAt { get; set; }
    public TimeOnly EndAt { get; set; }

    public virtual ICollection<IntervewerInterview> Interviewers { get; set; } = new List<IntervewerInterview>();
}
