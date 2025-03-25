using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModelAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                schema: "Security",
                table: "Users");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "Security",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Benefit",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkingAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobBenefits",
                schema: "Common",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BenefitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobBenefits", x => new { x.JobId, x.BenefitId });
                    table.ForeignKey(
                        name: "FK_JobBenefits_Benefit_BenefitId",
                        column: x => x.BenefitId,
                        principalSchema: "Common",
                        principalTable: "Benefit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobBenefits_Jobs_JobId",
                        column: x => x.JobId,
                        principalSchema: "Common",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobLevels",
                schema: "Common",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLevels", x => new { x.JobId, x.LevelId });
                    table.ForeignKey(
                        name: "FK_JobLevels_Jobs_JobId",
                        column: x => x.JobId,
                        principalSchema: "Common",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobLevels_Levels_LevelId",
                        column: x => x.LevelId,
                        principalSchema: "Common",
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(555)", maxLength: 555, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CV_Attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    YearOfExperience = table.Column<int>(type: "int", nullable: false),
                    HighestLevel = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruiterOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "Common",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidates_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidates_Users_RecruiterOwnerId",
                        column: x => x.RecruiterOwnerId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobSkills",
                schema: "Common",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => new { x.JobId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_JobSkills_Jobs_JobId",
                        column: x => x.JobId,
                        principalSchema: "Common",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "Common",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateSkills",
                schema: "Common",
                columns: table => new
                {
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateSkills", x => new { x.CandidateId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_CandidateSkills_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalSchema: "Common",
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "Common",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruiterOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MeetingUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    CandidateId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interviews_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalSchema: "Common",
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interviews_Candidates_CandidateId1",
                        column: x => x.CandidateId1,
                        principalSchema: "Common",
                        principalTable: "Candidates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Interviews_Jobs_JobId",
                        column: x => x.JobId,
                        principalSchema: "Common",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interviews_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Interviews_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Interviews_Users_RecruiterOwnerId",
                        column: x => x.RecruiterOwnerId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interviews_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IntervewerInterviews",
                schema: "Common",
                columns: table => new
                {
                    InterviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervewerInterviews", x => new { x.InterviewId, x.UserId });
                    table.ForeignKey(
                        name: "FK_IntervewerInterviews_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "Common",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntervewerInterviews_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruiterOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactPeriodFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactPeriodTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalSchema: "Common",
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalSchema: "Common",
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Common",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "Common",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_Levels_LevelId",
                        column: x => x.LevelId,
                        principalSchema: "Common",
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "Common",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_Users_RecruiterOwnerId",
                        column: x => x.RecruiterOwnerId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Benefit",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Lunch" },
                    { new Guid("55555555-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "25-day leave" },
                    { new Guid("66666666-cccc-cccc-cccc-cccccccccccc"), "Healthcare insurance" },
                    { new Guid("77777777-dddd-dddd-dddd-dddddddddddd"), "Hybrid working" },
                    { new Guid("88888888-eeee-eeee-eeee-eeeeeeeeeeee"), "Travel" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "ContactTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Trial 2 months" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Trainee 3 months" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "1 year" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "3 years" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Unlimited" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "IT" },
                    { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "HR" },
                    { new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), "Finance" },
                    { new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), "Communication" },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), "Marketing" },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), "Accounting" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Jobs",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "Description", "EndDate", "IsDeleted", "MaxSalary", "MinSalary", "StartDate", "Status", "Title", "UpdatedAt", "UpdatedById", "WorkingAddress" },
                values: new object[,]
                {
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Analyze business data and provide insights.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4000.00m, 1500.00m, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Backend Developer", null, null, "789 Data Lane" },
                    { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Lead a team of developers and manage projects.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6000.00m, 2500.00m, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Team Lead", null, null, "321 Lead Street" },
                    { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Analyze business data and provide insights.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4000.00m, 1500.00m, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Data Analyst", null, null, "789 Data Lane" },
                    { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Manage project teams and oversee development processes.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 5000.00m, 2000.00m, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Project Manager", null, null, "456 Business Road" },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Develop and maintain web applications.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3000.00m, 1000.00m, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Software Engineer", null, null, "123 Tech Street" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Levels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Fresher" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Junior" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Senior" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Leader" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Manager" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Vice Head" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Positions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Backend Developer" },
                    { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Business Analyst" },
                    { new Guid("99999999-cccc-cccc-cccc-cccccccccccc"), "Tester" },
                    { new Guid("aaaaaaaa-dddd-dddd-dddd-dddddddddddd"), "HR" },
                    { new Guid("bbbbbbbb-eeee-eeee-eeee-eeeeeeeeeeee"), "Project Manager" },
                    { new Guid("cccccccc-ffff-ffff-ffff-ffffffffffff"), "Not available" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "Description", "IsDeleted", "Name", "NormalizedName", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "Admin", "ADMIN", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "Recruiter", "RECRUITER", null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "Interviewer", "INTERVIEWER", null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "Manager", "Manager", null, null }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Skills",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-dddd-dddd-dddd-dddddddddddd"), "C++" },
                    { new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"), "Business analysis" },
                    { new Guid("33333333-ffff-ffff-ffff-ffffffffffff"), "Communication" },
                    { new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Java" },
                    { new Guid("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Node.js" },
                    { new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"), ".NET" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "CreatedById", "DateOfBirth", "DeletedAt", "DeletedById", "DepartmentId", "Email", "EmailConfirmed", "FullName", "Gender", "IsActive", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedById", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 0, null, "56efc444-6d1c-474e-8341-493482c76457", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "admin@example.com", true, "Admin", 2, true, false, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAEAACcQAAAAEKzO7BHEb758wHgIVI3x0NuSEd8BLlECa+TDvKAF1cUtkj6O5hM9PMp42jCeWnGeww==", null, false, "SECURITY_STAMP_1", false, null, null, "admin" },
                    { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, "d6c3eb8a-97b9-4922-a15e-8f1c68920247", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "recruiter@example.com", true, "Recruiter", 2, true, false, false, null, "RECRUITER@EXAMPLE.COM", "RECRUITER", "AQAAAAEAACcQAAAAEOTNmM1M0OJV+VJyKbHIj8b7oJSH/W5uTr8LQy8HO8bhEIb9ZDf9m1KwnBavT5m9Yg==", null, false, "SECURITY_STAMP_2", false, null, null, "recruiter" },
                    { new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), 0, null, "821b51cb-8cc5-4e7d-8cff-4fcea57502c3", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "interviewer@example.com", true, "Interviewer", 2, true, false, false, null, "INTERVIEWER@EXAMPLE.COM", "INTERVIEWER", "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==", null, false, "SECURITY_STAMP_3", false, null, null, "interviewer" },
                    { new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 0, null, "c49fad22-c12c-43ed-9776-31be1d1284c4", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "manager@example.com", true, "Manager", 2, true, false, false, null, "MANAGER@EXAMPLE.COM", "MANAGER", "AQAAAAEAACcQAAAAELbr2xCUqc37Qu/fRYpRYOQTzUtPnCVXx7muwkJEhUlRlhGuAGD2kJzcIokmv4YrZQ==", null, false, "SECURITY_STAMP_4", false, null, null, "manager" },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), 0, null, "83dfa575-6029-4c25-81f0-0217e9869225", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "interview2@example.com", true, "Interviewer 2", 2, true, false, false, null, "INTERVIEW2@EXAMPLE.COM", "INTERVIEW2", "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==", null, false, "SECURITY_STAMP_5", false, null, null, "interview2" },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), 0, null, "b1dc5c60-f2ba-4ab3-b1ef-1a5cab4d041d", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "interviewer3@example.com", true, "Interviewer 3", 2, true, false, false, null, "INTERVIEWER3@EXAMPLE.COM", "INTERVIEWER3", "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==", null, false, "SECURITY_STAMP_6", false, null, null, "interviewer3" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Candidates",
                columns: new[] { "Id", "Address", "CV_Attachment", "CreatedAt", "CreatedById", "DateOfBirth", "DeletedAt", "DeletedById", "Email", "Gender", "HighestLevel", "IsDeleted", "Name", "Note", "Phone", "PositionId", "RecruiterOwnerId", "Status", "UpdatedAt", "UpdatedById", "YearOfExperience" },
                values: new object[,]
                {
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "123 Main St", "john_doe_cv.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "john.doe@example.com", 0, 1, false, "John Doe", null, "1234567890", new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, null, 0 },
                    { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "456 Elm St", "jane_smith_cv.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1992, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "jane.smith@example.com", 1, 2, false, "Jane Smith", null, "0987654321", new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, null, 0 },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), "789 Maple St", "alice_brown_cv.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1995, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "alice.brown@example.com", 1, 3, false, "Alice Brown", null, "1112223333", new Guid("99999999-cccc-cccc-cccc-cccccccccccc"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, null, 0 },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), "321 Oak St", "bob_johnson_cv.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1988, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "bob.johnson@example.com", 0, 0, false, "Bob Johnson", null, "4445556666", new Guid("bbbbbbbb-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, null, 0 }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "JobBenefits",
                columns: new[] { "BenefitId", "JobId" },
                values: new object[,]
                {
                    { new Guid("88888888-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("77777777-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("55555555-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("77777777-dddd-dddd-dddd-dddddddddddd"), new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("66666666-cccc-cccc-cccc-cccccccccccc"), new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa") }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "JobLevels",
                columns: new[] { "JobId", "LevelId" },
                values: new object[,]
                {
                    { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "JobSkills",
                columns: new[] { "JobId", "SkillId" },
                values: new object[,]
                {
                    { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), new Guid("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), new Guid("33333333-ffff-ffff-ffff-ffffffffffff") },
                    { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc") }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "CandidateSkills",
                columns: new[] { "CandidateId", "SkillId" },
                values: new object[,]
                {
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("33333333-ffff-ffff-ffff-ffffffffffff") },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), new Guid("11111111-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Interviews",
                columns: new[] { "Id", "CandidateId", "CandidateId1", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "EndAt", "IsDeleted", "JobId", "Location", "MeetingUrl", "Note", "RecruiterOwnerId", "Result", "StartAt", "Status", "Title", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new TimeOnly(10, 30, 0), false, new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Văn phòng công ty", null, "Kiểm tra kỹ năng lập trình cơ bản", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(9, 30, 0), 3, "Technical Interview Round 1", null, null },
                    { new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new TimeOnly(15, 0, 0), false, new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, "https://meet.example.com/jane-smith", "Kiểm tra kỹ năng quản lý nhóm", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(14, 0, 0), 1, "Management Interview", null, null },
                    { new Guid("cccc3333-3333-3333-3333-cccccccccccc"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new TimeOnly(11, 0, 0), false, new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), "Văn phòng Hà Nội", null, "Phỏng vấn sâu về kiến thức backend", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(10, 0, 0), 0, "Backend Technical Round", null, null },
                    { new Guid("dddd4444-4444-4444-4444-dddddddddddd"), new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new TimeOnly(14, 30, 0), false, new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), "Hội trường A", null, "Phỏng vấn về văn hóa công ty và thái độ làm việc", new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Passed", new TimeOnly(13, 30, 0), 2, "HR Round", null, null },
                    { new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new TimeOnly(17, 0, 0), false, new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"), null, "https://meet.example.com/frontend-test", "Bài test live coding React.js", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(16, 0, 0), 3, "Frontend Coding Challenge", null, null },
                    { new Guid("ffff6666-6666-6666-6666-ffffffffffff"), new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new TimeOnly(16, 0, 0), false, new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), "Phòng họp 2", null, "Bài test phân tích dữ liệu", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(15, 0, 0), 0, "Data Analysis Test", null, null }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "IntervewerInterviews",
                columns: new[] { "InterviewId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("cccc3333-3333-3333-3333-cccccccccccc"), new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("dddd4444-4444-4444-4444-dddddddddddd"), new Guid("66666666-ffff-ffff-ffff-ffffffffffff") },
                    { new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("ffff6666-6666-6666-6666-ffffffffffff"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Offers",
                columns: new[] { "Id", "ApprovedById", "BasicSalary", "CandidateId", "ContactPeriodFrom", "ContactPeriodTo", "ContactTypeId", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "DepartmentId", "DueDate", "InterviewId", "IsDeleted", "LevelId", "Note", "PositionId", "RecruiterOwnerId", "Status", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 1500.00m, new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9042), new DateTime(2026, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9265), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 4, 1, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(8721), new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), false, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Offer for Backend Developer position", new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 5, null, null },
                    { new Guid("22222222-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 2000.00m, new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9831), new DateTime(2027, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9832), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 4, 4, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9825), new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), false, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Offer for Business Analyst position", new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 4, null, null },
                    { new Guid("33333333-dddd-dddd-dddd-dddddddddddd"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), 1200.00m, new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2025, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9843), new DateTime(2026, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9843), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), new DateTime(2025, 3, 30, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9842), new Guid("cccc3333-3333-3333-3333-cccccccccccc"), false, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Offer for Tester position", new Guid("99999999-cccc-cccc-cccc-cccccccccccc"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 2, null, null },
                    { new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 1600.00m, new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9852), new DateTime(2026, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9853), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 4, 8, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9852), new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), false, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Second offer for Backend Developer position", new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 4, null, null },
                    { new Guid("55555555-ffff-ffff-ffff-ffffffffffff"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 2100.00m, new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9860), new DateTime(2027, 3, 25, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9861), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 4, 4, 2, 3, 35, 749, DateTimeKind.Utc).AddTicks(9860), new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), false, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Second offer for Business Analyst position", new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 4, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                schema: "Security",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CreatedById",
                schema: "Common",
                table: "Candidates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_DeletedById",
                schema: "Common",
                table: "Candidates",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PositionId",
                schema: "Common",
                table: "Candidates",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_RecruiterOwnerId",
                schema: "Common",
                table: "Candidates",
                column: "RecruiterOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UpdatedById",
                schema: "Common",
                table: "Candidates",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkills_SkillId",
                schema: "Common",
                table: "CandidateSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_IntervewerInterviews_UserId",
                schema: "Common",
                table: "IntervewerInterviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_CandidateId",
                schema: "Common",
                table: "Interviews",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_CandidateId1",
                schema: "Common",
                table: "Interviews",
                column: "CandidateId1");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_CreatedById",
                schema: "Common",
                table: "Interviews",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_DeletedById",
                schema: "Common",
                table: "Interviews",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_JobId",
                schema: "Common",
                table: "Interviews",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_RecruiterOwnerId",
                schema: "Common",
                table: "Interviews",
                column: "RecruiterOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_UpdatedById",
                schema: "Common",
                table: "Interviews",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobBenefits_BenefitId",
                schema: "Common",
                table: "JobBenefits",
                column: "BenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_LevelId",
                schema: "Common",
                table: "JobLevels",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CreatedById",
                schema: "Common",
                table: "Jobs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DeletedById",
                schema: "Common",
                table: "Jobs",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UpdatedById",
                schema: "Common",
                table: "Jobs",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_SkillId",
                schema: "Common",
                table: "JobSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ApprovedById",
                schema: "Common",
                table: "Offers",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CandidateId",
                schema: "Common",
                table: "Offers",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ContactTypeId",
                schema: "Common",
                table: "Offers",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CreatedById",
                schema: "Common",
                table: "Offers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DeletedById",
                schema: "Common",
                table: "Offers",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DepartmentId",
                schema: "Common",
                table: "Offers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_InterviewId",
                schema: "Common",
                table: "Offers",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_LevelId",
                schema: "Common",
                table: "Offers",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_PositionId",
                schema: "Common",
                table: "Offers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_RecruiterOwnerId",
                schema: "Common",
                table: "Offers",
                column: "RecruiterOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UpdatedById",
                schema: "Common",
                table: "Offers",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                schema: "Security",
                table: "Users",
                column: "DepartmentId",
                principalSchema: "Common",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CandidateSkills",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "IntervewerInterviews",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "JobBenefits",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "JobLevels",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "JobSkills",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Offers",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Benefit",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "ContactTypes",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Interviews",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Levels",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Candidates",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "Common");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "Security",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                schema: "Security",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
