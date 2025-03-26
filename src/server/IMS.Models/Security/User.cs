using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Security;

public class User : IdentityUser<Guid>, IBaseEntity
{
    [Required]
    [StringLength(255)]
    public required string FullName { get; set; }

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public override required string Email { get; set; }

    public Gender Gender { get; set; } = Gender.OTHER;

    [StringLength(500)]
    public string? Address { get; set; }

    [ForeignKey(nameof(Department))]
    public Guid? DepartmentId { get; set; }

    public Department? Department { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public Guid? CreatedById { get; set; }

    public User? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public Guid? UpdatedById { get; set; }

    public User? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey(nameof(DeletedBy))]
    public Guid? DeletedById { get; set; }

    public User? DeletedBy { get; set; }
    
    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }
}
