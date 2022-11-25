using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"), "a105cbf2-5a53-4c16-8f42-b62c5425608f", "Staff role", "Staff", "staff" },
                    { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "53c17004-d6c6-4b61-ba94-016863e21591", "Administrator role", "Admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), new Guid("69bd714f-9576-45ba-b5b7-f00649be00de") });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedDate", "Dob", "Email", "EmailConfirmed", "FirstName", "Gender", "IsLoginFirstTime", "LastName", "Location", "LockoutEnabled", "LockoutEnd", "ModifiedDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), 0, "56a12768-c35d-4d6f-b371-a7d2aa46c5aa", new DateTime(2022, 11, 25, 3, 14, 22, 580, DateTimeKind.Local).AddTicks(6226), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Toan", 0, true, "Bach", 0, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "admin", "AQAAAAEAACcQAAAAEAq7/PyW9OOqv2Px2/XSsD51D/svp/sG/xKz6ggVVI9gg/tdF5LZtiCghLK5mGhz3A==", null, false, new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "", false, "admin" },
                    { new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), 0, "321bed9e-ad74-4ed2-8161-a5ef7100835c", new DateTime(2022, 11, 25, 3, 14, 22, 593, DateTimeKind.Local).AddTicks(9726), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "staff@gmail.com", true, "Toan", 1, true, "Bach", 1, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "staff@gmail.com", "staff", "AQAAAAEAACcQAAAAEMBGzTSjBcDfwUfgC/u8BvZQtgPhbGU+kfCWSepIG1K5Z0fn+oHyeoq3n6Ml3tViWA==", null, false, new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"), "", false, "staff" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"));

            migrationBuilder.DeleteData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), new Guid("69bd714f-9576-45ba-b5b7-f00649be00de") });

            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"));

            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"));
        }
    }
}
