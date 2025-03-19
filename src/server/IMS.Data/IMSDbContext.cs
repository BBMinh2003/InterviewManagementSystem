using System;
using IMS.Core.Constants;
using IMS.Models.Common;
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
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<ContactType> ContactTypes { get; set; }



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

        // Common
        builder.Entity<Job>().ToTable("Jobs", CoreConstants.Schemas.Common);
        builder.Entity<Department>().ToTable("Departments", CoreConstants.Schemas.Common);
        builder.Entity<Position>().ToTable("Positions", CoreConstants.Schemas.Common);
        builder.Entity<Candidate>().ToTable("Candidates", CoreConstants.Schemas.Common);
        builder.Entity<Interview>().ToTable("Interviews", CoreConstants.Schemas.Common);
        builder.Entity<Offer>().ToTable("Offers", CoreConstants.Schemas.Common);
        builder.Entity<Level>().ToTable("Levels", CoreConstants.Schemas.Common);
        builder.Entity<ContactType>().ToTable("ContactTypes", CoreConstants.Schemas.Common);
        
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
