using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Security;

namespace IMS.Models.Common;

[Table("Positions", Schema = "Common")]

public class Position : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }
}
