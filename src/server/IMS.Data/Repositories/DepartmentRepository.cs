using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IMSDbContext _context;

    public DepartmentRepository(IMSDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Department>> GetAllAsync()=>
        await _context.Departments.ToListAsync();

}
