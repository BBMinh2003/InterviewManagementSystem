using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;

public class BenefitRepository : IBenefitRepository
{
    private readonly IMSDbContext _context;

    public BenefitRepository(IMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Benefit>> GetAllAsync() =>
        await _context.Benefits.ToListAsync();

}
