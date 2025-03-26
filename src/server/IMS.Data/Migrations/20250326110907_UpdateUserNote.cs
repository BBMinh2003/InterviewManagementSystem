using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8474), new DateTime(2026, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8602), new DateTime(2025, 4, 2, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8914), new DateTime(2027, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8914), new DateTime(2025, 4, 5, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8911) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8922), new DateTime(2026, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8923), new DateTime(2025, 3, 31, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8922) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8929), new DateTime(2026, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8930), new DateTime(2025, 4, 9, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8929) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8953), new DateTime(2027, 3, 26, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8953), new DateTime(2025, 4, 5, 11, 9, 4, 380, DateTimeKind.Utc).AddTicks(8952) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "ConcurrencyStamp",
                value: "8f2b2684-d935-48fe-9af8-f8eca90b2e88");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "ConcurrencyStamp",
                value: "5ab60cec-c2bf-4ed7-94ed-cf0b1c8d2f34");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-cccc-cccc-cccccccccccc"),
                column: "ConcurrencyStamp",
                value: "ea6867de-05c6-4381-9d62-4ea15c3f3fd7");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "ConcurrencyStamp",
                value: "6c192abe-854e-4033-90f3-b3e3221997aa");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "ConcurrencyStamp",
                value: "40d82d11-f289-4fa2-acbd-27a9f090f86f");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "7bcdc464-7e10-457e-9b9f-a444747b761c");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(3928), new DateTime(2026, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4046), new DateTime(2025, 4, 2, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(3719) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4341), new DateTime(2027, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4342), new DateTime(2025, 4, 5, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4338) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4352), new DateTime(2026, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4352), new DateTime(2025, 3, 31, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4351) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4360), new DateTime(2026, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4360), new DateTime(2025, 4, 9, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4359) });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Offers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                columns: new[] { "ContactPeriodFrom", "ContactPeriodTo", "DueDate" },
                values: new object[] { new DateTime(2025, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4366), new DateTime(2027, 3, 26, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4367), new DateTime(2025, 4, 5, 2, 35, 8, 484, DateTimeKind.Utc).AddTicks(4366) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "ConcurrencyStamp",
                value: "d594b020-37d0-46f6-bf72-594b82dd21b5");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "ConcurrencyStamp",
                value: "ffb8c46b-8f62-42e5-be66-272a28ee6a95");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-cccc-cccc-cccccccccccc"),
                column: "ConcurrencyStamp",
                value: "414a511d-20c1-4e07-b3a0-ff4cd76e94b7");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "ConcurrencyStamp",
                value: "f22c89e7-4324-48ef-b767-50fca12e3f85");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "ConcurrencyStamp",
                value: "7ffe75d9-3979-4670-8f48-3dd57f07604b");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "7401cd56-5069-4dc0-a427-5c05f8cd2ebd");
        }
    }
}
