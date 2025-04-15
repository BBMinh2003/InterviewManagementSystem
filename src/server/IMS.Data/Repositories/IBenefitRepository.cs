using IMS.Models.Common;

namespace IMS.Data.Repositories;

public interface IBenefitRepository
{
    Task<IEnumerable<Benefit>> GetAllAsync();
}
