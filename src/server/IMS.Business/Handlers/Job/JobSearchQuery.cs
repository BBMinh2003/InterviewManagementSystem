using IMS.Business.ViewModels;
using IMS.Models.Common;

namespace IMS.Business.Handlers.Job;

public class JobSearchQuery : BaseSearchQuery<JobViewModel>
{
    public JobStatus? Status { get; set; } = null;
}
