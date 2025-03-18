using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("ContactTypes", Schema = "Common")]
public class ContactType : BaseEntity, IBaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }
}
