using System;
using IMS.Business.ViewModels.UserViews;
using MediatR;

namespace IMS.Business.Handlers.UserHandlers;

public class UserCreateUpdateQuery: IRequest<UserCreateUpdateViewModel>
{
}
