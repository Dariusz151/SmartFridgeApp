using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class RecipeTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "app",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "app",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyLevel",
                schema: "app",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesRequired",
                schema: "app",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                schema: "app",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "app",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                schema: "app",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "MinutesRequired",
                schema: "app",
                table: "Recipes");
        }
    }
}
