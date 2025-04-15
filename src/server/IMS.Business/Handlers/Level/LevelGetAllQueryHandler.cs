using AutoMapper;
using IMS.Business.ViewModels.Level;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Level;

public class LevelGetAllQueryHandler : BaseHandler,
    IRequestHandler<LevelGetAllQuery, IEnumerable<LevelViewModel>>
{
    public LevelGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<LevelViewModel>> Handle(LevelGetAllQuery request, CancellationToken cancellationToken)
    {
        var skills = await _unitOfWork.LevelRepository.GetQuery().ToListAsync(cancellationToken);

        var skillViewModels = skills.Select(s => new LevelViewModel
        {
            Id = s.Id,
            Name = s.Name,
        }).ToList();

        return skillViewModels;
    }
}
