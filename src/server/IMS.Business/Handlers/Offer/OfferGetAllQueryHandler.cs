using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<OfferGetAllQuery, IEnumerable<OfferViewModel>>
{
    public async Task<IEnumerable<OfferViewModel>> Handle(OfferGetAllQuery request, CancellationToken cancellationToken)
    {
        var offers = await _unitOfWork.OfferRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.ContactType)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Department)
            .Include(x => x.Interview)
            .Include(x => x.Level)
            .Include(x => x.Position)
            .ToListAsync(cancellationToken);

        var offerViewModels = _mapper.Map<List<OfferViewModel>>(offers);

        return offerViewModels;

    }
}
