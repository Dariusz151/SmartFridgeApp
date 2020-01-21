using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class DatabaseConfigChanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipes",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "FoodProducts",
                newName: "FoodProducts",
                newSchema: "app");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Recipes",
                schema: "app",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "FoodProducts",
                schema: "app",
                newName: "FoodProducts");
        }
    }
}
