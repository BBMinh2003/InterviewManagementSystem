using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Common;

[Table("JobLevels", Schema = "Common")]
public class JobLevel
{
    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(Level))]
    public Guid LevelId { get; set; }

    public Level? Level { get; set; }
}
