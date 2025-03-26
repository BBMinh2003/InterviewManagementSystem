using System;
using AutoMapper;
using IMS.Business.ViewModels.UserViews;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;

namespace IMS.API.ConfigurationOptions.Resolvers;

public class UserRolesResolver : IValueResolver<User, UserViewModel, List<string>>
{
    private readonly UserManager<User> _userManager;

    public UserRolesResolver(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public List<string> Resolve(User source, UserViewModel destination, List<string> destMember, ResolutionContext context)
    {
        var roles = _userManager.GetRolesAsync(source).Result;
        return roles.ToList();
    }
}
