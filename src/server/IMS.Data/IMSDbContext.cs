using System;
using IMS.Core.Constants;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data;

public class IMSDbContext : IdentityDbContext<User, Role, Guid>
{
    public IMSDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename Identity tables
        builder.Entity<User>().ToTable("Users", CoreConstants.Schemas.Security);
        builder.Entity<Role>().ToTable("Roles", CoreConstants.Schemas.Security);
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", CoreConstants.Schemas.Security);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", CoreConstants.Schemas.Security);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", CoreConstants.Schemas.Security);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", CoreConstants.Schemas.Security);
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", CoreConstants.Schemas.Security);
        builder.Entity<User>().HasQueryFilter(x => !x.DeletedAt.HasValue);
        builder.Entity<Role>().HasQueryFilter(x => !x.DeletedAt.HasValue);
    }
    public override int SaveChanges()
    {
        BeforeSaveChange();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        BeforeSaveChange();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void BeforeSaveChange()
    {
        var entities = this.ChangeTracker.Entries<IBaseEntity>();

        foreach (var item in entities)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    item.Entity.CreatedAt = DateTime.Now;
                    break;
                case EntityState.Modified:
                    item.Entity.UpdatedAt = DateTime.Now;
                    break;
            }
        }
    }
}
