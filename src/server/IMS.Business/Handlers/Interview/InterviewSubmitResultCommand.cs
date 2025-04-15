using System;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using MediatR;

namespace IMS.Business.Handlers;

public class InterviewSubmitResultCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Result Result { get; set; }
}
