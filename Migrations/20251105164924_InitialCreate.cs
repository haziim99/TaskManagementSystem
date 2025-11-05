using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedDate", "Description", "DueDate", "IsCompleted", "Priority", "Status", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 17, 49, 24, 651, DateTimeKind.Local).AddTicks(3802), "Design and create the project database", null, true, 2, 2, "Create Database" },
                    { 2, new DateTime(2025, 11, 5, 17, 49, 24, 651, DateTimeKind.Local).AddTicks(4178), "Design a professional and user-friendly interface", new DateTime(2025, 11, 12, 17, 49, 24, 651, DateTimeKind.Local).AddTicks(4179), false, 1, 1, "Develop User Interface" },
                    { 3, new DateTime(2025, 11, 5, 17, 49, 24, 651, DateTimeKind.Local).AddTicks(4353), "Implement login and registration system", new DateTime(2025, 11, 19, 17, 49, 24, 651, DateTimeKind.Local).AddTicks(4354), false, 2, 0, "Add Authentication System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
