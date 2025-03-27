using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;

namespace IMS.Business.Handlers;

public class UpdateInterviewStatusCommand : IRequest<bool>
{
    public Guid InterviewId { get; set; }
}

public class UpdateInterviewStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<UpdateInterviewStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateInterviewStatusCommand request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(request.InterviewId);

        if (interview == null)
        {
            throw new ResourceNotFoundException($"Interview with ID {request.InterviewId} not found.");
        }

        if (interview.Status is InterviewStatus.New or InterviewStatus.Invited or InterviewStatus.Interviewed)
        {
            interview.Status = InterviewStatus.Cancelled;
            _unitOfWork.InterviewRepository.Update(interview);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        return false;
    }
}