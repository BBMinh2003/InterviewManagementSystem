using System;
using AutoMapper;
using IMS.Data.UnitOfWorks;

namespace IMS.Business.Handlers;

public class BaseHandler(IUnitOfWork unitOfWork, IMapper mapper)
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    protected readonly IMapper _mapper = mapper;
}
