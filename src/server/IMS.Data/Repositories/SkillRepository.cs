using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly IMSDbContext _context;

    public SkillRepository(IMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Skill>> GetAllAsync() =>
        await _context.Skills.ToListAsync();

}
