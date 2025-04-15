using System;

namespace IMS.Business.ViewModels.UserViews;


public class GetUserViewModel: BaseInfoViewModel
{
    public string? FullName { get; set; }

    public List<string> Roles { get; set; } = [];

}
