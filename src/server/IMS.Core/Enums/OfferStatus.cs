using System.ComponentModel;

namespace IMS.Models.Common;

public enum OfferStatus
{
    [Description("Waiting For Approval")]
    WaitingForApproval,
    Approved,
    Rejected,
    [Description("Waiting For Response")]
    WaitingForResponse,
    Accepted,
    Declined,
    Cancelled
}
