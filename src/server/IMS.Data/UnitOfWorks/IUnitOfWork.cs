using IMS.Data.Repositories;
using IMS.Models.Common;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IMSDbContext Context { get; }


    IRepository<RefreshToken> RefreshTokenRepository { get; }

    IRepository<Candidate> CandidateRepository { get; }

    IRepository<Job> JobRepository { get; }

    IGenericRepository<Skill> GenericSkillRepository { get; }

    IGenericRepository<Position> PositionRepository {get ;}

    IGenericRepository<Level> LevelRepository {get ;}

    IGenericRepository<Benefit> BenefitRepository {get ;}

    IRepository<Interview> InterviewRepository {get;}

    IRepository<Offer> OfferRepository { get; }

    IGenericRepository<Department> DepartmentRepository { get; }

    IDepartmentRepository DepartmentsRepository {get ;}

    IContactTypeRepository ContactTypeRepository {get ;}

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    new void Dispose();
    Task RollbackTransactionAsync();
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
