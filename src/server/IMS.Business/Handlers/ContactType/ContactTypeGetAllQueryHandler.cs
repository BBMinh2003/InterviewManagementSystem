using System;
using AutoMapper;
using IMS.Business.ViewModels.ContactType;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class ContactTypeGetAllQueryHandler: BaseHandler,
    IRequestHandler<ContactTypeGetAllQuery, IEnumerable<ContactTypeViewModel>>
{
    public ContactTypeGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<ContactTypeViewModel>> Handle(ContactTypeGetAllQuery request, CancellationToken cancellationToken)
    {
        var contactTypes = await _unitOfWork.ContactTypeRepository.GetAllAsync();

        var contactTypeViewModels = contactTypes.Select(s => new ContactTypeViewModel
        {
            Id = s.Id,
            Name = s.Name,
        }).ToList();

        return contactTypeViewModels;
    }
}
