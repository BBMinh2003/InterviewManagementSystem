using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_CreatedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_DeletedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_CreatedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_DeletedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UpdatedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Department",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                schema: "Security",
                table: "RefreshTokens");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "Security",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BaseEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseEntity_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntity_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntity_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CV_Attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    YearOfExperience = table.Column<int>(type: "int", nullable: false),
                    HighestLevel = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruiterOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "Common",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_Users_RecruiterOwnerId",
                        column: x => x.RecruiterOwnerId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CandidateId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interviews_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Interviews_Users_RecruiterOwnerId",
                        column: x => x.RecruiterOwnerId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Offers_Users_RecruiterOwnerId",
                        column: x => x.RecruiterOwnerId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                schema: "Security",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_CreatedById",
                table: "BaseEntity",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_DeletedById",
                table: "BaseEntity",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_UpdatedById",
                table: "BaseEntity",
                column: "UpdatedById");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_BaseEntity_Id",
                schema: "Security",
                table: "RefreshTokens",
                column: "Id",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_RefreshTokens_BaseEntity_Id",
                schema: "Security",
                table: "RefreshTokens");

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

            migrationBuilder.DropTable(
                name: "BaseEntity");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentId",
                schema: "Security",
                table: "Users");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Security",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                schema: "Security",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "Security",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                schema: "Security",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Security",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Security",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                schema: "Security",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_CreatedById",
                schema: "Security",
                table: "RefreshTokens",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_DeletedById",
                schema: "Security",
                table: "RefreshTokens",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UpdatedById",
                schema: "Security",
                table: "RefreshTokens",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_CreatedById",
                schema: "Security",
                table: "RefreshTokens",
                column: "CreatedById",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_DeletedById",
                schema: "Security",
                table: "RefreshTokens",
                column: "DeletedById",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedById",
                schema: "Security",
                table: "RefreshTokens",
                column: "UpdatedById",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
