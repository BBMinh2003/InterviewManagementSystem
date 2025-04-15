using AutoMapper;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;

namespace IMS.Business.Handlers;

public class InterviewUpdateStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<InterviewUpdateStatusCommand, bool>
{
    public async Task<bool> Handle(InterviewUpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(request.InterviewId);

        if (interview == null)
        {
            throw new ResourceNotFoundException($"Interview with ID {request.InterviewId} not found.");
        }

        if (interview.Status is not (InterviewStatus.New or InterviewStatus.Invited or InterviewStatus.Interviewed))
        {
            return false;
        }

        interview.Status = InterviewStatus.Cancelled;
        _unitOfWork.InterviewRepository.Update(interview);

        var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(interview.CandidateId);
        if (candidate != null)
        {
            candidate.Status = CandidateStatus.CancelledInterview;
            _unitOfWork.CandidateRepository.Update(candidate);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

}