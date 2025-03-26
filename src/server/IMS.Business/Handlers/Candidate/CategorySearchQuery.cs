using IMS.Business.ViewModels;
using IMS.Core.Enums;

namespace IMS.Business.Handlers;

public class CategorySearchQuery : BaseSearchQuery<CandidateViewModel>
{
    public CandidateStatus? Status { get; set; } = null;
}
