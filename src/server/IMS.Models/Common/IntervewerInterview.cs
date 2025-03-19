using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("IntervewerInterviews", Schema = "Common")]
public class IntervewerInterview
{
    [ForeignKey(nameof(Interview))]
    public Guid InterviewId { get; set; }

    public Interview? Interview { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public User? User { get; set; }
}
