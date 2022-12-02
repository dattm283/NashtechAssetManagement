using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Data.Migrations
{
    public partial class UpdateSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "33ed9da7-35aa-46da-9ed0-a7028245b937");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "ec82f8a5-713b-4b22-8d7b-498d6e10270f");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"), new Guid("70bd814f-9576-45ba-b5b7-f00649be00de") });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00bf"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "83ffff5c-3b00-4d82-bd66-2a2ae967fa4e", new DateTime(2022, 12, 2, 10, 31, 32, 992, DateTimeKind.Local).AddTicks(1385), new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEMdHb3SDr8KHSCwenrFwmAb2dpOrA4LvG0T9NxcTH7r7kucQtc60rLiAUdnHZi5xyQ==", "SD0002" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "4002fd1a-b92b-4ab9-a4b9-79ebd08ed61e", new DateTime(2022, 12, 2, 10, 31, 32, 984, DateTimeKind.Local).AddTicks(7978), new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEEn9e2wK6v4JMmITMiGhmTLPayo3vSSv0jtRIpnBqECNG+3Bd0MiVD/rhyt3GMCO6A==", "SD0001" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "d73e49a0-2ddf-47bd-b2fe-73eeffe4a074", new DateTime(2022, 12, 2, 10, 31, 32, 999, DateTimeKind.Local).AddTicks(5476), new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEP6U7ktsudVoP/LwXIVgG93lOt0f+JyG8sj8+xEeBtiAf5MEdQSzokKu0YjDmfrvuQ==", "SD0003" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd814f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "6ac20c26-3534-48bb-9437-e942e40d3ece", new DateTime(2022, 12, 2, 10, 31, 33, 5, DateTimeKind.Local).AddTicks(9667), new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEFhCKEPsDMsG6dhsmCtSQ/zGK7poQ1vcMP9q6IdFYWOeuMpxmIGVFXkquGmvDlql8A==", "SD0004" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("73bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "f2269894-ed4c-43a3-94b8-ab807a039bd5", new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4035), new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEGoToFsr4EZJir6os3J0mz3W2BGHdFEr8M3etBs/E9GZfTUpm+wY7xmfs4BLlvNcCQ==", "SD0005" });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4536));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4560));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4578));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4588));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 7,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 8,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4606));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 9,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4615));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 10,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4647));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 1, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 3, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 2, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 4, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 3, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 5, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 4, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 6, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 5, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 6, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 8, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 7, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 9, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 8, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 9, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { 10, new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"), new Guid("70bd814f-9576-45ba-b5b7-f00649be00de") });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "be605214-67ee-4836-b562-785fbebd7c2e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "17c519fc-6bc5-4f90-94a8-ddd3c0eb3b2e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00bf"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "88354f28-29eb-49b2-b49a-0d3071b44181", new DateTime(2022, 11, 28, 15, 34, 16, 123, DateTimeKind.Local).AddTicks(7956), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEIq8cdgtoEtQNSHIe+6hTFaq55bxWCzEOqIeU5o4fEr5boXHY/1kKw74C75REvrK3A==", " SD0002" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "4ba65805-c5e3-4b65-a465-70859f6e28d7", new DateTime(2022, 11, 28, 15, 34, 16, 116, DateTimeKind.Local).AddTicks(7583), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEJ2fIlkofKgDH9GZPrBzwSAxUxIAE3m2UxnWSpuOx/1kc0n4i7unrZoGr0mrle1FVA==", " SD0001" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "e4a8a9f7-3607-4532-85cf-d4aae045a9b0", new DateTime(2022, 11, 28, 15, 34, 16, 130, DateTimeKind.Local).AddTicks(2999), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAELO1gKNHMO8GvxY+7ftChoQQNsGsp6mKfVAPYPU0eSGs+/W3Ui/SRG3UN/GDjYouTw==", " SD0003" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd814f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "d0ebbadb-e605-476f-86d3-8d1809c0c3d7", new DateTime(2022, 11, 28, 15, 34, 16, 137, DateTimeKind.Local).AddTicks(5311), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEJVvJAuApziWfQIch2e23yBbZP2mzQXA4TZTnxCoKC3AuWS6OR3DQA3ixyHWui4Ahw==", " SD0004" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("73bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "Dob", "PasswordHash", "StaffCode" },
                values: new object[] { "946a0f03-6fdb-4751-9cb3-8f0db45f8425", new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1198), new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEI02rO2Zt7fyIM4QZe1T+e4oyO0UgPnpnhop/gbHhHDnjyQflB4dvMXaD2lV3/5MYA==", " SD0005" });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1664));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1678));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1687));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1696));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1705));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1715));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 7,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1723));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 8,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1732));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 9,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1740));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 10,
                column: "InstalledDate",
                value: new DateTime(2022, 11, 28, 15, 34, 16, 145, DateTimeKind.Local).AddTicks(1751));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 1, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 3, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 4, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 5, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 6, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AssetId", "AssignedDate", "ReturnedDate" },
                values: new object[] { null, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 8, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
