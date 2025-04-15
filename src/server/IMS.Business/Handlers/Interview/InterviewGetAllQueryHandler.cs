using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

    public class InterviewGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) :
        BaseHandler(unitOfWork, mapper),
        IRequestHandler<InterviewGetAllQuery, IEnumerable<InterviewViewModel>>
    {
        public async Task<IEnumerable<InterviewViewModel>> Handle(InterviewGetAllQuery request, CancellationToken cancellationToken)
        {
            var interviews = await _unitOfWork.InterviewRepository.GetQuery()
                .Include(x => x.Candidate)
                .Include(x => x.Job)
                .Include(x => x.RecruiterOwner)
                .Include(x => x.Interviewers)
                .ThenInclude(ii => ii.User)
                .ToListAsync(cancellationToken);

            var interviewViewModels = _mapper.Map<List<InterviewViewModel>>(interviews);

            return interviewViewModels;

        }
    }
