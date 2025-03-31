using System;
using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Models.Common;

namespace IMS.Business.Handlers;

public class InterviewCreateCommand : BaseCreateCommand<InterviewViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid CandidateId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid JobId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid RecruiterOwnerId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string Title { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? Note { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? Location { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? MeetingUrl { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public Result Result { get; set; } = Result.NotApplicable;

    [Required(ErrorMessage = "The {0} field is required")]
    public TimeOnly StartAt { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public TimeOnly EndAt { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public DateOnly InterviewDate { get; set; }

    public List<Guid> InterviewerIds { get; set; } = [];

}
