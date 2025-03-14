using System;
using IMS.Data.Repositories;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
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