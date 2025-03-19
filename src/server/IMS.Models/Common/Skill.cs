using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Skills", Schema = "Common")]
public class Skill : BaseEntity
{
    [Required] 
    [StringLength(255)]
    public required string Name { get; set; }

    public string? Description { get; set; }
}
