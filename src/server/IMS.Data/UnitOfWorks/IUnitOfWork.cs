using IMS.Data.Repositories;
using IMS.Models.Common;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks;

public interface IUnitOfWork: IDisposable
{
    IMSDbContext Context { get; }


    IRepository<RefreshToken> RefreshTokenRepository { get; }
    
    IRepository<Candidate> CandidateRepository { get; }

    IRepository<Interview> InterviewRepository {get;}

    IRepository<Offer> OfferRepository {get;}

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    void Dispose();
    Task RollbackTransactionAsync();
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
