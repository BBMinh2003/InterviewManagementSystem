using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserGetAdminQueryHandler : BaseUserHandler, IRequestHandler<UserGetAdminQuery, IEnumerable<GetUserViewModel>>
{
    public UserGetAdminQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : base(unitOfWork, mapper, userManager, roleManager, currentUser, emailService)
    {
    }
    public async Task<IEnumerable<GetUserViewModel>> Handle(UserGetAdminQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.GetUsersInRoleAsync("Admin");

        var result = users
            .AsQueryable()
            .Include(u => u.Department)
            .Include(u => u.CreatedBy)
            .Include(u => u.UpdatedBy)
            .Include(u => u.DeletedBy)
            .ToList();

        return _mapper.Map<IEnumerable<GetUserViewModel>>(result);
    }
}
