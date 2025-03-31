using System;
using IMS.Models.Common;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferUpdateStatusCommand: IRequest<bool>
{
    public Guid OfferId { get; set; }

    public OfferStatus offerStatus { get; set; }
}
