using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers.UserHandlers;

public class UserGetRecruiterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : BaseUserHandler(unitOfWork, mapper, userManager, roleManager, currentUser, emailService), IRequestHandler<UserGetRecruiterQuery, IEnumerable<GetUserViewModel>>
{
    public async Task<IEnumerable<GetUserViewModel>> Handle(UserGetRecruiterQuery request, CancellationToken cancellationToken)
    {
        var recruiters = await _userManager.GetUsersInRoleAsync("Recruiter");
        var managers = await _userManager.GetUsersInRoleAsync("Manager");

        var users = recruiters.Union(managers).ToList();

        return _mapper.Map<IEnumerable<GetUserViewModel>>(users);
    }
}