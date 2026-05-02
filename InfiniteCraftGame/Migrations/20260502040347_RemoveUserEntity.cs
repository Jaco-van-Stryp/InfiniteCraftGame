using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniteCraftGame.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserEntity : Migration
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

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_WordCombinations_DiscoveredById",
                table: "WordCombinations");

            migrationBuilder.DropIndex(
                name: "IX_UserWords_UserId",
                table: "UserWords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordCombinations_DiscoveredById",
                table: "WordCombinations",
                column: "DiscoveredById");

            migrationBuilder.CreateIndex(
                name: "IX_UserWords_UserId",
                table: "UserWords",
                column: "UserId");

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
    }
}
