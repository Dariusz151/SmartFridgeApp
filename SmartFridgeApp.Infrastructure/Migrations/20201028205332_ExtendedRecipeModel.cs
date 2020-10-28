using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class ExtendedRecipeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelOfDifficulty",
                schema: "app",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredTime",
                schema: "app",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelOfDifficulty",
                schema: "app",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RequiredTime",
                schema: "app",
                table: "Recipes");
        }
    }
}
