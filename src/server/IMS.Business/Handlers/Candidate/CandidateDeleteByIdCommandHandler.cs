using AutoMapper;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateDeleteByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<CandidateDeleteByIdCommand, bool>
{
    public async Task<bool> Handle(
        CandidateDeleteByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CandidateRepository.GetByIdAsync(request.Id) ??
            throw new ResourceNotFoundException($"Candidate with {request.Id} is not found");

        _unitOfWork.CandidateRepository.Delete(entity);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}