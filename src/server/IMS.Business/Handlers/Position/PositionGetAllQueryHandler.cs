using AutoMapper;
using IMS.Business.ViewModels.Position;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Position;

public class PositionGetAllQueryHandler : BaseHandler,
    IRequestHandler<PositionGetAllQuery, IEnumerable<PositionViewModel>>
{
    public PositionGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<PositionViewModel>> Handle(PositionGetAllQuery request, CancellationToken cancellationToken)
    {
        var positions = await _unitOfWork.PositionRepository.GetQuery().ToListAsync(cancellationToken);

        var positionViewModels = positions.Select(s => new PositionViewModel
        {
            Id = s.Id,
            Name = s.Name,
        }).ToList();

        return positionViewModels;
    }
}
