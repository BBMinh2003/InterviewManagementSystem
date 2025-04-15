using System;
using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels.UserViews;
using IMS.Models.Security;
using MediatR;

namespace IMS.Business.Handlers.UserHandlers;

public class UserProfileUpdateCommand: IRequest<UserViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string Email { get; set; }

    [StringLength(500, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [Phone(ErrorMessage = "The {0} field is not a valid phone number")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Gender Gender { get; set; } = Gender.OTHER;

    [Required(ErrorMessage = "The {0} field is required")]
    public required DateTime DateOfBirth { get; set; }

}
