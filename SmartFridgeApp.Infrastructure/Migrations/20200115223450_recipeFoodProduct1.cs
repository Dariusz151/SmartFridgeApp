using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class recipeFoodProduct1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodProducts_Recipes_RecipeId",
                table: "FoodProducts");

            migrationBuilder.DropIndex(
                name: "IX_FoodProducts_RecipeId",
                table: "FoodProducts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "MinutesRequired",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "FoodProducts");

            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "FoodProducts",
                newName: "FoodProducts",
                newSchema: "app");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recipes",
                newName: "RecipeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "app",
                table: "FoodProducts",
                newName: "FoodProductId");

            migrationBuilder.CreateTable(
                name: "RecipeFoodProducts",
                schema: "app",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false),
                    FoodProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeFoodProducts", x => new { x.FoodProductId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeFoodProducts_FoodProducts_FoodProductId",
                        column: x => x.FoodProductId,
                        principalSchema: "app",
                        principalTable: "FoodProducts",
                        principalColumn: "FoodProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeFoodProducts_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeFoodProducts_RecipeId",
                schema: "app",
                table: "RecipeFoodProducts",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeFoodProducts",
                schema: "app");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "app",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "FoodProducts",
                schema: "app",
                newName: "FoodProducts");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "Recipes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FoodProductId",
                table: "FoodProducts",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyLevel",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesRequired",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "FoodProducts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodProducts_RecipeId",
                table: "FoodProducts",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodProducts_Recipes_RecipeId",
                table: "FoodProducts",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
