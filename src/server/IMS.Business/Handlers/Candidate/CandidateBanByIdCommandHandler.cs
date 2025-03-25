using AutoMapper;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateBanByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<CandidateBanByIdCommand, bool>
{
   
    public async Task<bool> Handle(CandidateBanByIdCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(request.Id)
            ?? throw new ResourceNotFoundException($"Candidate with ID {request.Id} not found");


        // Nếu candidate đã bị ban trước đó, không cần cập nhật lại
        if (candidate.Status == CandidateStatus.Banned)
        {
            return false;
        }

        // Cập nhật trạng thái thành Banned
        candidate.Status = CandidateStatus.Banned;

        _unitOfWork.CandidateRepository.Update(candidate);
        var result = await _unitOfWork.SaveChangesAsync();

        return result > 0; 
    }
}
