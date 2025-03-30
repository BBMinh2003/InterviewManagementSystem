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

public class UserUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IUserIdentity currentUser,
        IEmailService emailService
        ) : BaseUserHandler(unitOfWork, mapper, userManager, roleManager, currentUser, emailService), IRequestHandler<UserUpdateCommand, UserViewModel>
{
    public async Task<UserViewModel> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
            ?? throw new ResourceNotFoundException("User not found.");

        user.UpdatedAt = DateTime.Now;
        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Address = request.Address ?? user.Address;
        user.PhoneNumber = request.PhoneNumber;
        user.Gender = request.Gender;
        user.DateOfBirth = request.DateOfBirth;
        user.IsActive = request.IsActive;
        user.Note = request.Note ?? user.Note;

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to remove roles: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        var role = request.RoleId != Guid.Empty ? await _roleManager.FindByIdAsync(request.RoleId.ToString())
            ?? throw new ResourceNotFoundException("Role not found") : null;

        await _userManager.UpdateAsync(user);
        await _userManager.AddToRoleAsync(user, role.Name);

        return _mapper.Map<UserViewModel>(user);
    }
}
