using System;
using System.Linq.Expressions;
using IMS.Core.Constants;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;


public abstract class RepositoryBase<T, TContext> : GenericRepository<T, TContext>, IRepository<T>
        where T : class, IBaseEntity
        where TContext : IMSDbContext
{
    #region Constructor

    protected RepositoryBase(TContext dataContext) : base(dataContext)
    {
    }

    #endregion

    protected abstract Guid CurrentUserId { get; }

    protected abstract string CurrentUserName { get; }

    #region Overwrite Methods

    public override void Add(T entity)
    {
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        if (entity.GetType().Equals(typeof(RefreshToken)))
        {
            System.Console.WriteLine("RefreshToken created");
        }
        else
        {
            if (CurrentUserId == Guid.Empty)
                entity.CreatedById = Guid.Parse("14448318-7efc-4b97-9b46-0d6ea6ab2056");
            else
                entity.CreatedById = CurrentUserId;
        }
        entity.CreatedAt = DateTime.UtcNow;
        DbSet.Add(entity);
    }

    public override void Update(T entity)
    {
        if (CurrentUserId == Guid.Empty)
            entity.UpdatedById = Guid.Parse("14448318-7efc-4b97-9b46-0d6ea6ab2056");
        else
            entity.UpdatedById = CurrentUserId;
        entity.UpdatedAt = DateTime.UtcNow;
        UpdateEntityObject(entity);
    }

    public override void Delete(T entity, bool isHardDelete = false)
    {
        if (isHardDelete)
        {
            DbSet.Remove(entity);
        }
        else
        {
            entity.DeletedAt = DateTime.UtcNow;
            if (CurrentUserId == Guid.Empty)
                entity.DeletedById = Guid.Parse("14448318-7efc-4b97-9b46-0d6ea6ab2056");
            else
                entity.DeletedById = Guid.Empty;
            entity.IsDeleted = true;
            UpdateEntityObject(entity);
        }
    }

    public override void Delete(Expression<Func<T, bool>> where, bool isHardDelete = false)
    {
        var entities = GetQuery(where).AsEnumerable();
        foreach (var entity in entities)
        {
            Delete(entity, isHardDelete);
        }
    }

    public IQueryable<T> GetQueryById(Guid id)
    {
        return GetQuery(m => m.Id == id);
    }

    public Task<TResult?> GetPropertyById<TResult>(Guid id,
        Expression<Func<T, TResult>> selector)
    {
        return GetQueryById(id).Select(selector).FirstOrDefaultAsync();
    }

    public IQueryable<T> GetQueryWithDeleted()
    {
        return GetQuery().Where(x => x.IsDeleted || x.IsDeleted == false);
    }

    public T? Refresh(T entity)
    {
        DataContext.Entry(entity).State = EntityState.Detached;
        return GetById(entity.Id);
    }

    #endregion

    #region Private Methods

    private void UpdateEntityObject(T entity)
    {
        DbSet.Attach(entity);
        if (CurrentUserId == Guid.Empty)
            entity.UpdatedById = Guid.Parse("14448318-7efc-4b97-9b46-0d6ea6ab2056");
        else
            entity.UpdatedById = CurrentUserId;

        entity.UpdatedAt = DateTime.UtcNow;
        DataContext.Entry(entity).State = EntityState.Modified;
        DataContext.Entry(entity).GetDatabaseValues()?.ToObject();
    }
    #endregion
}
