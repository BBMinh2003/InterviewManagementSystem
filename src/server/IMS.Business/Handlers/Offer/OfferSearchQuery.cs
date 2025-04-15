using System;
using IMS.Business.ViewModels;
using IMS.Models.Common;

namespace IMS.Business.Handlers;

public class OfferSearchQuery : BaseSearchQuery<OfferViewModel>
{
    public OfferStatus? Status { get; set; }

    public Guid? DepartmentId { get; set; }
}
