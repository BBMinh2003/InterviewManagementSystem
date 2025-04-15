using System;
using IMS.Models.Common;

namespace IMS.Data.Repositories;

public interface IContactTypeRepository
{
    Task<IEnumerable<ContactType>> GetAllAsync();
}