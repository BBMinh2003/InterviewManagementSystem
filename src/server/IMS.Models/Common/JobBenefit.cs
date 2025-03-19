using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Common;

[Table("JobBenefits", Schema = "Common")]
[PrimaryKey(nameof(JobId), nameof(BenefitId))]
public class JobBenefit
{
    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(Benefit))]
    public Guid BenefitId { get; set; }

    public Benefit? Benefit { get; set; }
}
