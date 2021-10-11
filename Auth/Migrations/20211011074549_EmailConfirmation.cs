using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Migrations
{
    public partial class EmailConfirmation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RefreshTokens_RefreshTokenId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "RefreshTokenId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedEmail",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RefreshTokens_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId",
                principalTable: "RefreshTokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RefreshTokens_RefreshTokenId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsConfirmedEmail",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "RefreshTokenId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RefreshTokens_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId",
                principalTable: "RefreshTokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
