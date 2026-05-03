using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniteCraftGame.Migrations
{
    /// <inheritdoc />
    public partial class AddDefinitionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "WordCombinations",
                type: "character varying(2550)",
                maxLength: 2550,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "UserWords",
                type: "character varying(2550)",
                maxLength: 2550,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Definition",
                table: "WordCombinations");

            migrationBuilder.DropColumn(
                name: "Definition",
                table: "UserWords");
        }
    }
}
