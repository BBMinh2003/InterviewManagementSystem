using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Common;

[Table("CandidateSkills", Schema = "Common")]
[PrimaryKey(nameof(CandidateId), nameof(SkillId))]
public class CandidateSkill
{
    [ForeignKey(nameof(Candidate))]
    public Guid CandidateId { get; set; }

    public Candidate? Candidate { get; set; }

    [ForeignKey(nameof(Skill))]
    public Guid SkillId { get; set; }

    public Skill? Skill { get; set; }
}
