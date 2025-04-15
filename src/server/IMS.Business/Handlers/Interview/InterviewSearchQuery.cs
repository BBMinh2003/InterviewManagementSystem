using System;
using IMS.Business.ViewModels;
using IMS.Models.Common;
using IMS.Models.Security;

namespace IMS.Business.Handlers;

public class InterviewSearchQuery : BaseSearchQuery<InterviewViewModel>
{
    public InterviewStatus? Status { get; set; }

    public Guid? InterviewerId { get; set; }
}
