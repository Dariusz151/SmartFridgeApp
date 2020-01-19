using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class EditRecipesStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFoodProducts_FoodProducts_FoodProductId",
                schema: "app",
                table: "RecipeFoodProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFoodProducts_Recipes_RecipeId",
                schema: "app",
                table: "RecipeFoodProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeFoodProducts",
                schema: "app",
                table: "RecipeFoodProducts");

            migrationBuilder.RenameTable(
                name: "Recipes",
                schema: "app",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "FoodProducts",
                schema: "app",
                newName: "FoodProducts");

            migrationBuilder.RenameTable(
                name: "RecipeFoodProducts",
                schema: "app",
                newName: "RecipeFoodProduct");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeFoodProducts_RecipeId",
                table: "RecipeFoodProduct",
                newName: "IX_RecipeFoodProduct_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeFoodProduct",
                table: "RecipeFoodProduct",
                columns: new[] { "FoodProductId", "RecipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFoodProduct_FoodProducts_FoodProductId",
                table: "RecipeFoodProduct",
                column: "FoodProductId",
                principalTable: "FoodProducts",
                principalColumn: "FoodProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFoodProduct_Recipes_RecipeId",
                table: "RecipeFoodProduct",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFoodProduct_FoodProducts_FoodProductId",
                table: "RecipeFoodProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFoodProduct_Recipes_RecipeId",
                table: "RecipeFoodProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeFoodProduct",
                table: "RecipeFoodProduct");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipes",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "FoodProducts",
                newName: "FoodProducts",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "RecipeFoodProduct",
                newName: "RecipeFoodProducts",
                newSchema: "app");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeFoodProduct_RecipeId",
                schema: "app",
                table: "RecipeFoodProducts",
                newName: "IX_RecipeFoodProducts_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeFoodProducts",
                schema: "app",
                table: "RecipeFoodProducts",
                columns: new[] { "FoodProductId", "RecipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFoodProducts_FoodProducts_FoodProductId",
                schema: "app",
                table: "RecipeFoodProducts",
                column: "FoodProductId",
                principalSchema: "app",
                principalTable: "FoodProducts",
                principalColumn: "FoodProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFoodProducts_Recipes_RecipeId",
                schema: "app",
                table: "RecipeFoodProducts",
                column: "RecipeId",
                principalSchema: "app",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
