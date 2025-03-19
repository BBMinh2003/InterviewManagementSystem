using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Skills", Schema = "Common")]
public class Skill 
{
    [Required]
    [StringLength(255)]
    public required Guid Id { get; set; }
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }
}
