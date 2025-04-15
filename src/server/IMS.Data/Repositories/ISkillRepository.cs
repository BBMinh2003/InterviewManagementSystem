using IMS.Models.Common;

namespace IMS.Data.Repositories;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetAllAsync();
}