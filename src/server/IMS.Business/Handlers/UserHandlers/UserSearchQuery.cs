using System;
using IMS.Business.ViewModels.UserViews;

namespace IMS.Business.Handlers.UserHandlers;

public class UserSearchQuery : BaseSearchQuery<UserViewModel>
{
    public string? Role { get; set; }
}
