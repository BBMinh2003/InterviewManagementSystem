using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Candidates", Schema = "Common")]
public class Candidate : BaseEntity, IBaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [Required]
    [StringLength(255)]
    public required string Email { get; set; }

    [Required]
    public required DateTime DateOfBirth { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    [StringLength(20)]
    public required string Phone { get; set; }

    [Required]
    public required string Gender { get; set; }

    [Required]
    public required string CV_Attachment { get; set; }

    public string? Note { get; set; }

    public CandidateStatus Status { get; set; } 

    public Guid YearOfExperience { get; set; }

    [Required]
    public required string HighestLevel { get; set; }

    [ForeignKey(nameof(Position))]
    public Guid PositionId { get; set; }

    public Position? Position { get; set; }

    [ForeignKey(nameof(RecruiterOwner))]
    public Guid RecruiterOwnerId { get; set; }

    public User? RecruiterOwner { get; set; }
    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
}

public enum CandidateStatus
{
    WaitingForInterview,
    WaitingForApproval,
    WaitingForResponse,
    Open,
    PassedInterview,
    ApprovedOffer,
    RejectedOffer,
    AcceptedOffer,
    DeclinedOffer,
    CancelledOffer,
    FailedInterview,
    CancelledInterview,
    Banned
}