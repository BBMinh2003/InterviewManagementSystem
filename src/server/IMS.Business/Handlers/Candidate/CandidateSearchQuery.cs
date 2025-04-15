using IMS.Business.ViewModels;
using IMS.Core.Enums;

namespace IMS.Business.Handlers;

public class CandidateSearchQuery : BaseSearchQuery<CandidateViewModel>
{
    public CandidateStatus? Status { get; set; } = null;
}
