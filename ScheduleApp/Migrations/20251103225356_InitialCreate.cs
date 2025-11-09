using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScheduleApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DayOfWeek = table.Column<string>(type: "text", nullable: false),
                    PeriodNumber = table.Column<int>(type: "integer", nullable: false),
                    SubjectName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ScheduleItems",
                columns: new[] { "Id", "CreatedAt", "DayOfWeek", "PeriodNumber", "SubjectName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Monday", 2, "Numerical Methods", null },
                    { 2, new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Monday", 3, "Computer Engineering", null },
                    { 3, new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Monday", 5, "Philosophy", null },
                    { 4, new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Wednesday", 3, "Numerical Methods", null },
                    { 5, new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Wednesday", 4, "Intelligent Programming Systems", null },
                    { 6, new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Wednesday", 6, "Numerical Methods", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_DayOfWeek_PeriodNumber",
                table: "ScheduleItems",
                columns: new[] { "DayOfWeek", "PeriodNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleItems");
        }
    }
}
