using System;
using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels.Auth;
using MediatR;

namespace IMS.Business.Handlers.Auth;

public class RefreshAccessTokenCommand : IRequest<LoginResponse>
{
    [Required(ErrorMessage = "{0} is required")]
    public required string RefreshToken { get; set; }
}
