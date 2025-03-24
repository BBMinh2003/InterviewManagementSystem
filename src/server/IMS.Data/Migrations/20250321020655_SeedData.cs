using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BaseEntity",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "IsDeleted", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("22222222-cccc-cccc-cccc-cccccccccccc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("33333333-dddd-dddd-dddd-dddddddddddd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("55555555-ffff-ffff-ffff-ffffffffffff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("cccc3333-3333-3333-3333-cccccccccccc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("dddd4444-4444-4444-4444-dddddddddddd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null },
                    { new Guid("ffff6666-6666-6666-6666-ffffffffffff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null }
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
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 0, null, "56b3a73a-b913-4d99-b02a-1623718ec83e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "admin@example.com", true, "Admin", 2, true, false, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAEAACcQAAAAEKzO7BHEb758wHgIVI3x0NuSEd8BLlECa+TDvKAF1cUtkj6O5hM9PMp42jCeWnGeww==", null, false, "SECURITY_STAMP_1", false, null, null, "admin" },
                    { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, null, "d800c29b-59c8-42dd-bc13-a1113e30f128", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "recruiter@example.com", true, "Recruiter", 2, true, false, false, null, "RECRUITER@EXAMPLE.COM", "RECRUITER", "AQAAAAEAACcQAAAAEOTNmM1M0OJV+VJyKbHIj8b7oJSH/W5uTr8LQy8HO8bhEIb9ZDf9m1KwnBavT5m9Yg==", null, false, "SECURITY_STAMP_2", false, null, null, "recruiter" },
                    { new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), 0, null, "0cc4e817-3f97-452d-844c-c6d965309276", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "interviewer@example.com", true, "Interviewer", 2, true, false, false, null, "INTERVIEWER@EXAMPLE.COM", "INTERVIEWER", "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==", null, false, "SECURITY_STAMP_3", false, null, null, "interviewer" },
                    { new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 0, null, "68a18a36-f5cb-46c8-aad7-3303898cb8eb", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "manager@example.com", true, "Manager", 2, true, false, false, null, "MANAGER@EXAMPLE.COM", "MANAGER", "AQAAAAEAACcQAAAAELbr2xCUqc37Qu/fRYpRYOQTzUtPnCVXx7muwkJEhUlRlhGuAGD2kJzcIokmv4YrZQ==", null, false, "SECURITY_STAMP_4", false, null, null, "manager" },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), 0, null, "302a1e9a-7037-461c-ae72-941974d3fa3d", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "interview2@example.com", true, "Interviewer 2", 2, true, false, false, null, "INTERVIEW2@EXAMPLE.COM", "INTERVIEW2", "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==", null, false, "SECURITY_STAMP_5", false, null, null, "interview2" },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), 0, null, "dee8d046-6858-423c-a212-a896013963a0", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "interviewer3@example.com", true, "Interviewer 3", 2, true, false, false, null, "INTERVIEWER3@EXAMPLE.COM", "INTERVIEWER3", "AQAAAAEAACcQAAAAEM41t5RUbsvo9ImUsQhuLuI0RLJRt5t7HAVUPnU9Z3naZud31HsypTKOyjmD1tv/UQ==", null, false, "SECURITY_STAMP_6", false, null, null, "interviewer3" }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Candidates",
                columns: new[] { "Id", "Address", "CV_Attachment", "DateOfBirth", "Email", "Gender", "HighestLevel", "Name", "Note", "Phone", "PositionId", "RecruiterOwnerId", "Status", "YearOfExperience" },
                values: new object[,]
                {
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "123 Main St", "john_doe_cv.pdf", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", 0, 1, "John Doe", null, "1234567890", new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, 0 },
                    { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "456 Elm St", "jane_smith_cv.pdf", new DateTime(1992, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", 1, 2, "Jane Smith", null, "0987654321", new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, 0 },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), "789 Maple St", "alice_brown_cv.pdf", new DateTime(1995, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice.brown@example.com", 1, 3, "Alice Brown", null, "1112223333", new Guid("99999999-cccc-cccc-cccc-cccccccccccc"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, 0 },
                    { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), "321 Oak St", "bob_johnson_cv.pdf", new DateTime(1988, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob.johnson@example.com", 0, 0, "Bob Johnson", null, "4445556666", new Guid("bbbbbbbb-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 0, 0 }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Jobs",
                columns: new[] { "Id", "Description", "EndDate", "MaxSalary", "MinSalary", "StartDate", "Status", "Title", "WorkingAddress" },
                values: new object[,]
                {
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"), "Analyze business data and provide insights.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000.00m, 1500.00m, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Backend Developer", "789 Data Lane" },
                    { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), "Lead a team of developers and manage projects.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6000.00m, 2500.00m, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Team Lead", "321 Lead Street" },
                    { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), "Analyze business data and provide insights.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000.00m, 1500.00m, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Data Analyst", "789 Data Lane" },
                    { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Manage project teams and oversee development processes.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000.00m, 2000.00m, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Project Manager", "456 Business Road" },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Develop and maintain web applications.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000.00m, 1000.00m, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Software Engineer", "123 Tech Street" }
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
                columns: new[] { "Id", "CandidateId", "CandidateId1", "EndAt", "JobId", "Location", "MeetingUrl", "Note", "RecruiterOwnerId", "Result", "StartAt", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, new TimeOnly(10, 30, 0), new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Văn phòng công ty", null, "Kiểm tra kỹ năng lập trình cơ bản", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(9, 30, 0), 3, "Technical Interview Round 1" },
                    { new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(15, 0, 0), new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, "https://meet.example.com/jane-smith", "Kiểm tra kỹ năng quản lý nhóm", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(14, 0, 0), 1, "Management Interview" },
                    { new Guid("cccc3333-3333-3333-3333-cccccccccccc"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, new TimeOnly(11, 0, 0), new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), "Văn phòng Hà Nội", null, "Phỏng vấn sâu về kiến thức backend", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(10, 0, 0), 0, "Backend Technical Round" },
                    { new Guid("dddd4444-4444-4444-4444-dddddddddddd"), new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(14, 30, 0), new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), "Hội trường A", null, "Phỏng vấn về văn hóa công ty và thái độ làm việc", new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Passed", new TimeOnly(13, 30, 0), 2, "HR Round" },
                    { new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, new TimeOnly(17, 0, 0), new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"), null, "https://meet.example.com/frontend-test", "Bài test live coding React.js", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(16, 0, 0), 3, "Frontend Coding Challenge" },
                    { new Guid("ffff6666-6666-6666-6666-ffffffffffff"), new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), null, new TimeOnly(16, 0, 0), new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), "Phòng họp 2", null, "Bài test phân tích dữ liệu", new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new TimeOnly(15, 0, 0), 0, "Data Analysis Test" }
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
                columns: new[] { "Id", "ApprovedById", "BasicSalary", "CandidateId", "ContactPeriodFrom", "ContactPeriodTo", "ContactTypeId", "DepartmentId", "DueDate", "InterviewId", "LevelId", "Note", "PositionId", "RecruiterOwnerId", "Status" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 1500.00m, new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8012), new DateTime(2026, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8132), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 28, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(7831), new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Offer for Backend Developer position", new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 5 },
                    { new Guid("22222222-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 2000.00m, new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8468), new DateTime(2027, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8468), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 3, 31, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8463), new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Offer for Business Analyst position", new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 4 },
                    { new Guid("33333333-dddd-dddd-dddd-dddddddddddd"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), 1200.00m, new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2025, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8489), new DateTime(2026, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8490), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), new DateTime(2025, 3, 26, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8488), new Guid("cccc3333-3333-3333-3333-cccccccccccc"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Offer for Tester position", new Guid("99999999-cccc-cccc-cccc-cccccccccccc"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 2 },
                    { new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 1600.00m, new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8497), new DateTime(2026, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8497), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 4, 4, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8496), new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Second offer for Backend Developer position", new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 4 },
                    { new Guid("55555555-ffff-ffff-ffff-ffffffffffff"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), 2100.00m, new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8504), new DateTime(2027, 3, 21, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8504), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 3, 31, 2, 6, 54, 873, DateTimeKind.Utc).AddTicks(8503), new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Second offer for Business Analyst position", new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Common",
                table: "CandidateSkills",
                keyColumns: new[] { "CandidateId", "SkillId" },
                keyValues: new object[] { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "CandidateSkills",
                keyColumns: new[] { "CandidateId", "SkillId" },
                keyValues: new object[] { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "CandidateSkills",
                keyColumns: new[] { "CandidateId", "SkillId" },
                keyValues: new object[] { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "CandidateSkills",
                keyColumns: new[] { "CandidateId", "SkillId" },
                keyValues: new object[] { new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "CandidateSkills",
                keyColumns: new[] { "CandidateId", "SkillId" },
                keyValues: new object[] { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("33333333-ffff-ffff-ffff-ffffffffffff") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "CandidateSkills",
                keyColumns: new[] { "CandidateId", "SkillId" },
                keyValues: new object[] { new Guid("66666666-ffff-ffff-ffff-ffffffffffff"), new Guid("11111111-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "IntervewerInterviews",
                keyColumns: new[] { "InterviewId", "UserId" },
                keyValues: new object[] { new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "IntervewerInterviews",
                keyColumns: new[] { "InterviewId", "UserId" },
                keyValues: new object[] { new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "IntervewerInterviews",
                keyColumns: new[] { "InterviewId", "UserId" },
                keyValues: new object[] { new Guid("cccc3333-3333-3333-3333-cccccccccccc"), new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "IntervewerInterviews",
                keyColumns: new[] { "InterviewId", "UserId" },
                keyValues: new object[] { new Guid("dddd4444-4444-4444-4444-dddddddddddd"), new Guid("66666666-ffff-ffff-ffff-ffffffffffff") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "IntervewerInterviews",
                keyColumns: new[] { "InterviewId", "UserId" },
                keyValues: new object[] { new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"), new Guid("33333333-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "IntervewerInterviews",
                keyColumns: new[] { "InterviewId", "UserId" },
                keyValues: new object[] { new Guid("ffff6666-6666-6666-6666-ffffffffffff"), new Guid("44444444-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobBenefits",
                keyColumns: new[] { "BenefitId", "JobId" },
                keyValues: new object[] { new Guid("88888888-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("77777777-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobBenefits",
                keyColumns: new[] { "BenefitId", "JobId" },
                keyValues: new object[] { new Guid("55555555-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobBenefits",
                keyColumns: new[] { "BenefitId", "JobId" },
                keyValues: new object[] { new Guid("77777777-dddd-dddd-dddd-dddddddddddd"), new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobBenefits",
                keyColumns: new[] { "BenefitId", "JobId" },
                keyValues: new object[] { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobBenefits",
                keyColumns: new[] { "BenefitId", "JobId" },
                keyValues: new object[] { new Guid("66666666-cccc-cccc-cccc-cccccccccccc"), new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobLevels",
                keyColumns: new[] { "JobId", "LevelId" },
                keyValues: new object[] { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobLevels",
                keyColumns: new[] { "JobId", "LevelId" },
                keyValues: new object[] { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobLevels",
                keyColumns: new[] { "JobId", "LevelId" },
                keyValues: new object[] { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobLevels",
                keyColumns: new[] { "JobId", "LevelId" },
                keyValues: new object[] { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobLevels",
                keyColumns: new[] { "JobId", "LevelId" },
                keyValues: new object[] { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobSkills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("66666666-dddd-dddd-dddd-dddddddddddd"), new Guid("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobSkills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("77777777-cccc-cccc-cccc-cccccccccccc"), new Guid("33333333-ffff-ffff-ffff-ffffffffffff") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobSkills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobSkills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "JobSkills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Levels",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Positions",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Positions",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-ffff-ffff-ffff-ffffffffffff"));

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
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("22222222-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Benefit",
                keyColumn: "Id",
                keyValue: new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Benefit",
                keyColumn: "Id",
                keyValue: new Guid("55555555-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Benefit",
                keyColumn: "Id",
                keyValue: new Guid("66666666-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Benefit",
                keyColumn: "Id",
                keyValue: new Guid("77777777-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Benefit",
                keyColumn: "Id",
                keyValue: new Guid("88888888-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Candidates",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Interviews",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Interviews",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Interviews",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-3333-3333-3333-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Interviews",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-4444-4444-4444-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Interviews",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Interviews",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-6666-6666-6666-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Levels",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Levels",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Levels",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Levels",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Levels",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

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
                schema: "Common",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("33333333-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"));

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
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-1111-1111-1111-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-2222-2222-2222-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-3333-3333-3333-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-4444-4444-4444-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-5555-5555-5555-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-6666-6666-6666-ffffffffffff"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Candidates",
                keyColumn: "Id",
                keyValue: new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Candidates",
                keyColumn: "Id",
                keyValue: new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Candidates",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("66666666-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("77777777-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Positions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("44444444-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeed"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("66666666-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("77777777-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "BaseEntity",
                keyColumn: "Id",
                keyValue: new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Positions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Positions",
                keyColumn: "Id",
                keyValue: new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Common",
                table: "Positions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        }
    }
}
