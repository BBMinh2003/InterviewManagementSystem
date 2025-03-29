using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewGetByIdQueryHandler : BaseHandler, IRequestHandler<InterviewGetByIdQuery, InterviewViewModel>
{
    public InterviewGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<InterviewViewModel> Handle(InterviewGetByIdQuery request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Job) 
            .Include(x => x.RecruiterOwner) 
            .Include(x => x.Interviewers)
            .ThenInclude(ii => ii.User) 
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Interview not found");

        return _mapper.Map<InterviewViewModel>(interview);

    }
}
