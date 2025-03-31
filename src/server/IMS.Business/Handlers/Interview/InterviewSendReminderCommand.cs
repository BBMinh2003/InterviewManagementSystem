using System;
using MediatR;

namespace IMS.Business.Handlers;

public class InterviewSendReminderCommand : IRequest<bool>
{
    public Guid InterviewId { get; set; }
}
