using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniteCraftGame.Migrations
{
    /// <inheritdoc />
    public partial class AddEmojiColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Emoji",
                table: "WordCombinations",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Emoji",
                table: "UserWords",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "WordCombinations");

            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "UserWords");
        }
    }
}
