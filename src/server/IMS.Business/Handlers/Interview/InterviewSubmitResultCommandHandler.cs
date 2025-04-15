using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSubmitResultCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
        BaseHandler(unitOfWork, mapper),
        IRequestHandler<InterviewSubmitResultCommand, bool>
{
public async Task<bool> Handle(InterviewSubmitResultCommand request, CancellationToken cancellationToken)
{
    var interview = await _unitOfWork.InterviewRepository.GetQuery()
        .Include(x => x.Candidate)
        .Include(x => x.Job)
        .Include(x => x.RecruiterOwner)
        .Include(x => x.Interviewers)
        .ThenInclude(ii => ii.User)
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
        ?? throw new ResourceNotFoundException($"Interview with ID {request.Id} not found");

    if (interview.Result == Result.Passed || interview.Result == Result.Failed)
    {
        return false;
    }

    if (interview.Result == Result.NotApplicable)
    {
        if (request.Result != Result.Passed && request.Result != Result.Failed)
        {
            throw new InvalidOperationException("Result can only be updated to Passed or Failed.");
        }
    }

    interview.Result = request.Result;

    _unitOfWork.InterviewRepository.Update(interview);
    await _unitOfWork.SaveChangesAsync();

    var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(interview.CandidateId);
    if (candidate != null)
    {
        if (request.Result == Result.Failed)
        {
            candidate.Status = CandidateStatus.FailedInterview;
        }
        else
        {
            candidate.Status = CandidateStatus.PassedInterview;
        }

        _unitOfWork.CandidateRepository.Update(candidate);
    }

    await _unitOfWork.SaveChangesAsync();
    return true;
}

}