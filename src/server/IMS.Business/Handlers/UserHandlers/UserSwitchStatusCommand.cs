using System.ComponentModel.DataAnnotations;
using IMS.Business.ViewModels.UserViews;

namespace IMS.Business.Handlers.UserHandlers;

public class UserSwitchStatusCommand : BaseUpdateCommand<UserViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    public required bool IsActive { get; set; } = true;
}
