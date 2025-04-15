using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Core.Exceptions;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserProfileUpdateCommandHandler : BaseUserHandler, IRequestHandler<UserProfileUpdateCommand, UserViewModel>
{
    public UserProfileUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : base(unitOfWork, mapper, userManager, roleManager, currentUser, emailService)
    {
    }

    public async Task<UserViewModel> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
    {
       var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken)
            ?? throw new ResourceNotFoundException("User not found.");

        user.UpdatedAt = DateTime.Now;
        user.UpdatedBy = await _userManager.FindByIdAsync(_currentUser.UserId.ToString());
        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Address = request.Address ?? user.Address;
        user.PhoneNumber = request.PhoneNumber;
        user.Gender = request.Gender;
        user.DateOfBirth = request.DateOfBirth;

        await _userManager.UpdateAsync(user);

        return _mapper.Map<UserViewModel>(user);
    }
}
