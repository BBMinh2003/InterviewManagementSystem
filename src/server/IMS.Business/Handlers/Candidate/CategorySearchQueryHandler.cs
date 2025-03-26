using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Extensions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CategorySearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : 
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<CategorySearchQuery, PaginatedResult<CandidateViewModel>>
{
    public async Task<PaginatedResult<CandidateViewModel>> Handle(
        CategorySearchQuery request,
        CancellationToken cancellationToken)
    {
        // Tao query
        var query = _unitOfWork.CandidateRepository.GetQuery();

        // Check keyword not null or empty, then filter
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => x.Name.Contains(request.Keyword) 
                || x.Note!.Contains(request.Keyword)
                || x.Email.Contains(request.Keyword) 
                || x.Address.Contains(request.Keyword)
                || x.Phone.Contains(request.Keyword));
        }

        // Check keyword not null or empty, then filter
        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        // Dem so luong
        int total = await query.CountAsync(cancellationToken);

        // Sap xep
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(x => x.Name);
        }

        // Lay du lieu
        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.DeletedBy)
            .ToListAsync(cancellationToken);

        // Chuyen du lieu sang view model
        var viewModels = _mapper.Map<IEnumerable<CandidateViewModel>>(items);

        // Tra ve ket qua
        return new PaginatedResult<CandidateViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}
