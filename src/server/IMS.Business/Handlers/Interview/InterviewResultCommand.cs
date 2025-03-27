using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewResultCommand : IRequest<InterviewViewModel>
{
    public Guid Id { get; set; }
    public Result Result { get; set; }
}

public class InterviewResultCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
        BaseHandler(unitOfWork, mapper),
        IRequestHandler<InterviewResultCommand, InterviewViewModel>
    {
        public async Task<InterviewViewModel> Handle(InterviewResultCommand request, CancellationToken cancellationToken)
        {
            // Tìm cuộc phỏng vấn theo ID
            var interview = await _unitOfWork.InterviewRepository.GetQuery()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new ResourceNotFoundException($"Interview with ID {request.Id} not found");

            if (interview.Result == Result.NotApplicable)
            {
                if (request.Result != Result.Passed && request.Result != Result.Failed)
                {
                    throw new Exception("Result can only be updated to Passed or Failed.");
                }
            }
            else
            {
                throw new Exception("Result has already been set and cannot be changed.");
            }

            // Cập nhật kết quả phỏng vấn
            interview.Result = request.Result;

            _unitOfWork.InterviewRepository.Update(interview);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InterviewViewModel>(interview);
        }
    }