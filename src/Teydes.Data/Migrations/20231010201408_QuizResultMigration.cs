using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuizResultMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Quizzes_TestId",
                table: "QuizResults");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "QuizResults",
                newName: "QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResults_TestId",
                table: "QuizResults",
                newName: "IX_QuizResults_QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Quizzes_QuizId",
                table: "QuizResults",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Quizzes_QuizId",
                table: "QuizResults");

            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "QuizResults",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResults_QuizId",
                table: "QuizResults",
                newName: "IX_QuizResults_TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Quizzes_TestId",
                table: "QuizResults",
                column: "TestId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
