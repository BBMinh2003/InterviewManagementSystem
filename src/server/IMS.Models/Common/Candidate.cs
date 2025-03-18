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

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? CV_Attachment { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public Guid YearOfExperience { get; set; }

    public string? HighestLevel { get; set; }

    [ForeignKey(nameof(Position))]
    public Guid PositionId { get; set; }

    public Position? Position { get; set; }
    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
}
