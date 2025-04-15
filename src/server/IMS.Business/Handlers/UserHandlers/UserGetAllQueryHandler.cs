using System;
using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserGetAllQueryHandler : BaseUserHandler, IRequestHandler<UserGetAllQuery, IEnumerable<UserViewModel>>
{
    public UserGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : base(unitOfWork, mapper, userManager, roleManager, currentUser, emailService)
    {
    }

    public async Task<IEnumerable<UserViewModel>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.AsQueryable()
        .Include(u => u.Department)
        .Include(u => u.CreatedBy)
        .Include(u => u.UpdatedBy)
        .Include(u => u.DeletedBy)
        .ToListAsync();

        return _mapper.Map<IEnumerable<UserViewModel>>(users);
    }

}
