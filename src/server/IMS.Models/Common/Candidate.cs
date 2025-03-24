using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Core.Enums;
using IMS.Core.Extensions;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Candidates", Schema = "Common")]
public class Candidate : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(DateValidator), nameof(DateValidator.ValidatePastDate))]
    public required DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(555)]
    public required string Address { get; set; }

    [Required]
    [StringLength(15)]
    [Phone]
    public required string Phone { get; set; }

    [Required]
    public required Gender Gender { get; set; }

    [Required]
    public required string CV_Attachment { get; set; }

    public string? Note { get; set; }

    public CandidateStatus Status { get; set; }

    public int YearOfExperience { get; set; }

    [Required]
    public required HighestLevel HighestLevel { get; set; }

    [ForeignKey(nameof(Position))]
    public Guid PositionId { get; set; }

    public Position? Position { get; set; }

    [ForeignKey(nameof(RecruiterOwner))]
    public Guid RecruiterOwnerId { get; set; }

    public User? RecruiterOwner { get; set; }
    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}
