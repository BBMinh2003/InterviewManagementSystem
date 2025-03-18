using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Common;

[Table("JobSkills", Schema = "Common")]
public class JobSkill
{
    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(Skill))]
    public Guid SkillId { get; set; }

    public Skill? Skill { get; set; }
}
