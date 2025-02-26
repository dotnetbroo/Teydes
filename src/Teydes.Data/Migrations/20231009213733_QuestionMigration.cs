﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teydes.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Assets_AssetId",
                table: "Questions");

            migrationBuilder.AlterColumn<long>(
                name: "AssetId",
                table: "Questions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Assets_AssetId",
                table: "Questions",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Assets_AssetId",
                table: "Questions");

            migrationBuilder.AlterColumn<long>(
                name: "AssetId",
                table: "Questions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Assets_AssetId",
                table: "Questions",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
