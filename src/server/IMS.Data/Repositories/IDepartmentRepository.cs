using System;
using IMS.Models.Common;

namespace IMS.Data.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
}
