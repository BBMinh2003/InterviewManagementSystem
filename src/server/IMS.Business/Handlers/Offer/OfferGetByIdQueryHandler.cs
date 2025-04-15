using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferGetByIdQueryHandler: BaseHandler, IRequestHandler<OfferGetByIdQuery, OfferViewModel>
{
    public OfferGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<OfferViewModel> Handle(OfferGetByIdQuery request, CancellationToken cancellationToken)
    {
        var offer = await _unitOfWork.OfferRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.ApprovedBy)
            .Include(x => x.ContactType)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Department)
            .Include(x => x.Interview)
            .Include(x => x.Level)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Offer not found");

        return _mapper.Map<OfferViewModel>(offer);

    }
}
