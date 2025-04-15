using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Core.Extensions;
using IMS.Models.Security;
using Microsoft.AspNetCore.Http;

namespace IMS.Business.Handlers;

public class CandidateCreateCommand : BaseCreateCommand<CandidateViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [DataType(DataType.Date, ErrorMessage = "The {0} field is not a valid date")]
    [CustomValidation(typeof(DateValidator), nameof(DateValidator.ValidatePastDate), ErrorMessage = "The {0} field must be a past date")]
    public required DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(555, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(15, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    [Phone(ErrorMessage = "The {0} field is not a valid phone number")]
    public required string Phone { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Gender Gender { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required string CV_Attachment { get; set; }

    public string? Note { get; set; }

    public CandidateStatus Status { get; set; } = CandidateStatus.Open;

    public int YearOfExperience { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required HighestLevel HighestLevel { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public Guid PositionId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public Guid RecruiterOwnerId { get; set; }

    // Danh sách kỹ năng của ứng viên
    public List<Guid> CandidateSkillIds { get; set; } = [];

    [Required]
    public required IFormFile  CvFile { get; set; }
}
