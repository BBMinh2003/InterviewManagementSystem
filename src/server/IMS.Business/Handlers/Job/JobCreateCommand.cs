using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels;
using IMS.Models.Common;

namespace IMS.Business.Handlers;

public class JobCreateCommand : BaseCreateCommand<JobViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [DataType(DataType.Date, ErrorMessage = "The {0} field is not a valid date")]
    public required DateTime StartDate { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [DataType(DataType.Date, ErrorMessage = "The {0} field is not a valid date")]
    public required DateTime EndDate { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required decimal MinSalary { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public decimal MaxSalary { get; set; }

    public string? WorkingAddress { get; set; }

    public string? Description { get; set; }

    public JobStatus Status { get; set; } = JobStatus.Draft;

    public List<Guid> JobSkills { get; set; } = [];

    public List<Guid> JobBenefits { get; set; } = [];

    public List<Guid> JobLevels { get; set; } = [];

}