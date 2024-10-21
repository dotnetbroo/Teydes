using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "FirstName", "LastName", "Password", "PhoneNumber", "Role", "Salt", "UpdatedAt" },
                values: new object[] { 9L, new DateTime(2023, 10, 16, 10, 58, 56, 649, DateTimeKind.Utc).AddTicks(4758), "Javohir", "Boyaliyev", "$2a$11$Kv96RMO8xBWAdwgo8kYOD./o6LAhS9iZnbcYbTrUmYONN.lN4vj7m", "+998889084000", 3, "0b345b73-5e3c-47f3-8c39-79414f7fe1e3", new DateTime(2023, 10, 16, 10, 58, 56, 649, DateTimeKind.Utc).AddTicks(4759) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "FirstName", "LastName", "Password", "PhoneNumber", "Role", "Salt", "UpdatedAt" },
                values: new object[] { 1L, new DateTime(2023, 10, 16, 10, 56, 56, 470, DateTimeKind.Utc).AddTicks(3558), "Normadjon", "G'offorov", "$2a$11$Kv96RMO8xBWAdwgo8kYOD./o6LAhS9iZnbcYbTrUmYONN.lN4vj7m", "+998900981101", 3, "0b345b73-5e3c-47f3-8c39-79414f7fe1e3", new DateTime(2023, 10, 16, 10, 56, 56, 470, DateTimeKind.Utc).AddTicks(3561) });
        }
    }
}
