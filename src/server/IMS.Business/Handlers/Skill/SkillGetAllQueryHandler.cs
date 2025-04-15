using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Skill;

public class SkillGetAllQueryHandler : BaseHandler,
    IRequestHandler<SkillGetAllQuery, IEnumerable<SkillViewModel>>
{
    public SkillGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<SkillViewModel>> Handle(SkillGetAllQuery request, CancellationToken cancellationToken)
    {
        var skills = await _unitOfWork.GenericSkillRepository.GetQuery().ToListAsync(cancellationToken);

        var skillViewModels = skills.Select(s => new SkillViewModel
        {
            Id = s.Id,
            Name = s.Name,
        }).ToList();

        return skillViewModels;
    }
}
