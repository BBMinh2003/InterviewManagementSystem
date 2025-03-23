using System;
using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers.Auth;

public class ResetPasswordCommand : IRequest<BaseResponse>
{
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Token is required")]
    public required string Token { get; set; }

}
