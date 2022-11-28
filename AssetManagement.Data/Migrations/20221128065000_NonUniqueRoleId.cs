using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Data.Migrations
{
    public partial class NonUniqueRoleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUser");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "6eb67d67-a702-4e37-9eea-92a47294ec2b");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "c9c3cea2-7f4d-43d9-a9dd-3254761be4eb");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "9ebfd8a1-6b69-4071-8795-f0f4d6d1dd43", new DateTime(2022, 11, 28, 13, 50, 0, 505, DateTimeKind.Local).AddTicks(8396), "AQAAAAEAACcQAAAAEBLWR/6AHaz1QjthNV5cnoX2baPJ0YIs62QptAyY3xXEbjVtLjlG6bfqkD+scKmk8g==" });

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "df92ec2e-c8ba-49d0-ab6c-b3285b9b3d06", new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(8970), "AQAAAAEAACcQAAAAEEHVw0woVMDw+xw4xYKwmJjaJHe9JD/r/a36Pw5YzaFo7FPHR50KY/tft8JiatOEpg==" });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9077));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9091));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9101));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9111));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9160));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9174));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 7,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9184));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 8,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 9,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9202));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 10,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 13, 50, 0, 511, DateTimeKind.Local).AddTicks(9214));

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUser",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUser");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "9ebbb802-dfa8-4d96-a5b7-66f8c8db1529");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "f48978a5-4704-47f4-8eb4-5f6b24345966");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "751572cb-786a-41ff-b034-2244125e852c", new DateTime(2022, 11, 28, 3, 8, 13, 206, DateTimeKind.Local).AddTicks(4684), "AQAAAAEAACcQAAAAELnjF8R4L1zm4r0vMehTaw1SG+jnXiCShS03EzNJmRd3svzSbPXFQN8DmVoAGHXD4g==" });

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "0c29f395-8334-470c-a295-cd3ba69273cb", new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(2849), "AQAAAAEAACcQAAAAEDGrqiZewXVV0oyTafGu065y7HzLJ2yLM1P/gMMlSqcqkYlJ4PkwQ/GTuGQucJY5Zg==" });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3205));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3220));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3230));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3248));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 7,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3276));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 8,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 9,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3377));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 10,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 3, 8, 13, 213, DateTimeKind.Local).AddTicks(3389));

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUser",
                column: "RoleId",
                unique: true);
        }
    }
}
