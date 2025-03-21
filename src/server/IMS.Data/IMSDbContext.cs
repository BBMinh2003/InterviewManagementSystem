using System;
using IMS.Core.Constants;
using IMS.Core.Enums;
using IMS.Models.Common;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));

    }
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
        builder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), CreatedAt = DateTime.Parse("2023-10-01"), Name = "Admin", NormalizedName = "ADMIN" },
            new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), CreatedAt = DateTime.Parse("2023-10-01"), Name = "Recruiter", NormalizedName = "RECRUITER" },
            new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt = DateTime.Parse("2023-10-01"), Name = "Interviewer", NormalizedName = "INTERVIEWER" },
            new Role { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), CreatedAt = DateTime.Parse("2023-10-01"), Name = "Manager", NormalizedName = "Manager" }
        );

        builder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserName = "admin",
                FullName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEKzO7BHEb758wHgIVI3x0NuSEd8BLlECa+TDvKAF1cUtkj6O5hM9PMp42jCeWnGeww==",
                IsActive = true,
                SecurityStamp = "SECURITY_STAMP_1",// Giá trị tĩnh
            },
            new User
            {
                Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                UserName = "recruiter",
                FullName = "Recruiter",
                NormalizedUserName = "RECRUITER",
                Email = "recruiter@example.com",
                NormalizedEmail = "RECRUITER@EXAMPLE.COM",
                EmailConfirmed = true,
                CreatedAt = new DateTime(2024, 10, 1),
                PasswordHash = "AQAAAAEAACcQAAAAEOTNmM1M0OJV+VJyKbHIj8b7oJSH/W5uTr8LQy8HO8bhEIb9ZDf9m1KwnBavT5m9Yg==",
                IsActive = true,
                SecurityStamp = "SECURITY_STAMP_2" // Giá trị tĩnh
            },
            new User
            {
                Id = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc"),
                UserName = "interviewer",
                FullName = "Interviewer",
                NormalizedUserName = "INTERVIEWER",
                Email = "interviewer@example.com",
                NormalizedEmail = "INTERVIEWER@EXAMPLE.COM",
                EmailConfirmed = true,
                CreatedAt = new DateTime(2024, 10, 1),
                PasswordHash = "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==",
                IsActive = true,
                SecurityStamp = "SECURITY_STAMP_3" // Giá trị tĩnh
            },
            new User
            {
                Id = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd"),
                UserName = "manager",
                FullName = "Manager",
                NormalizedUserName = "MANAGER",
                Email = "manager@example.com",
                NormalizedEmail = "MANAGER@EXAMPLE.COM",
                EmailConfirmed = true,
                CreatedAt = new DateTime(2024, 10, 1),
                PasswordHash = "AQAAAAEAACcQAAAAELbr2xCUqc37Qu/fRYpRYOQTzUtPnCVXx7muwkJEhUlRlhGuAGD2kJzcIokmv4YrZQ==",
                IsActive = true,
                SecurityStamp = "SECURITY_STAMP_4" // Giá trị tĩnh
            },
            new User
            {
                Id = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee"),
                UserName = "interview2",
                FullName = "Interviewer 2",
                NormalizedUserName = "INTERVIEW2",
                Email = "interview2@example.com",
                NormalizedEmail = "INTERVIEW2@EXAMPLE.COM",
                EmailConfirmed = true,
                CreatedAt = new DateTime(2024, 10, 1),
                PasswordHash = "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==",
                IsActive = true,
                SecurityStamp = "SECURITY_STAMP_5" // Giá trị tĩnh
            },
            new User
            {
                Id = Guid.Parse("66666666-ffff-ffff-ffff-ffffffffffff"),
                UserName = "interviewer3",
                FullName = "Interviewer 3",
                NormalizedUserName = "INTERVIEWER3",
                Email = "interviewer3@example.com",
                NormalizedEmail = "INTERVIEWER3@EXAMPLE.COM",
                EmailConfirmed = true,
                CreatedAt = new DateTime(2024, 10, 1),
                PasswordHash = "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==",
                SecurityStamp = "SECURITY_STAMP_6", // Giá trị tĩnh
                IsActive = true,

            }
        );
        // Seed data cho Candidate
        builder.Entity<Candidate>().HasData(
            new Candidate
            {
                Id = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                Gender = Gender.MALE,
                CV_Attachment = "john_doe_cv.pdf",
                HighestLevel = HighestLevel.BachelorsDegree,
                PositionId = Guid.Parse("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Backend Developer
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Recruiter
            },
            new Candidate
            {
                Id = Guid.Parse("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                Phone = "0987654321",
                DateOfBirth = new DateTime(1992, 2, 2),
                Address = "456 Elm St",
                Gender = Gender.FEMALE,
                CV_Attachment = "jane_smith_cv.pdf",
                HighestLevel = HighestLevel.MastersDegree,
                PositionId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Business Analyst
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Recruiter
            },
            new Candidate
            {
                Id = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee"),
                Name = "Alice Brown",
                Email = "alice.brown@example.com",
                Phone = "1112223333",
                DateOfBirth = new DateTime(1995, 5, 5),
                Address = "789 Maple St",
                Gender = Gender.FEMALE,
                CV_Attachment = "alice_brown_cv.pdf",
                HighestLevel = HighestLevel.PhD,
                PositionId = Guid.Parse("99999999-cccc-cccc-cccc-cccccccccccc"), // Tester
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Recruiter
            },
            new Candidate
            {
                Id = Guid.Parse("66666666-ffff-ffff-ffff-ffffffffffff"),
                Name = "Bob Johnson",
                Email = "bob.johnson@example.com",
                Phone = "4445556666",
                DateOfBirth = new DateTime(1988, 8, 8),
                Address = "321 Oak St",
                Gender = Gender.MALE,
                CV_Attachment = "bob_johnson_cv.pdf",
                HighestLevel = HighestLevel.HighSchool,
                PositionId = Guid.Parse("bbbbbbbb-eeee-eeee-eeee-eeeeeeeeeeee"), // Project Manager
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Recruiter
            }
        );

        // Seed data cho UserRoles
        builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") }, // Admin có role Admin
            new IdentityUserRole<Guid> { UserId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222") }, // Recruiter có role Recruiter
            new IdentityUserRole<Guid> { UserId = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc"), RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333") }, // Interviewer có role Interviewer
            new IdentityUserRole<Guid> { UserId = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd"), RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444") }, // Manager có role Manager

            // 🆕 Thêm user có nhiều role
            new IdentityUserRole<Guid> { UserId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444") }, // Admin có thêm role Manager
            new IdentityUserRole<Guid> { UserId = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc"), RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222") }  // Interviewer cũng là Recruiter
        );


        // Seed data cho job
        builder.Entity<Job>().HasData(
            new Job
            {
                Id = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Title = "Software Engineer",
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 10, 1),
                MinSalary = 1000.00m,
                MaxSalary = 3000.00m,
                WorkingAddress = "123 Tech Street",
                Description = "Develop and maintain web applications.",
                Status = JobStatus.Open
            },
            new Job
            {
                Id = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Title = "Project Manager",
                StartDate = new DateTime(2025, 5, 1),
                EndDate = new DateTime(2025, 12, 1),
                MinSalary = 2000.00m,
                MaxSalary = 5000.00m,
                WorkingAddress = "456 Business Road",
                Description = "Manage project teams and oversee development processes.",
                Status = JobStatus.Open
            },
            new Job
            {
                Id = Guid.Parse("77777777-cccc-cccc-cccc-cccccccccccc"),
                Title = "Data Analyst",
                StartDate = new DateTime(2025, 6, 1),
                EndDate = new DateTime(2025, 11, 1),
                MinSalary = 1500.00m,
                MaxSalary = 4000.00m,
                WorkingAddress = "789 Data Lane",
                Description = "Analyze business data and provide insights.",
                Status = JobStatus.Closed
            },
            new Job
            {
                Id = Guid.Parse("66666666-dddd-dddd-dddd-dddddddddddd"),
                Title = "Team Lead",
                StartDate = new DateTime(2025, 7, 1),
                EndDate = new DateTime(2025, 12, 1),
                MinSalary = 2500.00m,
                MaxSalary = 6000.00m,
                WorkingAddress = "321 Lead Street",
                Description = "Lead a team of developers and manage projects.",
                Status = JobStatus.Open
            },
            new Job
            {
                Id = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeed"),
                Title = "Backend Developer",
                StartDate = new DateTime(2025, 8, 1),
                EndDate = new DateTime(2025, 12, 1),
                MinSalary = 1500.00m,
                MaxSalary = 4000.00m,
                WorkingAddress = "789 Data Lane",
                Description = "Analyze business data and provide insights.",
                Status = JobStatus.Closed
            }
        );

        // Seed data cho Candidate skill
        builder.Entity<CandidateSkill>().HasData(
            new CandidateSkill
            {
                CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // John Doe
                SkillId = Guid.Parse("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // Java
            },
            new CandidateSkill
            {
                CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // John Doe
                SkillId = Guid.Parse("ffffffff-cccc-cccc-cccc-cccccccccccc") // .NET
            },
            new CandidateSkill
            {
                CandidateId = Guid.Parse("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Jane Smith
                SkillId = Guid.Parse("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Node.js
            },
            new CandidateSkill
            {
                CandidateId = Guid.Parse("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Jane Smith
                SkillId = Guid.Parse("22222222-eeee-eeee-eeee-eeeeeeeeeeee") // Business analysis
            },
            new CandidateSkill
            {
                CandidateId = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), // Alice Brown
                SkillId = Guid.Parse("33333333-ffff-ffff-ffff-ffffffffffff") // Communication
            },
            new CandidateSkill
            {
                CandidateId = Guid.Parse("66666666-ffff-ffff-ffff-ffffffffffff"), // Bob Johnson
                SkillId = Guid.Parse("11111111-dddd-dddd-dddd-dddddddddddd") // C++
            }
        );

        // Seed data cho Job benefits
        builder.Entity<JobBenefit>().HasData(
            new JobBenefit
            {
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                BenefitId = Guid.Parse("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // Lunch
            },
            new JobBenefit
            {
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                BenefitId = Guid.Parse("66666666-cccc-cccc-cccc-cccccccccccc") // Healthcare insurance
            },
            new JobBenefit
            {
                JobId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Project Manager
                BenefitId = Guid.Parse("55555555-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // 25-day leave
            },
            new JobBenefit
            {
                JobId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Project Manager
                BenefitId = Guid.Parse("77777777-dddd-dddd-dddd-dddddddddddd") // Hybrid working
            },
            new JobBenefit
            {
                JobId = Guid.Parse("77777777-cccc-cccc-cccc-cccccccccccc"), // Data Analyst
                BenefitId = Guid.Parse("88888888-eeee-eeee-eeee-eeeeeeeeeeee") // Travel
            }
        );

        // Seed data cho job level
        builder.Entity<JobLevel>().HasData(
            new JobLevel
            {
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                LevelId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // Fresher
            },
            new JobLevel
            {
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                LevelId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Junior
            },
            new JobLevel
            {
                JobId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Project Manager
                LevelId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") // Manager
            },
            new JobLevel
            {
                JobId = Guid.Parse("77777777-cccc-cccc-cccc-cccccccccccc"), // Data Analyst
                LevelId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc") // Senior
            },
            new JobLevel
            {
                JobId = Guid.Parse("66666666-dddd-dddd-dddd-dddddddddddd"), // Team Lead
                LevelId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd") // Leader
            }
        );

        // Seed data cho job skill
        builder.Entity<JobSkill>().HasData(
            new JobSkill
            {
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                SkillId = Guid.Parse("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // Java
            },
            new JobSkill
            {
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                SkillId = Guid.Parse("ffffffff-cccc-cccc-cccc-cccccccccccc") // .NET
            },
            new JobSkill
            {
                JobId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Project Manager
                SkillId = Guid.Parse("22222222-eeee-eeee-eeee-eeeeeeeeeeee") // Business analysis
            },
            new JobSkill
            {
                JobId = Guid.Parse("77777777-cccc-cccc-cccc-cccccccccccc"), // Data Analyst
                SkillId = Guid.Parse("33333333-ffff-ffff-ffff-ffffffffffff") // Communication
            },
            new JobSkill
            {
                JobId = Guid.Parse("66666666-dddd-dddd-dddd-dddddddddddd"), // Backend Developer
                SkillId = Guid.Parse("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Node.js
            }
        );

        // Seed data cho Interview
        builder.Entity<Interview>().HasData(
            new Interview
            {
                Id = Guid.Parse("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"),
                CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // John Doe
                JobId = Guid.Parse("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Software Engineer
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
                Title = "Technical Interview Round 1",
                Note = "Kiểm tra kỹ năng lập trình cơ bản",
                Location = "Văn phòng công ty",
                MeetingUrl = null,
                Result = null,
                Status = InterviewStatus.Cancelled,
                StartAt = new TimeOnly(9, 30),
                EndAt = new TimeOnly(10, 30)
            },
            new Interview
            {
                Id = Guid.Parse("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"),
                CandidateId = Guid.Parse("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Jane Smith
                JobId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Project Manager
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
                Title = "Management Interview",
                Note = "Kiểm tra kỹ năng quản lý nhóm",
                Location = null,
                MeetingUrl = "https://meet.example.com/jane-smith",
                Result = null,
                Status = InterviewStatus.Invited,
                StartAt = new TimeOnly(14, 00),
                EndAt = new TimeOnly(15, 00)
            },
            new Interview
            {
                Id = Guid.Parse("cccc3333-3333-3333-3333-cccccccccccc"),
                CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // John Doe
                JobId = Guid.Parse("77777777-cccc-cccc-cccc-cccccccccccc"), // Backend Developer
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
                Title = "Backend Technical Round",
                Note = "Phỏng vấn sâu về kiến thức backend",
                Location = "Văn phòng Hà Nội",
                MeetingUrl = null,
                Result = null,
                Status = InterviewStatus.New,
                StartAt = new TimeOnly(10, 00),
                EndAt = new TimeOnly(11, 00)
            },
            new Interview
            {
                Id = Guid.Parse("dddd4444-4444-4444-4444-dddddddddddd"),
                CandidateId = Guid.Parse("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Jane Smith
                JobId = Guid.Parse("66666666-dddd-dddd-dddd-dddddddddddd"), // HR Specialist
                RecruiterOwnerId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Admin
                Title = "HR Round",
                Note = "Phỏng vấn về văn hóa công ty và thái độ làm việc",
                Location = "Hội trường A",
                MeetingUrl = null,
                Result = "Passed",
                Status = InterviewStatus.Interviewed,
                StartAt = new TimeOnly(13, 30),
                EndAt = new TimeOnly(14, 30)
            },
            new Interview
            {
                Id = Guid.Parse("eeee5555-5555-5555-5555-eeeeeeeeeeee"),
                CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // John Doe
                JobId = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeed"), // Frontend Developer
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
                Title = "Frontend Coding Challenge",
                Note = "Bài test live coding React.js",
                Location = null,
                MeetingUrl = "https://meet.example.com/frontend-test",
                Result = null,
                Status = InterviewStatus.Cancelled,
                StartAt = new TimeOnly(16, 00),
                EndAt = new TimeOnly(17, 00)
            },
            new Interview
            {
                Id = Guid.Parse("ffff6666-6666-6666-6666-ffffffffffff"),
                CandidateId = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), // Alice Brown
                JobId = Guid.Parse("77777777-cccc-cccc-cccc-cccccccccccc"), // Data Analyst
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
                Title = "Data Analysis Test",
                Note = "Bài test phân tích dữ liệu",
                Location = "Phòng họp 2",
                MeetingUrl = null,
                Result = null,
                Status = InterviewStatus.New,
                StartAt = new TimeOnly(15, 00),
                EndAt = new TimeOnly(16, 00)
            }
        );

        // Seed data cho IntervewerInterview
        builder.Entity<IntervewerInterview>().HasData(
            new IntervewerInterview
            {
                InterviewId = Guid.Parse("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), // Technical Interview Round 1
                UserId = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc") // Interviewer 1
            },
            new IntervewerInterview
            {
                InterviewId = Guid.Parse("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), // Management Interview
                UserId = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd") // Manager
            },
            new IntervewerInterview
            {
                InterviewId = Guid.Parse("cccc3333-3333-3333-3333-cccccccccccc"), // Backend Technical Round
                UserId = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee") // Interviewer 2
            },
            new IntervewerInterview
            {
                InterviewId = Guid.Parse("dddd4444-4444-4444-4444-dddddddddddd"), // HR Round
                UserId = Guid.Parse("66666666-ffff-ffff-ffff-ffffffffffff") // Interviewer 3
            },
            new IntervewerInterview
            {
                InterviewId = Guid.Parse("eeee5555-5555-5555-5555-eeeeeeeeeeee"), // Final Interview
                UserId = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc") // Interviewer 1
            },
            new IntervewerInterview
            {
                InterviewId = Guid.Parse("ffff6666-6666-6666-6666-ffffffffffff"), // Additional Interview
                UserId = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd") // Manager
            }
        );

        // Seed data cho offer
        builder.Entity<Offer>().HasData(
            new Offer
            {
                Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PositionId = Guid.Parse("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Backend Developer
                CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Sửa lại CandidateId hợp lệ
                DepartmentId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // IT
                RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
                ContactTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Trial 2 months
                InterviewId = Guid.Parse("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), // Technical Interview Round 1
                Note = "Offer for Backend Developer position",
                Status = OfferStatus.Declined,
                LevelId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Junior
                BasicSalary = 1500.00m,
                DueDate = DateTime.UtcNow.AddDays(7),
                ContactPeriodFrom = DateTime.UtcNow,
                ContactPeriodTo = DateTime.UtcNow.AddYears(1),
                ApprovedById = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd") // Manager
            },
        new Offer
        {
            Id = Guid.Parse("22222222-cccc-cccc-cccc-cccccccccccc"),
            PositionId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Business Analyst
            CandidateId = Guid.Parse("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Sửa lại CandidateId hợp lệ
            DepartmentId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // HR
            RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
            ContactTypeId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Trainee 3 months
            InterviewId = Guid.Parse("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), // Management Interview
            Note = "Offer for Business Analyst position",
            Status = OfferStatus.Accepted,
            LevelId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), // Senior
            BasicSalary = 2000.00m,
            DueDate = DateTime.UtcNow.AddDays(10),
            ContactPeriodFrom = DateTime.UtcNow,
            ContactPeriodTo = DateTime.UtcNow.AddYears(2),
            ApprovedById = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd") // Manager
        },
        new Offer
        {
            Id = Guid.Parse("33333333-dddd-dddd-dddd-dddddddddddd"),
            PositionId = Guid.Parse("99999999-cccc-cccc-cccc-cccccccccccc"), // Tester
            CandidateId = Guid.Parse("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), // Sửa lại CandidateId hợp lệ
            DepartmentId = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc"), // Finance
            RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
            ContactTypeId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // 1 year
            InterviewId = Guid.Parse("cccc3333-3333-3333-3333-cccccccccccc"), // Backend Technical Round
            Note = "Offer for Tester position",
            Status = OfferStatus.Rejected,
            LevelId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Fresher
            BasicSalary = 1200.00m,
            DueDate = DateTime.UtcNow.AddDays(5),
            ContactPeriodFrom = DateTime.UtcNow,
            ContactPeriodTo = DateTime.UtcNow.AddYears(1),
            ApprovedById = Guid.Parse("33333333-cccc-cccc-cccc-cccccccccccc") // Interviewer
        },
        new Offer
        {
            Id = Guid.Parse("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
            PositionId = Guid.Parse("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Backend Developer
            CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Candidate đã có Offer trước đó
            DepartmentId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // IT
            RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
            ContactTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Trial 2 months
            InterviewId = Guid.Parse("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), // Technical Interview Round 1
            Note = "Second offer for Backend Developer position",
            Status = OfferStatus.Accepted,
            LevelId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Junior
            BasicSalary = 1600.00m,
            DueDate = DateTime.UtcNow.AddDays(14),
            ContactPeriodFrom = DateTime.UtcNow,
            ContactPeriodTo = DateTime.UtcNow.AddYears(1),
            ApprovedById = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd") // Manager
        },
        new Offer
        {
            Id = Guid.Parse("55555555-ffff-ffff-ffff-ffffffffffff"),
            PositionId = Guid.Parse("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Business Analyst
            CandidateId = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Cùng Candidate nhưng Offer khác
            DepartmentId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // HR
            RecruiterOwnerId = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Recruiter
            ContactTypeId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Trainee 3 months
            InterviewId = Guid.Parse("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), // Management Interview
            Note = "Second offer for Business Analyst position",
            Status = OfferStatus.Accepted,
            LevelId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), // Senior
            BasicSalary = 2100.00m,
            DueDate = DateTime.UtcNow.AddDays(10),
            ContactPeriodFrom = DateTime.UtcNow,
            ContactPeriodTo = DateTime.UtcNow.AddYears(2),
            ApprovedById = Guid.Parse("44444444-dddd-dddd-dddd-dddddddddddd") // Manager
        });
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
