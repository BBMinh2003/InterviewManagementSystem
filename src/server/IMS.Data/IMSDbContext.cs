using System;
using IMS.Core.Constants;
using IMS.Core.Enums;
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

        builder.Entity<BaseEntity>()
            .HasOne(i => i.CreatedBy)
            .WithMany()
            .HasForeignKey(i => i.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<BaseEntity>()
            .HasOne(i => i.DeletedBy)
            .WithMany()
            .HasForeignKey(i => i.DeletedById)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<BaseEntity>()
            .HasOne(i => i.UpdatedBy)
            .WithMany()
            .HasForeignKey(i => i.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Interview>(entity =>
        {
            entity.HasOne(i => i.Candidate)
                .WithMany()
                .HasForeignKey(i => i.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Job)
                .WithMany()
                .HasForeignKey(i => i.JobId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        builder.Entity<IntervewerInterview>()
            .HasOne(ii => ii.User)
            .WithMany()
            .HasForeignKey(ii => ii.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Offer>()
            .HasOne(o => o.Candidate)
            .WithMany()
            .HasForeignKey(o => o.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Offer>()
            .HasOne(o => o.Interview)
            .WithMany()
            .HasForeignKey(o => o.InterviewId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Offer>()
            .HasOne(o => o.RecruiterOwner)
            .WithMany()
            .HasForeignKey(o => o.RecruiterOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data cho ContractType
        builder.Entity<ContactType>().HasData(
            new ContactType { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Trial 2 months" },
            new ContactType { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Trainee 3 months" },
            new ContactType { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "1 year" },
            new ContactType { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "3 years" },
            new ContactType { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Unlimited" }
        );

        // Seed data cho Level
        builder.Entity<Level>().HasData(
            new Level { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Fresher" },
            new Level { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Junior" },
            new Level { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Senior" },
            new Level { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Leader" },
            new Level { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Manager" },
            new Level { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Vice Head" }
        );

        // Seed data cho Department
        builder.Entity<Department>().HasData(
            new Department { Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "IT" },
            new Department { Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "HR" },
            new Department { Id = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc"), Name = "Finance" },
            new Department { Id = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd"), Name = "Communication" },
            new Department { Id = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Marketing" },
            new Department { Id = Guid.Parse("66666666-ffff-ffff-ffff-ffffffffffff"), Name = "Accounting" }
        );

        // Seed data cho Position
        builder.Entity<Position>().HasData(
            new Position { Id = Guid.Parse("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Backend Developer" },
            new Position { Id = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Business Analyst" },
            new Position { Id = Guid.Parse("99999999-cccc-cccc-cccc-cccccccccccc"), Name = "Tester" },
            new Position { Id = Guid.Parse("aaaaaaaa-dddd-dddd-dddd-dddddddddddd"), Name = "HR" },
            new Position { Id = Guid.Parse("bbbbbbbb-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Project Manager" },
            new Position { Id = Guid.Parse("cccccccc-ffff-ffff-ffff-ffffffffffff"), Name = "Not available" }
        );

        // Seed data cho Skill
        builder.Entity<Skill>().HasData(
            new Skill { Id = Guid.Parse("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Java" },
            new Skill { Id = Guid.Parse("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Node.js" },
            new Skill { Id = Guid.Parse("ffffffff-cccc-cccc-cccc-cccccccccccc"), Name = ".NET" },
            new Skill { Id = Guid.Parse("11111111-dddd-dddd-dddd-dddddddddddd"), Name = "C++" },
            new Skill { Id = Guid.Parse("22222222-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Business analysis" },
            new Skill { Id = Guid.Parse("33333333-ffff-ffff-ffff-ffffffffffff"), Name = "Communication" }
        );

        // Seed data cho Benefit
        builder.Entity<Benefit>().HasData(
            new Benefit { Id = Guid.Parse("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Lunch" },
            new Benefit { Id = Guid.Parse("55555555-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "25-day leave" },
            new Benefit { Id = Guid.Parse("66666666-cccc-cccc-cccc-cccccccccccc"), Name = "Healthcare insurance" },
            new Benefit { Id = Guid.Parse("77777777-dddd-dddd-dddd-dddddddddddd"), Name = "Hybrid working" },
            new Benefit { Id = Guid.Parse("88888888-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Travel" }
        );

        //Seed data cho role
        
        //Seed data cho user
        //Seed data cho candidate
        //Seed data cho job
        //Seed data cho Candidate skill
        //Seed data cho job
        //Seed data cho Job benefits
        //Seed data cho job level
        //Seed data cho job skill
        //Seed data cho Interview
        //Seed data cho inerviewerInterview
        //Seed data cho offer
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
