using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Common;

[Table("JobSkills", Schema = "Common")]
[PrimaryKey(nameof(JobId), nameof(SkillId))]
public class JobSkill
{
    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(Skill))]
    public Guid SkillId { get; set; }

    public Skill? Skill { get; set; }
}
