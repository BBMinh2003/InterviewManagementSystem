using IMS.Models.Common;

namespace IMS.Data.Repositories;

public interface ILevelRepository
{
    Task<IEnumerable<Level>> GetAllAsync();
}
