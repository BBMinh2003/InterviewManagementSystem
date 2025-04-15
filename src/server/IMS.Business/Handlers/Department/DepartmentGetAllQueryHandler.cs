using System;
using AutoMapper;
using IMS.Business.ViewModels.Common;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class DepartmentGetAllQueryHandler: BaseHandler,
    IRequestHandler<DepartmentGetAllQuery, IEnumerable<DepartmentViewModel>>
{
    public DepartmentGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<DepartmentViewModel>> Handle(DepartmentGetAllQuery request, CancellationToken cancellationToken)
    {
        var departments = await _unitOfWork.DepartmentsRepository.GetAllAsync();

        var departmentViewModels = departments.Select(s => new DepartmentViewModel
        {
            Id = s.Id,
            Name = s.Name,
        }).ToList();

        return departmentViewModels;
    }
}
