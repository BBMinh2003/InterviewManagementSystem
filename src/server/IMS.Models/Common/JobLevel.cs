using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Common;

[Table("JobLevels", Schema = "Common")]
[PrimaryKey(nameof(JobId), nameof(LevelId))]
public class JobLevel
{
    [ForeignKey(nameof(Job))]
    public Guid JobId { get; set; }

    public Job? Job { get; set; }

    [ForeignKey(nameof(Level))]
    public Guid LevelId { get; set; }

    public Level? Level { get; set; }
}
