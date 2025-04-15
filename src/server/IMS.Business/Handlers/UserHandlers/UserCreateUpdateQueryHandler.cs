using System;
using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.Common;
using IMS.Business.ViewModels.RoleViews;
using IMS.Business.ViewModels.UserViews;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserCreateUpdateQueryHandler : BaseUserHandler, IRequestHandler<UserCreateUpdateQuery, UserCreateUpdateViewModel>
{
    public UserCreateUpdateQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : base(unitOfWork, mapper, userManager, roleManager, currentUser, emailService)
    {
    }

    public async Task<UserCreateUpdateViewModel> Handle(UserCreateUpdateQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.Select((r) => new RoleViewModel { Id = r.Id, Name = r.Name ?? "" }).ToListAsync();
        var deparments = await _unitOfWork.DepartmentRepository.GetQuery().Select((d) => new DepartmentViewModel { Id = d.Id, Name = d.Name ?? "" }).ToListAsync();
        return new UserCreateUpdateViewModel
        {
            Roles = roles,
            Departments = deparments
        };
    }
}
