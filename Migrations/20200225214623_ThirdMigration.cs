using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeltExam.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Exercises",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CreatorUserId",
                table: "Exercises",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Users_CreatorUserId",
                table: "Exercises",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Users_CreatorUserId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_CreatorUserId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Exercises");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Exercises",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Exercises",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
