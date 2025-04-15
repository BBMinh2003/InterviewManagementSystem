using System;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using IMS.Core.Enums;
using IMS.Models.Common;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IMS.Data.SeedData;

public static class DbInitializer
{
    public static async Task Initialize(IMSDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager, string rolesJsonPath, string usersJsonPath)
    {
        await context.Database.EnsureCreatedAsync();

        await SeedContactTypeAsync(context);
        await SeedLevelAsync(context);
        await SeedDepartmentAsync(context);
        await SeedPositionAsync(context);
        await SeedSkillAsync(context);
        await SeedBenefitAsync(context);
        await SeedUserAndRoleAsync(context, roleManager, userManager, rolesJsonPath, usersJsonPath);
        await SeedCandidateAsync(context);
        await SeedJobAsync(context);
        await SeedInterviewAsync(context);
        await SeedOfferAsync(context);

    }

    private static async Task SeedUserAndRoleAsync(IMSDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager, string rolesJsonPath, string usersJsonPath)
    {
        await context.Database.EnsureCreatedAsync();

        string jsonRoles = await File.ReadAllTextAsync(rolesJsonPath);
        var roles = JsonConvert.DeserializeObject<List<Role>>(jsonRoles);

        string jsonUsers = await File.ReadAllTextAsync(usersJsonPath);
        var users = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonUsers);

        if (roles == null || users == null)
        {
            return;
        }

        await SeedUserAndRoles(userManager, roleManager, users, roles);

        await context.SaveChangesAsync();
    }

    private static async Task SeedUserAndRoles(
     UserManager<User> userManager,
     RoleManager<Role> roleManager,
     List<UserViewModel> users,
     List<Role> roles)
    {
        if (!await userManager.Users.AnyAsync(x => x.UserName == "admin"))
        {
            if (users == null || roles == null)
            {
                return;
            }

            await SeedRolesAsync(roleManager, roles);
            await SeedUsersAsync(userManager, users);
        }
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager, List<Role> roles)
    {
        foreach (var role in roles)
        {
            if (string.IsNullOrWhiteSpace(role.Name)) continue;

            var existingRole = await roleManager.FindByNameAsync(role.Name);
            if (existingRole == null)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager, List<UserViewModel> users)
    {
        var passwordHasher = new PasswordHasher<User>();

        foreach (var userVm in users)
        {
            var existingUser = await userManager.FindByIdAsync(userVm.Id.ToString());
            if (existingUser == null)
            {
                var newUser = new User
                {
                    Id = userVm.Id,
                    UserName = userVm.UserName,
                    FullName = userVm.FullName ?? "",
                    NormalizedUserName = userVm.NormalizedUserName,
                    Email = userVm.Email ?? "",
                    NormalizedEmail = userVm.NormalizedEmail,
                    EmailConfirmed = true,
                    CreatedAt = userVm.CreatedAt,
                    IsActive = true,
                    SecurityStamp = userVm.SecurityStamp
                };

                newUser.PasswordHash = passwordHasher.HashPassword(newUser, userVm.Password);

                var result = await userManager.CreateAsync(newUser);

                if (result.Succeeded && !string.IsNullOrEmpty(userVm.Role))
                {
                    await userManager.AddToRoleAsync(newUser, userVm.Role);
                }
            }
        }
    }

    private static async Task SeedOfferAsync(IMSDbContext context)
    {
        if (!await context.Offers.AnyAsync())
        {
            context.Offers.AddRange(
                new Offer
                {
                    Id = Guid.Parse("212914e1-c4bc-4d48-84b6-35f6c80e2469"),
                    PositionId = Guid.Parse("cb88b24f-76a4-41bd-9283-9d69984b9d80"),
                    CandidateId = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    DepartmentId = Guid.Parse("2169f928-3fe6-49d1-ada3-7ed50efe857e"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    ContactTypeId = Guid.Parse("1577075e-0531-48a4-b425-3436a7dca48b"),
                    InterviewId = Guid.Parse("3bb50954-7677-4c62-8073-cbf864ea7a85"),
                    Note = "Offer for Backend Developer position",
                    Status = OfferStatus.Declined,
                    LevelId = Guid.Parse("4dd558f1-6b3d-4cdd-99c9-6b4a28f78004"),
                    BasicSalary = 1500.00m,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    ContactPeriodFrom = DateTime.UtcNow,
                    ContactPeriodTo = DateTime.UtcNow.AddYears(1),
                    ApprovedById = Guid.Parse("e9c215a3-ebf3-46df-8337-b6945d4ae194")
                },
                new Offer
                {
                    Id = Guid.Parse("3dd91c11-2279-48f5-8aa3-cbc744a4813f"),
                    PositionId = Guid.Parse("d6934e7c-8749-413a-8dc4-0ac922218ade"),
                    CandidateId = Guid.Parse("131915c0-8140-4bd4-8422-c5d03ed1c8e0"),
                    DepartmentId = Guid.Parse("16dfd695-091a-4230-9d6f-490d055d4145"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    ContactTypeId = Guid.Parse("2804d947-2bda-491d-93af-957dad2ce355"),
                    InterviewId = Guid.Parse("6c37a56c-2b4b-4d7d-8d25-e0f1f9ed1b10"),
                    Note = "Offer for Business Analyst position",
                    Status = OfferStatus.Accepted,
                    LevelId = Guid.Parse("6290dfca-5c9d-47ef-bd19-298788fdab02"),
                    BasicSalary = 2000.00m,
                    DueDate = DateTime.UtcNow.AddDays(10),
                    ContactPeriodFrom = DateTime.UtcNow,
                    ContactPeriodTo = DateTime.UtcNow.AddYears(2),
                    ApprovedById = Guid.Parse("e9c215a3-ebf3-46df-8337-b6945d4ae194")
                },
                new Offer
                {
                    Id = Guid.Parse("7d04b39a-7570-4213-87f4-0f1a1101f3ae"),
                    PositionId = Guid.Parse("5f8702e0-e366-4979-b7be-d5a9f8bd15a6"),
                    CandidateId = Guid.Parse("3d1a7327-276e-4283-a699-db16b5ea8427"),
                    DepartmentId = Guid.Parse("7f1452da-5942-4c78-82a3-3bb606e68b94"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    ContactTypeId = Guid.Parse("a6713097-3064-4613-870d-155a85a65956"),
                    InterviewId = Guid.Parse("b4233c1a-aa46-4f4b-aa00-0c8729f5a472"),
                    Note = "Offer for Tester position",
                    Status = OfferStatus.Rejected,
                    LevelId = Guid.Parse("898d0e0f-45da-4ce3-9d38-a96cd67efabc"),
                    BasicSalary = 1200.00m,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    ContactPeriodFrom = DateTime.UtcNow,
                    ContactPeriodTo = DateTime.UtcNow.AddYears(1),
                    ApprovedById = Guid.Parse("afa3bef6-4aee-4eb9-b4c7-9b259dc997b4")
                },
                new Offer
                {
                    Id = Guid.Parse("47edda24-a59a-4560-a674-fd2cd0c5e905"),
                    PositionId = Guid.Parse("cb88b24f-76a4-41bd-9283-9d69984b9d80"),
                    CandidateId = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    DepartmentId = Guid.Parse("2169f928-3fe6-49d1-ada3-7ed50efe857e"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    ContactTypeId = Guid.Parse("1577075e-0531-48a4-b425-3436a7dca48b"),
                    InterviewId = Guid.Parse("3bb50954-7677-4c62-8073-cbf864ea7a85"),
                    Note = "Second offer for Backend Developer position",
                    Status = OfferStatus.Accepted,
                    LevelId = Guid.Parse("4dd558f1-6b3d-4cdd-99c9-6b4a28f78004"),
                    BasicSalary = 1600.00m,
                    DueDate = DateTime.UtcNow.AddDays(14),
                    ContactPeriodFrom = DateTime.UtcNow,
                    ContactPeriodTo = DateTime.UtcNow.AddYears(1),
                    ApprovedById = Guid.Parse("e9c215a3-ebf3-46df-8337-b6945d4ae194")
                },
                new Offer
                {
                    Id = Guid.Parse("0470d312-9e0b-4a8b-afad-8ebce188a349"),
                    PositionId = Guid.Parse("d6934e7c-8749-413a-8dc4-0ac922218ade"),
                    CandidateId = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    DepartmentId = Guid.Parse("16dfd695-091a-4230-9d6f-490d055d4145"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    ContactTypeId = Guid.Parse("2804d947-2bda-491d-93af-957dad2ce355"),
                    InterviewId = Guid.Parse("6c37a56c-2b4b-4d7d-8d25-e0f1f9ed1b10"),
                    Note = "Second offer for Business Analyst position",
                    Status = OfferStatus.Accepted,
                    LevelId = Guid.Parse("b416602b-fb2c-495e-a022-0445d8212192"),
                    BasicSalary = 2100.00m,
                    DueDate = DateTime.UtcNow.AddDays(10),
                    ContactPeriodFrom = DateTime.UtcNow,
                    ContactPeriodTo = DateTime.UtcNow.AddYears(2),
                    ApprovedById = Guid.Parse("e9c215a3-ebf3-46df-8337-b6945d4ae194")
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedInterviewAsync(IMSDbContext context)
    {
        string MeetingUrlBase = " https://meet.example.com/";
        string IsoDate = "yyyy-MM-dd";
        if (!await context.Interviews.AnyAsync())
        {
            var interviews = new List<Interview>
            {
                new Interview
                {
                    Id = Guid.Parse("3bb50954-7677-4c62-8073-cbf864ea7a85"),
                    CandidateId = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    JobId = Guid.Parse("5b8566ab-7a52-4795-b798-abc9bfd6066d"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    Title = "Technical Interview Round 1",
                    Note = "Kiểm tra kỹ năng lập trình cơ bản",
                    Location = "Văn phòng công ty",
                    MeetingUrl = null,
                    Result = Result.NotApplicable,
                    Status = InterviewStatus.Cancelled,
                    StartAt = new TimeOnly(9, 30),
                    EndAt = new TimeOnly(10, 30),
                    InterviewDate = DateOnly.ParseExact("2025-04-01", IsoDate, CultureInfo.InvariantCulture),
                    Interviewers = new List<IntervewerInterview>
                    {
                        new IntervewerInterview
                        {
                            InterviewId = Guid.Parse("3bb50954-7677-4c62-8073-cbf864ea7a85"),
                            UserId = Guid.Parse("afa3bef6-4aee-4eb9-b4c7-9b259dc997b4")
                        }
                    }
                },
                new Interview
                {
                    Id = Guid.Parse("6c37a56c-2b4b-4d7d-8d25-e0f1f9ed1b10"),
                    CandidateId = Guid.Parse("131915c0-8140-4bd4-8422-c5d03ed1c8e0"),
                    JobId = Guid.Parse("b3a5568c-9250-44b9-bb35-e0a61bedecf4"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    Title = "Management Interview",
                    Note = "Kiểm tra kỹ năng quản lý nhóm",
                    Location = null,
                    MeetingUrl = $"{MeetingUrlBase}"+"jane-smith",
                    Result = Result.Failed,
                    Status = InterviewStatus.Invited,
                    StartAt = new TimeOnly(14, 00),
                    EndAt = new TimeOnly(15, 00),
                    InterviewDate = DateOnly.ParseExact("2025-04-02", IsoDate, CultureInfo.InvariantCulture),
                    Interviewers = new List<IntervewerInterview>
                    {
                        new IntervewerInterview
                        {
                            InterviewId = Guid.Parse("6c37a56c-2b4b-4d7d-8d25-e0f1f9ed1b10"),
                            UserId = Guid.Parse("e9c215a3-ebf3-46df-8337-b6945d4ae194")
                        }
                    }
                },
                new Interview
                {
                    Id = Guid.Parse("b4233c1a-aa46-4f4b-aa00-0c8729f5a472"),
                    CandidateId = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    JobId = Guid.Parse("64915173-1282-483d-ae82-d21879049ddd"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    Title = "Backend Technical Round",
                    Note = "Phỏng vấn sâu về kiến thức backend",
                    Location = "Văn phòng Hà Nội",
                    MeetingUrl = null,
                    Result = Result.NotApplicable,
                    Status = InterviewStatus.New,
                    StartAt = new TimeOnly(10, 00),
                    EndAt = new TimeOnly(11, 00),
                    InterviewDate = DateOnly.ParseExact("2025-04-03", IsoDate, CultureInfo.InvariantCulture),
                    Interviewers = new List<IntervewerInterview>
                    {
                        new IntervewerInterview
                        {
                            InterviewId = Guid.Parse("b4233c1a-aa46-4f4b-aa00-0c8729f5a472"),
                            UserId = Guid.Parse("610dbe77-2f4f-4ba5-afd9-717efe146c73")
                        }
                    }
                },
                new Interview
                {
                    Id = Guid.Parse("d9bee346-ca54-4f7a-a27f-1a4a0ced912f"),
                    CandidateId = Guid.Parse("131915c0-8140-4bd4-8422-c5d03ed1c8e0"),
                    JobId = Guid.Parse("ded8fa0c-cba0-45e5-a73c-5ed5291caf47"),
                    RecruiterOwnerId = Guid.Parse("14448318-7efc-4b97-9b46-0d6ea6ab2056"),
                    Title = "HR Round",
                    Note = "Phỏng vấn về văn hóa công ty và thái độ làm việc",
                    Location = "Hội trường A",
                    MeetingUrl = null,
                    Result = Result.Passed,
                    Status = InterviewStatus.Interviewed,
                    StartAt = new TimeOnly(13, 30),
                    EndAt = new TimeOnly(14, 30),
                    InterviewDate = DateOnly.ParseExact("2025-04-04", IsoDate, CultureInfo.InvariantCulture),
                    Interviewers = new List<IntervewerInterview>
                    {
                        new IntervewerInterview
                        {
                            InterviewId = Guid.Parse("d9bee346-ca54-4f7a-a27f-1a4a0ced912f"),
                            UserId = Guid.Parse("cdee234a-18b4-4db5-8998-fdd133fcbb7f")
                        }
                    }
                },
                new Interview
                {
                    Id = Guid.Parse("129a1850-f106-49c4-bab1-a5cbccf73f55"),
                    CandidateId = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    JobId = Guid.Parse("f57eff31-9fe5-434f-a51b-953ac184b362"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    Title = "Frontend Coding Challenge",
                    Note = "Bài test live coding React.js",
                    Location = null,
                    MeetingUrl = $"{MeetingUrlBase}"+"frontend-test",
                    Result = Result.Failed,
                    Status = InterviewStatus.Cancelled,
                    StartAt = new TimeOnly(16, 00),
                    EndAt = new TimeOnly(17, 00),
                    InterviewDate = DateOnly.ParseExact("2025-04-05", IsoDate, CultureInfo.InvariantCulture),
                    Interviewers = new List<IntervewerInterview>
                    {
                        new IntervewerInterview
                        {
                            InterviewId = Guid.Parse("129a1850-f106-49c4-bab1-a5cbccf73f55"),
                            UserId = Guid.Parse("afa3bef6-4aee-4eb9-b4c7-9b259dc997b4")
                        }
                    }
                },
                new Interview
                {
                    Id = Guid.Parse("41182b41-8fc8-4983-92a6-b5d349dcea90"),
                    CandidateId = Guid.Parse("3d1a7327-276e-4283-a699-db16b5ea8427"),
                    JobId = Guid.Parse("64915173-1282-483d-ae82-d21879049ddd"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    Title = "Data Analysis Test",
                    Note = "Bài test phân tích dữ liệu",
                    Location = "Phòng họp 2",
                    MeetingUrl = null,
                    Result = Result.Passed,
                    Status = InterviewStatus.New,
                    StartAt = new TimeOnly(15, 00),
                    EndAt = new TimeOnly(16, 00),
                    InterviewDate = DateOnly.ParseExact("2025-04-06", IsoDate, CultureInfo.InvariantCulture),
                    Interviewers = new List<IntervewerInterview>
                    {
                        new IntervewerInterview
                        {
                            InterviewId = Guid.Parse("41182b41-8fc8-4983-92a6-b5d349dcea90"),
                            UserId = Guid.Parse("e9c215a3-ebf3-46df-8337-b6945d4ae194")
                        }
                    }
                }
            };

            context.Interviews.AddRange(interviews);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedJobAsync(IMSDbContext context)
    {
        if (!await context.Jobs.AnyAsync())
        {
            context.Jobs.AddRange(
                new Job
                {
                    Id = Guid.Parse("5b8566ab-7a52-4795-b798-abc9bfd6066d"),
                    Title = "Software Engineer",
                    StartDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    MinSalary = 1000.00m,
                    MaxSalary = 3000.00m,
                    WorkingAddress = "123 Tech Street",
                    Description = "Develop and maintain web applications.",
                    Status = JobStatus.Open,
                    JobBenefits = new List<JobBenefit>
                    {
                        new JobBenefit { BenefitId = Guid.Parse("3aaad05e-cc1f-4ebc-88ea-70785ad89c4b") },
                        new JobBenefit { BenefitId = Guid.Parse("9e2a3e52-1330-4b3e-bd68-2b31250f1663") }
                    },
                    JobLevels = new List<JobLevel>
                    {
                        new JobLevel { LevelId = Guid.Parse("f6ba6382-56f4-44f2-89b7-be22d0174108") },
                        new JobLevel { LevelId = Guid.Parse("4dd558f1-6b3d-4cdd-99c9-6b4a28f78004") }
                    },
                    JobSkills = new List<JobSkill>
                    {
                        new JobSkill { SkillId = Guid.Parse("a536ddc6-33d5-4e4b-b9ae-db50d54bef19") },
                        new JobSkill { SkillId = Guid.Parse("6ebec1e6-6e7d-4b4f-9d9d-24331b21d0ec") }
                    }
                },
                new Job
                {
                    Id = Guid.Parse("b3a5568c-9250-44b9-bb35-e0a61bedecf4"),
                    Title = "Project Manager",
                    StartDate = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    MinSalary = 2000.00m,
                    MaxSalary = 5000.00m,
                    WorkingAddress = "456 Business Road",
                    Description = "Manage project teams and oversee development processes.",
                    Status = JobStatus.Open,
                    JobBenefits = new List<JobBenefit>
                    {
                        new JobBenefit { BenefitId = Guid.Parse("c2c28ee0-f6e6-4e99-8a0a-cf116ca11b55") },
                        new JobBenefit { BenefitId = Guid.Parse("f47e456b-2bbe-4dca-a542-ff4a2305756d") }
                    },
                    JobLevels = new List<JobLevel>
                    {
                        new JobLevel { LevelId = Guid.Parse("6290dfca-5c9d-47ef-bd19-298788fdab02") }
                    },
                    JobSkills = new List<JobSkill>
                    {
                        new JobSkill { SkillId = Guid.Parse("2c95cc6b-079f-4089-8a55-9a17596875d0") }
                    }
                },
                new Job
                {
                    Id = Guid.Parse("64915173-1282-483d-ae82-d21879049ddd"),
                    Title = "Data Analyst",
                    StartDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    MinSalary = 1500.00m,
                    MaxSalary = 4000.00m,
                    WorkingAddress = "789 Data Lane",
                    Description = "Analyze business data and provide insights.",
                    Status = JobStatus.Closed,
                    JobBenefits = new List<JobBenefit>
                    {
                        new JobBenefit { BenefitId = Guid.Parse("3a928c11-c0b4-4e62-ac1f-423d79a157fb") }
                    },
                    JobLevels = new List<JobLevel>
                    {
                        new JobLevel { LevelId = Guid.Parse("b416602b-fb2c-495e-a022-0445d8212192") }
                    },
                    JobSkills = new List<JobSkill>
                    {
                        new JobSkill { SkillId = Guid.Parse("9e16929c-e106-48f3-9692-2d22ed5ec7b4") }
                    }
                },
                new Job
                {
                    Id = Guid.Parse("ded8fa0c-cba0-45e5-a73c-5ed5291caf47"),
                    Title = "Team Lead",
                    StartDate = new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    MinSalary = 2500.00m,
                    MaxSalary = 6000.00m,
                    WorkingAddress = "321 Lead Street",
                    Description = "Lead a team of developers and manage projects.",
                    Status = JobStatus.Open,
                    JobLevels = new List<JobLevel>
                    {
                        new JobLevel { LevelId = Guid.Parse("898d0e0f-45da-4ce3-9d38-a96cd67efabc") }
                    }
                },
                new Job
                {
                    Id = Guid.Parse("f57eff31-9fe5-434f-a51b-953ac184b362"),
                    Title = "Backend Developer",
                    StartDate = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    MinSalary = 1500.00m,
                    MaxSalary = 4000.00m,
                    WorkingAddress = "789 Data Lane",
                    Description = "Analyze business data and provide insights.",
                    Status = JobStatus.Closed,
                    JobSkills = new List<JobSkill>
                    {
                        new JobSkill { SkillId = Guid.Parse("d355bb89-690b-4eb7-9772-780123916324") }
                    }
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCandidateAsync(IMSDbContext context)
    {
        if (!await context.Candidates.AnyAsync())
        {
            context.Candidates.AddRange(
                new Candidate
                {
                    Id = Guid.Parse("9228ff0b-db53-4473-9358-3bb41a0eb496"),
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "1234567890",
                    DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Address = "123 Main St",
                    Gender = Gender.MALE,
                    CV_Attachment = "john_doe_cv.pdf",
                    HighestLevel = HighestLevel.BachelorsDegree,
                    PositionId = Guid.Parse("cb88b24f-76a4-41bd-9283-9d69984b9d80"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    CandidateSkills = new List<CandidateSkill>
                    {
                        new CandidateSkill { SkillId = Guid.Parse("a536ddc6-33d5-4e4b-b9ae-db50d54bef19") },
                        new CandidateSkill { SkillId = Guid.Parse("6ebec1e6-6e7d-4b4f-9d9d-24331b21d0ec") }
                    }
                },
                new Candidate
                {
                    Id = Guid.Parse("131915c0-8140-4bd4-8422-c5d03ed1c8e0"),
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Phone = "0987654321",
                    DateOfBirth = new DateTime(1992, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                    Address = "456 Elm St",
                    Gender = Gender.FEMALE,
                    CV_Attachment = "jane_smith_cv.pdf",
                    HighestLevel = HighestLevel.MastersDegree,
                    PositionId = Guid.Parse("d6934e7c-8749-413a-8dc4-0ac922218ade"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    CandidateSkills = new List<CandidateSkill>
                    {
                        new CandidateSkill { SkillId = Guid.Parse("d355bb89-690b-4eb7-9772-780123916324") },
                        new CandidateSkill { SkillId = Guid.Parse("2c95cc6b-079f-4089-8a55-9a17596875d0") }
                    }
                },
                new Candidate
                {
                    Id = Guid.Parse("3d1a7327-276e-4283-a699-db16b5ea8427"),
                    Name = "Alice Brown",
                    Email = "alice.brown@example.com",
                    Phone = "1112223333",
                    DateOfBirth = new DateTime(1995, 5, 5, 0, 0, 0, DateTimeKind.Utc),
                    Address = "789 Maple St",
                    Gender = Gender.FEMALE,
                    CV_Attachment = "alice_brown_cv.pdf",
                    HighestLevel = HighestLevel.PhD,
                    PositionId = Guid.Parse("5f8702e0-e366-4979-b7be-d5a9f8bd15a6"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    CandidateSkills = new List<CandidateSkill>
                    {
                        new CandidateSkill { SkillId = Guid.Parse("9e16929c-e106-48f3-9692-2d22ed5ec7b4") }
                    }
                },
                new Candidate
                {
                    Id = Guid.Parse("f8288c08-6425-477e-a80c-42734033440f"),
                    Name = "Bob Johnson",
                    Email = "bob.johnson@example.com",
                    Phone = "4445556666",
                    DateOfBirth = new DateTime(1988, 8, 8, 0, 0, 0, DateTimeKind.Utc),
                    Address = "321 Oak St",
                    Gender = Gender.MALE,
                    CV_Attachment = "bob_johnson_cv.pdf",
                    HighestLevel = HighestLevel.HighSchool,
                    PositionId = Guid.Parse("99fdfdd1-c636-40be-a108-42137a86a997"),
                    RecruiterOwnerId = Guid.Parse("cff5628e-bed3-4f0f-8f1a-aef99c1001bc"),
                    CandidateSkills = new List<CandidateSkill>
                    {
                        new CandidateSkill { SkillId = Guid.Parse("e77f186e-94ab-4345-9192-7745e939cbc7") }
                    }
                }
            );
            await context.SaveChangesAsync();
        }
    }





    private static async Task SeedBenefitAsync(IMSDbContext context)
    {
        if (!await context.Benefits.AnyAsync())
        {
            context.Benefits.AddRange(
                new Benefit { Id = Guid.Parse("3aaad05e-cc1f-4ebc-88ea-70785ad89c4b"), Name = "Lunch" },
                new Benefit { Id = Guid.Parse("c2c28ee0-f6e6-4e99-8a0a-cf116ca11b55"), Name = "25-day leave" },
                new Benefit { Id = Guid.Parse("9e2a3e52-1330-4b3e-bd68-2b31250f1663"), Name = "Healthcare insurance" },
                new Benefit { Id = Guid.Parse("f47e456b-2bbe-4dca-a542-ff4a2305756d"), Name = "Hybrid working" },
                new Benefit { Id = Guid.Parse("3a928c11-c0b4-4e62-ac1f-423d79a157fb"), Name = "Travel" }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSkillAsync(IMSDbContext context)
    {
        if (!await context.Skills.AnyAsync())
        {
            context.Skills.AddRange(
                new Skill { Id = Guid.Parse("a536ddc6-33d5-4e4b-b9ae-db50d54bef19"), Name = "Java" },
                new Skill { Id = Guid.Parse("d355bb89-690b-4eb7-9772-780123916324"), Name = "Node.js" },
                new Skill { Id = Guid.Parse("6ebec1e6-6e7d-4b4f-9d9d-24331b21d0ec"), Name = ".NET" },
                new Skill { Id = Guid.Parse("e77f186e-94ab-4345-9192-7745e939cbc7"), Name = "C++" },
                new Skill { Id = Guid.Parse("2c95cc6b-079f-4089-8a55-9a17596875d0"), Name = "Business analysis" },
                new Skill { Id = Guid.Parse("9e16929c-e106-48f3-9692-2d22ed5ec7b4"), Name = "Communication" }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedPositionAsync(IMSDbContext context)
    {
        if (!await context.Positions.AnyAsync())
        {
            context.Positions.AddRange(
                new Position { Id = Guid.Parse("cb88b24f-76a4-41bd-9283-9d69984b9d80"), Name = "Backend Developer" },
                new Position { Id = Guid.Parse("d6934e7c-8749-413a-8dc4-0ac922218ade"), Name = "Business Analyst" },
                new Position { Id = Guid.Parse("5f8702e0-e366-4979-b7be-d5a9f8bd15a6"), Name = "Tester" },
                new Position { Id = Guid.Parse("32a8c485-01c4-4e42-aea9-8417f09d56e6"), Name = "HR" },
                new Position { Id = Guid.Parse("99fdfdd1-c636-40be-a108-42137a86a997"), Name = "Project Manager" },
                new Position { Id = Guid.Parse("b2910f48-28b9-4e0d-8376-2bb09ab7fbb2"), Name = "Not available" }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedDepartmentAsync(IMSDbContext context)
    {
        if (!await context.Departments.AnyAsync())
        {
            context.Departments.AddRange(
                new Department { Id = Guid.Parse("2169f928-3fe6-49d1-ada3-7ed50efe857e"), Name = "IT" },
                new Department { Id = Guid.Parse("16dfd695-091a-4230-9d6f-490d055d4145"), Name = "HR" },
                new Department { Id = Guid.Parse("7f1452da-5942-4c78-82a3-3bb606e68b94"), Name = "Finance" },
                new Department { Id = Guid.Parse("1c29761e-fb6c-40f0-b569-15beed28eff6"), Name = "Communication" },
                new Department { Id = Guid.Parse("24ad3454-b87b-4234-bed8-eeee892eae9e"), Name = "Marketing" },
                new Department { Id = Guid.Parse("c2c96b1a-9513-4595-912c-a521160a008a"), Name = "Accounting" }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedLevelAsync(IMSDbContext context)
    {
        if (!await context.Levels.AnyAsync())
        {
            context.Levels.AddRange(
                new Level { Id = Guid.Parse("f6ba6382-56f4-44f2-89b7-be22d0174108"), Name = "Fresher" },
                new Level { Id = Guid.Parse("4dd558f1-6b3d-4cdd-99c9-6b4a28f78004"), Name = "Junior" },
                new Level { Id = Guid.Parse("b416602b-fb2c-495e-a022-0445d8212192"), Name = "Senior" },
                new Level { Id = Guid.Parse("898d0e0f-45da-4ce3-9d38-a96cd67efabc"), Name = "Leader" },
                new Level { Id = Guid.Parse("6290dfca-5c9d-47ef-bd19-298788fdab02"), Name = "Manager" },
                new Level { Id = Guid.Parse("e2f9312a-9bf1-4454-bd82-ae89c5bde329"), Name = "Vice Head" }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedContactTypeAsync(IMSDbContext context)
    {
        if (!await context.ContactTypes.AnyAsync())
        {
            context.ContactTypes.AddRange(
                new ContactType { Id = Guid.Parse("1577075e-0531-48a4-b425-3436a7dca48b"), Name = "Trial 2 months" },
                new ContactType { Id = Guid.Parse("2804d947-2bda-491d-93af-957dad2ce355"), Name = "Trainee 3 months" },
                new ContactType { Id = Guid.Parse("a6713097-3064-4613-870d-155a85a65956"), Name = "1 year" },
                new ContactType { Id = Guid.Parse("c7d3c0dd-1f26-488c-83b4-0c3be6a4ca75"), Name = "3 years" },
                new ContactType { Id = Guid.Parse("f6ba6382-56f4-44f2-89b7-be22d0174108"), Name = "Unlimited" }
            );
            await context.SaveChangesAsync();
        }
    }
}




internal class UserViewModel
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; }
    public string? SecurityStamp { get; set; }
    public required string Role { get; set; }
}