using System;

namespace IMS.Business.ViewModels.RoleViews;

public class RoleViewModel
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}
