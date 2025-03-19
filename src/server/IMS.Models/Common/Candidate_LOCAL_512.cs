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

    [StringLength(500)]
    public string? Note { get; set; }

    [Required]
    public required CandidateStatus Status { get; set; }

    public int YearOfExperience { get; set; }

    public int HighestLevel { get; set; }

    [ForeignKey("Position")]
    public required Guid PositionId { get; set; }

    public required Position Position { get; set; }

    [ForeignKey("User")]
    public required Guid RecruiterId { get; set; }

    public required User User { get; set; }
}


