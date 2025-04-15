using IMS.Business.ViewModels.Benefit;
using IMS.Business.ViewModels.Level;
using IMS.Models.Common;

namespace IMS.Business.ViewModels;

public class JobViewModel: BaseInfoViewModel
{
    public required string Title { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal MinSalary { get; set; }

    public decimal MaxSalary { get; set; }

    public string? WorkingAddress { get; set; }

    public string? Description { get; set; }

    public JobStatus Status { get; set; }

    public List<SkillViewModel> JobSkills { get; set; } = [];

    public List<LevelViewModel> JobLevels { get; set; } = [];

    public List<BenefitViewModel> JobBenefits { get; set; } = [];
}
