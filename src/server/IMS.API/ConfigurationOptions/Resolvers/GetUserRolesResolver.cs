using AutoMapper;
using IMS.Business.ViewModels.UserViews;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;

namespace IMS.API.ConfigurationOptions.Resolvers;

public class GetUserRolesResolver : IValueResolver<User, GetUserViewModel, List<string>>
{
    private readonly UserManager<User> _userManager;

    public GetUserRolesResolver(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public List<string> Resolve(User source, GetUserViewModel destination, List<string> destMember, ResolutionContext context)
    {
        var roles = _userManager.GetRolesAsync(source).Result;
        return roles.ToList();
    }
}
