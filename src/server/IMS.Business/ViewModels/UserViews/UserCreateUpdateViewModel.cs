using System;
using IMS.Business.ViewModels.Common;
using IMS.Business.ViewModels.RoleViews;

namespace IMS.Business.ViewModels.UserViews;

public class UserCreateUpdateViewModel
{
    public IEnumerable<RoleViewModel> Roles { get; set; } = [];
    public IEnumerable<DepartmentViewModel> Departments { get; set; } = [];

}
