using System;
using AutoMapper;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferDeleteByIdCommandHandler: BaseHandler,
    IRequestHandler<OfferDeleteByIdCommand, bool>
{
    public OfferDeleteByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<bool> Handle(OfferDeleteByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.OfferRepository.GetByIdAsync(request.Id) ??
            throw new ResourceNotFoundException($"Offer with {request.Id} is not found");

        _unitOfWork.OfferRepository.Delete(entity);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}
