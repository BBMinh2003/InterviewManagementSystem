using AutoMapper;
using IMS.Business.ViewModels.Benefit;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Benefit;

public class BenefitGetAllQueryHandler : BaseHandler,
    IRequestHandler<BenefitGetAllQuery, IEnumerable<BenefitViewModel>>
{
    public BenefitGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<BenefitViewModel>> Handle(BenefitGetAllQuery request, CancellationToken cancellationToken)
    {
        var skills = await _unitOfWork.BenefitRepository.GetQuery().ToListAsync(cancellationToken);

        var skillViewModels = skills.Select(s => new BenefitViewModel
        {
            Id = s.Id,
            Name = s.Name,
        }).ToList();

        return skillViewModels;
    }
}
