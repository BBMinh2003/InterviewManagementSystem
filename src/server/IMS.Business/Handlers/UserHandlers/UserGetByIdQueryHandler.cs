using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Core.Exceptions;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers.UserHandlers;

public class UserGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : BaseUserHandler(unitOfWork, mapper, userManager, roleManager, currentUser, emailService), IRequestHandler<UserGetByIdQuery, UserViewModel>
{
    public async Task<UserViewModel> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.Id.ToString()) ??
            throw new ResourceNotFoundException("User not found");

        return _mapper.Map<UserViewModel>(user);
    }

}