using IMS.Data.Repositories;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks;

public interface IUnitOfWork
{
    IMSDbContext Context { get; }


    IRepository<RefreshToken> RefreshTokenRepository { get; }


    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    void Dispose();
    Task RollbackTransactionAsync();
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
