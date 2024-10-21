using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestionAnswerMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option",
                table: "QuestionAnswers");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "QuestionAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "QuestionAnswers");

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "QuestionAnswers",
                type: "text",
                nullable: true);
        }
    }
}
