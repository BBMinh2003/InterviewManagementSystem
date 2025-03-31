using System;
using AutoMapper;
using IMS.Business.Services;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers.UserHandlers;

public class BaseUserHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : BaseHandler(unitOfWork, mapper)
{
    protected readonly UserManager<User> _userManager = userManager;
    protected readonly RoleManager<Role> _roleManager = roleManager;
    protected readonly IUserIdentity _currentUser = currentUser;
    protected readonly IEmailService _emailService = emailService;
}
