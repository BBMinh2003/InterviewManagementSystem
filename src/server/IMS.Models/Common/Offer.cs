using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Offers", Schema = "Common")]
public class Offer : BaseEntity, IBaseEntity
{
    [ForeignKey(nameof(Position))]
    public Guid PositionId { get; set; }

    public Position? Position { get; set; }

    [ForeignKey(nameof(Candidate))]
    public Guid CandidateId { get; set; }

    public Candidate? Candidate { get; set; }

    [ForeignKey(nameof(Department))]
    public Guid DepartmentId { get; set; }

    public Department? Department { get; set; }

    [ForeignKey(nameof(RecruiterOwner))]
    public Guid RecruiterOwnerId { get; set; }

    public User? RecruiterOwner { get; set; }

    [ForeignKey(nameof(ContactType))]
    public Guid ContactTypeId { get; set; }

    public ContactType? ContactType { get; set; }

    [ForeignKey(nameof(Interview))]
    public Guid InterviewId { get; set; }

    public Interview? Interview { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    [ForeignKey(nameof(Level))]
    public Guid LevelId { get; set; }

    public Level? Level { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicSalary { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime ContactPeriodFrom { get; set; }

    public DateTime ContactPeriodTo { get; set; }
}