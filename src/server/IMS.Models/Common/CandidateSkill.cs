using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Common;

[Table("CandidateSkills", Schema = "Common")]
public class CandidateSkill
{
    [ForeignKey(nameof(Candidate))]
    public Guid CandidateId { get; set; }

    public Candidate? Candidate { get; set; }

    [ForeignKey(nameof(Skill))]
    public Guid SkillId { get; set; }

    public Skill? Skill { get; set; }
}
