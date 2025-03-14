using System;
using System.Windows.Input;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

public class RegisterRequestCommandHandler : IRequestHandler<RegisterRequestCommand, RegisterResponse>
    {
        private readonly UserManager<User> _userManager;

        public RegisterRequestCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterResponse> Handle(RegisterRequestCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (existingUser != null)
            {
                return new RegisterResponse { Message = "Email is already in use." };
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new RegisterResponse { Message = "User registration failed." };
            }

            return new RegisterResponse { UserId = user.Id, Message = "User registered successfully." };
        }
    }