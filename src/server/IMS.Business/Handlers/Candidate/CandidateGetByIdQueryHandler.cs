using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CandidateGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : 
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<CandidateGetByIdQuery, CandidateViewModel>
{
    public async Task<CandidateViewModel> Handle(
        CandidateGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CandidateRepository.GetQuery()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
            throw new ResourceNotFoundException("Candidate not found");

        return _mapper.Map<CandidateViewModel>(category);
    }
}  