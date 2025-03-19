using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Common;

[Table("JobBenefits", Schema = "Common")]
public class JobBenefit
{
    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(Benefit))]
    public Guid BenefitId { get; set; }

    public Benefit? Benefit { get; set; }
}
