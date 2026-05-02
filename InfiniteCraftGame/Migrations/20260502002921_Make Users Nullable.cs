using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniteCraftGame.Migrations
{
    /// <inheritdoc />
    public partial class MakeUsersNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWords_User_UserId",
                table: "UserWords");

            migrationBuilder.DropForeignKey(
                name: "FK_WordCombinations_User_DiscoveredById",
                table: "WordCombinations");

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscoveredById",
                table: "WordCombinations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserWords",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWords_User_UserId",
                table: "UserWords",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WordCombinations_User_DiscoveredById",
                table: "WordCombinations",
                column: "DiscoveredById",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWords_User_UserId",
                table: "UserWords");

            migrationBuilder.DropForeignKey(
                name: "FK_WordCombinations_User_DiscoveredById",
                table: "WordCombinations");

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscoveredById",
                table: "WordCombinations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserWords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWords_User_UserId",
                table: "UserWords",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordCombinations_User_DiscoveredById",
                table: "WordCombinations",
                column: "DiscoveredById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
