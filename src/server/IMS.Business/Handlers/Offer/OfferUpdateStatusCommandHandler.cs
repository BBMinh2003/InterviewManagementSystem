using System;
using AutoMapper;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferUpdateStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<OfferUpdateStatusCommand, bool>
{
    private static readonly HashSet<OfferStatus> ValidOfferStatuses = new HashSet<OfferStatus>
    {
        OfferStatus.Accepted,
        OfferStatus.Approved,
        OfferStatus.Rejected,
        OfferStatus.WaitingForApproval,
        OfferStatus.WaitingForResponse,
        OfferStatus.Declined,
        OfferStatus.Cancelled
    };
    public async Task<bool> Handle(OfferUpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.OfferId);

        if (offer == null)
        {
            throw new ResourceNotFoundException($"Offer with ID {request.OfferId} not found.");
        }

         if (!ValidOfferStatuses.Contains(offer.Status))
        {
            return false;
        }

        if (!ValidOfferStatuses.Contains(request.offerStatus))
        {
            return false;
        }

        offer.Status = request.offerStatus;
        _unitOfWork.OfferRepository.Update(offer);
        await _unitOfWork.SaveChangesAsync();

        return true; 
    }
}