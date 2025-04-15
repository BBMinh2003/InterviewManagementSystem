using System;
using IMS.Data.Repositories;
using IMS.Models.Common;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IMSDbContext _context;

    private readonly IUserIdentity _currentUser;

    private bool _disposed = false;

    public UnitOfWork(IMSDbContext context, IUserIdentity currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public IMSDbContext Context => _context;


    private IRepository<RefreshToken>? _refreshTokenRepository;

    public IRepository<RefreshToken> RefreshTokenRepository => _refreshTokenRepository ??= new Repository<RefreshToken>(_context, _currentUser);

    private IRepository<Candidate>? _candidateRepository;

    public IRepository<Candidate> CandidateRepository => _candidateRepository ??= new Repository<Candidate>(_context, _currentUser);

    private IRepository<Job>? _jobRepository;

    public IRepository<Job> JobRepository => _jobRepository ??= new Repository<Job>(_context, _currentUser);

    private IGenericRepository<Skill> _skillRepository;

    public IGenericRepository<Skill> GenericSkillRepository => _skillRepository ??= new GenericRepository<Skill, IMSDbContext>(_context);

    private IGenericRepository<Position> _positionRepository;
    public IGenericRepository<Position> PositionRepository => _positionRepository ??= new GenericRepository<Position, IMSDbContext>(_context);

    private IGenericRepository<Level> _levelRepository;
    public IGenericRepository<Level> LevelRepository => _levelRepository ??= new GenericRepository<Level, IMSDbContext>(_context);

    private IGenericRepository<Benefit> _benefitRepository;
    public IGenericRepository<Benefit> BenefitRepository => _benefitRepository ??= new GenericRepository<Benefit, IMSDbContext>(_context);

    private IRepository<Interview>? _interviewRepository;

    public IRepository<Interview> InterviewRepository => _interviewRepository ??= new Repository<Interview>(_context, _currentUser);

    private IRepository<Offer>? _offerRepository;

    public IRepository<Offer> OfferRepository => _offerRepository ??= new Repository<Offer>(_context, _currentUser);

    private IGenericRepository<Department>? _departmentRepository;
    
    public IGenericRepository<Department> DepartmentRepository => _departmentRepository ??= new GenericRepository<Department, IMSDbContext>(_context);

    private IDepartmentRepository _departmentsRepository;

    public IDepartmentRepository DepartmentsRepository => _departmentsRepository ??= new DepartmentRepository(_context);

    private IContactTypeRepository _contactTypeRepository;

    public IContactTypeRepository ContactTypeRepository => _contactTypeRepository ??= new ContactTypeRepository(_context);
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}