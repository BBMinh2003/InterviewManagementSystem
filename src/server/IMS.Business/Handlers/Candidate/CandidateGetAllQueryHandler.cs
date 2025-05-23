using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CandidateGetAllQueryHandler : BaseHandler,
    IRequestHandler<CandidateGetAllQuery, IEnumerable<CandidateViewModel>>
{
    public CandidateGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<CandidateViewModel>> Handle(CandidateGetAllQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _unitOfWork.CandidateRepository
        .GetQuery()
        .Where(c => !c.IsDeleted)
        .Include(c => c.Position)
        .Include(c => c.RecruiterOwner)
        .Include(c => c.CandidateSkills)
        .ThenInclude(cs => cs.Skill)
        .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<CandidateViewModel>>(candidates);
    }
}
