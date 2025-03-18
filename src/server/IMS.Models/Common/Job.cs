using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Jobs", Schema = "Common")]

public class Job : BaseEntity, IBaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Title { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaxSalary { get; set; }

    [StringLength(255)]
    public string? WorkingAddress { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();

    public virtual ICollection<JobLevel> JobLevels { get; set; } = new List<JobLevel>();

    public virtual ICollection<JobBenefit> JobBenefits { get; set; } = new List<JobBenefit>();
}
