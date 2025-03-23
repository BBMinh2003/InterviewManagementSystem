using System;
using System.Windows.Input;
using IMS.Models.Security;
using MediatR;

namespace IMS.Business.Handlers.Auth;

public class RegisterRequestCommand : IRequest<RegisterResponse>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Gender Gender { get; set; } = Gender.OTHER;
    public DateTime DateOfBirth { get; set; }
}

public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}
