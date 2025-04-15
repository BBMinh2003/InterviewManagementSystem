using IMS.Models.Common;

namespace IMS.Data.Repositories;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetAllAsync();
}
