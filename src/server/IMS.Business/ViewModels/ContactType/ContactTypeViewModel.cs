using System;

namespace IMS.Business.ViewModels.ContactType;

public class ContactTypeViewModel
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
}
