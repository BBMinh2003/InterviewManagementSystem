using AutoMapper;
using IMS.Business.Handlers.Auth;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Core.Exceptions;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IUserIdentity currentUser,
        IEmailService emailService,
        IPasswordService passwordService
        ) : BaseUserHandler(unitOfWork, mapper, userManager, roleManager, currentUser, emailService), IRequestHandler<UserCreateCommand, UserViewModel>
{
    private readonly IPasswordService _passwordService = passwordService;
    public async Task<UserViewModel> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new ResourceUniqueException("Email is already in use.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            Gender = request.Gender,
            DepartmentId = request.DepartmentId,
            DateOfBirth = request.DateOfBirth,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false,
            Note = request.Note
        };
        

        user.CreatedAt = DateTime.Now;

        var role = request.RoleId != Guid.Empty ? await _roleManager.FindByIdAsync(request.RoleId.ToString())
            ?? throw new ResourceNotFoundException("Role not found") : null;

        var password = _passwordService.GenerateValidPassword();
        await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, role.Name);

        await _emailService.SendEmailAsync(user.Email, "Account Created", $"Your account has been created. Your password is {password}");

        return _mapper.Map<UserViewModel>(user);
    }
}