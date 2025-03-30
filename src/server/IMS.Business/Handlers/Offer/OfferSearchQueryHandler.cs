using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Extensions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : 
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<OfferSearchQuery, PaginatedResult<OfferViewModel>>
{
    public async Task<PaginatedResult<OfferViewModel>> Handle(
        OfferSearchQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Department)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.ApprovedBy)
            .Include(x => x.Position)
            .Include(x => x.ContactType)
            .Include(x => x.Level)
            .Include(x => x.Interview)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x =>
                x.Note!.Contains(request.Keyword) ||
                x.Candidate!.Name.Contains(request.Keyword) ||
                x.Department!.Name.Contains(request.Keyword) ||
                x.Position!.Name.Contains(request.Keyword) ||
                x.ContactType!.Name.Contains(request.Keyword)
            );
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);
        }

        int total = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderByDescending(x => x.DueDate)
                        .ThenBy(x => x.ContactPeriodFrom);
        }

        var items = await query
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var viewModels = _mapper.Map<IEnumerable<OfferViewModel>>(items);

        return new PaginatedResult<OfferViewModel>(
            request.PageNumber,
            request.PageSize,
            total,
            viewModels
        );
    }
}