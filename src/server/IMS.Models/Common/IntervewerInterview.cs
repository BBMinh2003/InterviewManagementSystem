using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Common;

[Table("IntervewerInterviews", Schema = "Common")]
[PrimaryKey(nameof(InterviewId), nameof(UserId))]
public class IntervewerInterview
{
    [ForeignKey(nameof(Interview))]
    public Guid InterviewId { get; set; }

    public Interview? Interview { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public User? User { get; set; }
}
