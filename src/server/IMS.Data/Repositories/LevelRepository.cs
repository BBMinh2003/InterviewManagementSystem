using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;

public class LevelRepository : ILevelRepository
{
    private readonly IMSDbContext _context;

    public LevelRepository(IMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Level>> GetAllAsync() =>
        await _context.Levels.ToListAsync();

}