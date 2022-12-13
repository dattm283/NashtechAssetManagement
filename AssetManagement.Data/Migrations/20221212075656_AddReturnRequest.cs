using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Data.Migrations
{
    public partial class AddReturnRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ReturnRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcceptedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnRequests_AspNetUsers_AcceptedBy",
                        column: x => x.AcceptedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnRequests_AspNetUsers_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnRequests_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "dfc42204-9b72-4ac8-a08a-0d4930896e17");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "f7238206-c225-4499-aa72-4922ab40268e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00bf"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "ce506485-a76a-435f-a1bf-bec7ec38af85", new DateTime(2022, 12, 12, 14, 56, 56, 103, DateTimeKind.Local).AddTicks(727), "AQAAAAEAACcQAAAAEIQ59TNALkLADF2paA5pzdctM7veVfvrEjGFcH3xUAAQQmhE/iQqGn/5jhXPQhU3Vg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "db4bfe25-3ef9-4770-824b-c48448811652", new DateTime(2022, 12, 12, 14, 56, 56, 96, DateTimeKind.Local).AddTicks(7051), "AQAAAAEAACcQAAAAEIsMw5/bxy927fUXHr66aRLtn5v6wHUWF5EnaMP+ynKiTu1QhlENy+if94znixHkfA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "3ad20ac2-3783-40fe-8876-eae08719e020", new DateTime(2022, 12, 12, 14, 56, 56, 109, DateTimeKind.Local).AddTicks(6042), "AQAAAAEAACcQAAAAENBmgdZdxCCpA5Da9643w+QEcx1+mQhnZyiDNoAb0xRE6wA1C4lPaN6QsgMiE6BYRg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd814f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "7a35a2c9-f280-4e56-9130-4d4a6337d7d4", new DateTime(2022, 12, 12, 14, 56, 56, 115, DateTimeKind.Local).AddTicks(9408), "AQAAAAEAACcQAAAAENgmsNptiMMbWCqs+Epqx8pgGTuFLv5xXX1xyt8ZbbRNqD0IdTFg66ItpDtuhIGfCA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("73bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "ad3f4d69-0690-412a-a264-bad9471d3a78", new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5031), "AQAAAAEAACcQAAAAEAndSbMRHxBDh51SePd1C+rmy8VnJD4v1z28MCmP+Dpi5RV7icG1xqVYZpB5OYsf9w==" });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5409));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5418));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5427));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5436));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5446));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 7,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5455));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 8,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5464));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 9,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5472));

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 10,
                column: "InstalledDate",
                value: new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5484));

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCode", "CategoryId", "InstalledDate", "IsDeleted", "Location", "Name", "Specification", "State" },
                values: new object[,]
                {
                    { 11, "LA1000011", 2, new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5495), false, 0, "Laptop 11", "Core i11, 11GB RAM, 1150 GB HDD, Window 11", 1 },
                    { 12, "LA1000012", 1, new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5504), true, 0, "Laptop 12", "Core i12, 12GB RAM, 1250 GB HDD, Window 12", 4 },
                    { 13, "LA1000013", 2, new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5545), false, 0, "Laptop 13", "Core i13, 13GB RAM, 1350 GB HDD, Window 13", 1 },
                    { 14, "LA1000014", 1, new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5555), true, 0, "Laptop 14", "Core i14, 14GB RAM, 1450 GB HDD, Window 14", 4 },
                    { 15, "LA1000015", 2, new DateTime(2022, 12, 12, 14, 56, 56, 122, DateTimeKind.Local).AddTicks(5564), false, 0, "Laptop 15", "Core i15, 15GB RAM, 1550 GB HDD, Window 15", 1 }
                });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 15, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 17, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 18, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 19, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 20, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 21, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 22, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssetId", "AssignedBy", "AssignedDate", "AssignedTo", "IsDeleted", "Note", "ReturnedDate", "State" },
                values: new object[] { 11, 4, new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), false, "Note for assignment 11", new DateTime(2022, 12, 23, 0, 0, 0, 0, DateTimeKind.Local), 1 });

            migrationBuilder.InsertData(
                table: "ReturnRequests",
                columns: new[] { "Id", "AcceptedBy", "AssignedBy", "AssignedDate", "AssignmentId", "IsDeleted", "ReturnedDate", "State" },
                values: new object[,]
                {
                    { 1, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 1, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 2, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 2, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 3, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 3, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 4, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 4, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 5, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 5, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 6, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 6, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 7, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 7, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 8, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 8, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 9, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 9, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 }
                });

            migrationBuilder.InsertData(
                table: "ReturnRequests",
                columns: new[] { "Id", "AcceptedBy", "AssignedBy", "AssignedDate", "AssignmentId", "IsDeleted", "ReturnedDate", "State" },
                values: new object[] { 10, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 10, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssetId", "AssignedBy", "AssignedDate", "AssignedTo", "IsDeleted", "Note", "ReturnedDate", "State" },
                values: new object[,]
                {
                    { 12, 12, new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), false, "Note for assignment 12", new DateTime(2022, 12, 24, 0, 0, 0, 0, DateTimeKind.Local), 3 },
                    { 13, 13, new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), false, "Note for assignment 13", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Local), 2 },
                    { 14, 14, new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), false, "Note for assignment 14", new DateTime(2022, 12, 26, 0, 0, 0, 0, DateTimeKind.Local), 3 },
                    { 15, 15, new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), false, "Note for assignment 15", new DateTime(2022, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), 2 }
                });

            migrationBuilder.InsertData(
                table: "ReturnRequests",
                columns: new[] { "Id", "AcceptedBy", "AssignedBy", "AssignedDate", "AssignmentId", "IsDeleted", "ReturnedDate", "State" },
                values: new object[] { 11, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 11, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.InsertData(
                table: "ReturnRequests",
                columns: new[] { "Id", "AcceptedBy", "AssignedBy", "AssignedDate", "AssignmentId", "IsDeleted", "ReturnedDate", "State" },
                values: new object[,]
                {
                    { 12, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 12, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 13, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 13, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 14, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 14, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 },
                    { 15, null, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 15, false, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequests_AcceptedBy",
                table: "ReturnRequests",
                column: "AcceptedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequests_AssignedBy",
                table: "ReturnRequests",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequests_AssignmentId",
                table: "ReturnRequests",
                column: "AssignmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnRequests");

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00bf"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "83ffff5c-3b00-4d82-bd66-2a2ae967fa4e", new DateTime(2022, 12, 2, 10, 31, 32, 992, DateTimeKind.Local).AddTicks(1385), "AQAAAAEAACcQAAAAEMdHb3SDr8KHSCwenrFwmAb2dpOrA4LvG0T9NxcTH7r7kucQtc60rLiAUdnHZi5xyQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "4002fd1a-b92b-4ab9-a4b9-79ebd08ed61e", new DateTime(2022, 12, 2, 10, 31, 32, 984, DateTimeKind.Local).AddTicks(7978), "AQAAAAEAACcQAAAAEEn9e2wK6v4JMmITMiGhmTLPayo3vSSv0jtRIpnBqECNG+3Bd0MiVD/rhyt3GMCO6A==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "d73e49a0-2ddf-47bd-b2fe-73eeffe4a074", new DateTime(2022, 12, 2, 10, 31, 32, 999, DateTimeKind.Local).AddTicks(5476), "AQAAAAEAACcQAAAAEP6U7ktsudVoP/LwXIVgG93lOt0f+JyG8sj8+xEeBtiAf5MEdQSzokKu0YjDmfrvuQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70bd814f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "6ac20c26-3534-48bb-9437-e942e40d3ece", new DateTime(2022, 12, 2, 10, 31, 33, 5, DateTimeKind.Local).AddTicks(9667), "AQAAAAEAACcQAAAAEFhCKEPsDMsG6dhsmCtSQ/zGK7poQ1vcMP9q6IdFYWOeuMpxmIGVFXkquGmvDlql8A==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("73bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "f2269894-ed4c-43a3-94b8-ab807a039bd5", new DateTime(2022, 12, 2, 10, 31, 33, 12, DateTimeKind.Local).AddTicks(4035), "AQAAAAEAACcQAAAAEGoToFsr4EZJir6os3J0mz3W2BGHdFEr8M3etBs/E9GZfTUpm+wY7xmfs4BLlvNcCQ==" });

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
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 3, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 4, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 5, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 6, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 8, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 9, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AssignedDate", "ReturnedDate" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
