using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly IMSDbContext _context;

    public PositionRepository(IMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Position>> GetAllAsync() =>
        await _context.Positions.ToListAsync();

}
