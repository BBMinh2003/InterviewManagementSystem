using System;
using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels;
using IMS.Models.Common;

namespace IMS.Business.Handlers;

public class OfferUpdateCommand: BaseUpdateCommand<OfferViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid CandidateId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid DepartmentId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid RecruiterOwnerId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid ApproverId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid PositionId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid InterviewId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid ContactTypeId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid LevelId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required decimal BasicSalary { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required DateTime DueDate { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required DateTime ContactPeriodFrom { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required DateTime ContactPeriodTo { get; set; }

    public OfferStatus Status { get; set; } = OfferStatus.WaitingForApproval;
}