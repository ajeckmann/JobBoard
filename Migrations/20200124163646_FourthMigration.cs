using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamCSharp.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funthings_Users_CreatorUserId",
                table: "Funthings");

            migrationBuilder.DropIndex(
                name: "IX_Funthings_CreatorUserId",
                table: "Funthings");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Funthings");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Funthings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Funthings_UserID",
                table: "Funthings",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Funthings_Users_UserID",
                table: "Funthings",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funthings_Users_UserID",
                table: "Funthings");

            migrationBuilder.DropIndex(
                name: "IX_Funthings_UserID",
                table: "Funthings");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Funthings");

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Funthings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funthings_CreatorUserId",
                table: "Funthings",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funthings_Users_CreatorUserId",
                table: "Funthings",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
