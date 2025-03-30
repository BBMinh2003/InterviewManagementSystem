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

public class UserSwitchStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IUserIdentity currentUser,
        IEmailService emailService
        ) : BaseUserHandler(unitOfWork, mapper, userManager, roleManager, currentUser, emailService), IRequestHandler<UserSwitchStatusCommand, UserViewModel>
{
    public async Task<UserViewModel> Handle(UserSwitchStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("User not found.");

        user.IsActive = request.IsActive;

        await _userManager.UpdateAsync(user);

        return _mapper.Map<UserViewModel>(user);
    }
}