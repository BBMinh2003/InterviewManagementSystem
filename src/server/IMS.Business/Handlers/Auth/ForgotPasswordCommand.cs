using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace IMS.Business.ViewModels.Auth;

public class ForgotPasswordCommand : IRequest<BaseResponse>
{
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "ClientURI is required")]
    public required string ClientURI { get; set; }
}
