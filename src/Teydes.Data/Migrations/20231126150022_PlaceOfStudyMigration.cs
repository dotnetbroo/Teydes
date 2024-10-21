using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlaceOfStudyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStudyForeign",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "IsStudyForeign", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 11, 26, 15, 0, 22, 149, DateTimeKind.Utc).AddTicks(4087), false, new DateTime(2023, 11, 26, 15, 0, 22, 149, DateTimeKind.Utc).AddTicks(4090) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStudyForeign",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 16, 10, 58, 56, 649, DateTimeKind.Utc).AddTicks(4758), new DateTime(2023, 10, 16, 10, 58, 56, 649, DateTimeKind.Utc).AddTicks(4759) });
        }
    }
}
