using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuizResultScoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "QuizResults",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "QuizResults");
        }
    }
}
