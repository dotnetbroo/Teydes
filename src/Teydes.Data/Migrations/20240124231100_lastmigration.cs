using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class lastmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 1, 24, 23, 11, 0, 26, DateTimeKind.Utc).AddTicks(8421), new DateTime(2024, 1, 24, 23, 11, 0, 26, DateTimeKind.Utc).AddTicks(8423) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 11, 26, 15, 0, 22, 149, DateTimeKind.Utc).AddTicks(4087), new DateTime(2023, 11, 26, 15, 0, 22, 149, DateTimeKind.Utc).AddTicks(4090) });
        }
    }
}
