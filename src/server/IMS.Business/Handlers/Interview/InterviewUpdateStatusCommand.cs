using System;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class InterviewUpdateStatusCommand : IRequest<bool>
{
    public Guid InterviewId { get; set; }
}
