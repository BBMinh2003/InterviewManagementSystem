using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<OfferUpdateCommand, OfferViewModel>
{
    public async Task<OfferViewModel> Handle(
        OfferUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.OfferRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Department)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.ApprovedBy) 
            .Include(x => x.Position)
            .Include(x => x.ContactType)
            .Include(x => x.Level)
            .Include(x => x.Interview)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Offer with {request.Id} is not found");

        entity = _mapper.Map(request, entity);

        _unitOfWork.OfferRepository.Update(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to update offer.");
        }

        var updatedEntity = await _unitOfWork.OfferRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.ContactType)
            .Include(x => x.ApprovedBy)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Department)
            .Include(x => x.Interview)
            .Include(x => x.Level)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Offer with {entity.Id} is not found");

        return _mapper.Map<OfferViewModel>(updatedEntity);
    }
}