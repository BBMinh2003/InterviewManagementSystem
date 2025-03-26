using System;
using AutoMapper;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers.UserHandlers;

public class BaseUserHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager) : BaseHandler(unitOfWork, mapper)
{
    protected readonly UserManager<User> _userManager = userManager;
    protected readonly RoleManager<Role> _roleManager = roleManager;
}
