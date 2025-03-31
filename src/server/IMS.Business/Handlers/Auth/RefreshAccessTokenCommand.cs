using System;
using IMS.Business.ViewModels.Auth;
using MediatR;

namespace IMS.Business.Handlers.Auth;

public class RefreshAccessTokenCommand : IRequest<LoginResponse>
{
    public required string RefreshToken { get; set; }
}
