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

public class UserProfileQueryHandler : BaseUserHandler, IRequestHandler<UserProfileQuery, UserViewModel>
{
    public UserProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : base(unitOfWork, mapper, userManager, roleManager, currentUser, emailService)
    {
    }

    public async Task<UserViewModel> Handle(UserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId);
        if (user == null)
        {
            throw new ResourceNotFoundException("User not found");
        }
        return _mapper.Map<UserViewModel>(user);
    }
}