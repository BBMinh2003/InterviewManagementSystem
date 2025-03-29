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
        IRequestHandler<InterviewSubmitResultCommand, InterviewViewModel>
{
    public async Task<InterviewViewModel> Handle(InterviewSubmitResultCommand request, CancellationToken cancellationToken)
    {
        // Tìm cuộc phỏng vấn theo ID
        var interview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Job)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Interviewers)
            .ThenInclude(ii => ii.User)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Interview with ID {request.Id} not found");

        if (interview.Result == Result.NotApplicable)
        {
            if (request.Result != Result.Passed && request.Result != Result.Failed)
            {
                throw new InvalidOperationException("Result can only be updated to Passed or Failed.");
            }
        }

        // Cập nhật kết quả phỏng vấn
        interview.Result = request.Result;

        _unitOfWork.InterviewRepository.Update(interview);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<InterviewViewModel>(interview);
    }
}