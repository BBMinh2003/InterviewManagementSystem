using System;
using AutoMapper;
using IMS.Business.ViewModels.UserViews;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserGetAllQueryHandler : BaseUserHandler, IRequestHandler<UserGetAllQuery, IEnumerable<UserViewModel>>
{
    public UserGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager) : base(unitOfWork, mapper, userManager, roleManager)
    {
    }

    public async Task<IEnumerable<UserViewModel>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        List<User> users = await _userManager.Users.ToListAsync();

        return _mapper.Map<IEnumerable<UserViewModel>>(users);
    }

}
