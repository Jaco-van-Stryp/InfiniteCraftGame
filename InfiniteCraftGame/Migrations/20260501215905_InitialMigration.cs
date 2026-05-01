using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniteCraftGame.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "UserWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WordUnlocked = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWords_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordCombinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WordOne = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    WordTwo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    WordCombined = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DiscoveredById = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCombinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordCombinations_User_DiscoveredById",
                        column: x => x.DiscoveredById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWords_UserId",
                table: "UserWords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCombinations_DiscoveredById",
                table: "WordCombinations",
                column: "DiscoveredById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWords");

            migrationBuilder.DropTable(
                name: "WordCombinations");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
