using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Data.Migrations
{
    public partial class SeedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Assets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Location",
                table: "Assets",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AppUser",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_AppUser_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assignments_AppUser_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assignments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "6e366294-68e4-4c58-82e7-b6235ab15a12");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "a21f15d3-d9a2-43ea-92d8-4de01d506724");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "ce50172c-a580-44d4-abfc-8f2561304e84", new DateTime(2022, 11, 24, 15, 17, 9, 312, DateTimeKind.Local).AddTicks(7415), "AQAAAAEAACcQAAAAEMPTWMiN4TNoeqa9mRgBLZHWCQEJz6qk4q07LorlkIyCCWygTsOW2u2WJdYR5tjuDQ==" });

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "2836ad41-6059-457e-bdc4-f06019d6254b", new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(2888), "AQAAAAEAACcQAAAAEIR9Y87kfIdBysczWPzcYokD2KP+bD6NDFXb8xn3iDjVwDsmW+EuoPQFCl0Nm+dQsA==" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsDeleted", "Name", "Prefix" },
                values: new object[,]
                {
                    { 1, false, "Laptop", "LA" },
                    { 2, false, "Monitor", "MO" }
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCode", "CategoryId", "InstalledDate", "IsDeleted", "Location", "Name", "Specification", "State" },
                values: new object[,]
                {
                    { 1, "LA100001", 2, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3087), false, 0, "Laptop 1", "Core i1, 1GB RAM, 150 GB HDD, Window 1", 1 },
                    { 2, "LA100002", 1, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3103), true, 0, "Laptop 2", "Core i2, 2GB RAM, 250 GB HDD, Window 2", 0 },
                    { 3, "LA100003", 2, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3114), false, 0, "Laptop 3", "Core i3, 3GB RAM, 350 GB HDD, Window 3", 1 },
                    { 4, "LA100004", 1, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3124), true, 0, "Laptop 4", "Core i4, 4GB RAM, 450 GB HDD, Window 4", 0 },
                    { 5, "LA100005", 2, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3134), false, 0, "Laptop 5", "Core i5, 5GB RAM, 550 GB HDD, Window 5", 1 },
                    { 6, "LA100006", 1, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3152), true, 0, "Laptop 6", "Core i6, 6GB RAM, 650 GB HDD, Window 6", 0 },
                    { 7, "LA100007", 2, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3161), false, 0, "Laptop 7", "Core i7, 7GB RAM, 750 GB HDD, Window 7", 1 },
                    { 8, "LA100008", 1, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3264), true, 0, "Laptop 8", "Core i8, 8GB RAM, 850 GB HDD, Window 8", 0 },
                    { 9, "LA100009", 2, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3276), false, 0, "Laptop 9", "Core i9, 9GB RAM, 950 GB HDD, Window 9", 1 },
                    { 10, "LA1000010", 1, new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3289), true, 0, "Laptop 10", "Core i10, 10GB RAM, 1050 GB HDD, Window 10", 0 }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssetId", "AssignedBy", "AssignedDate", "AssignedTo", "ReturnedDate", "State" },
                values: new object[] { 1, 1, new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3306), new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new DateTime(2022, 11, 24, 15, 17, 9, 320, DateTimeKind.Local).AddTicks(3306), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CategoryId",
                table: "Assets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_UserName",
                table: "AppUser",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssetId",
                table: "Assignments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedBy",
                table: "Assignments",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedTo",
                table: "Assignments",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Categories_CategoryId",
                table: "Assets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Categories_CategoryId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Assets_CategoryId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_UserName",
                table: "AppUser");

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Assets");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("12147fe0-4571-4ad2-b8f7-d2c863eb78a5"),
                column: "ConcurrencyStamp",
                value: "7027a446-e8c2-4b00-bc09-2aefc892ba57");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "acfb6c92-c938-4485-ae2c-32addb60154a");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "1f5d5de4-0a8f-4e38-aad2-629dc6548e1f", new DateTime(2022, 11, 22, 10, 15, 18, 550, DateTimeKind.Local).AddTicks(6959), "AQAAAAEAACcQAAAAEJa907HNcTamMkcpMbHn3vH53c5Ppq4K14JUMo393SmcCtzM/ar4u0XN1lH2g2OWFQ==" });

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("70bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "134fc0d3-2394-4738-bc9c-43acef5271d4", new DateTime(2022, 11, 22, 10, 15, 18, 557, DateTimeKind.Local).AddTicks(3674), "AQAAAAEAACcQAAAAEJFjyQiaZQe3BwtkxgGbZEJ4ZqZT+swVAzTdM1KdC1aNTALr7Oy5OpYkHBMsS4hH/Q==" });
        }
    }
}
