using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class AddedRecipeCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FridgeItems_Users_UserId",
                schema: "app",
                table: "FridgeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Fridges_FridgeId",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RecipeCategory",
                schema: "app",
                table: "Recipes");

            migrationBuilder.AddColumn<short>(
                name: "RecipeCategoryId",
                schema: "app",
                table: "Recipes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecipeCategories",
                schema: "app",
                columns: table => new
                {
                    RecipeCategoryId = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCategories", x => x.RecipeCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeCategoryId",
                schema: "app",
                table: "Recipes",
                column: "RecipeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersFridgeItems",
                schema: "app",
                table: "FridgeItems",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeCategories_RecipeCategoryId",
                schema: "app",
                table: "Recipes",
                column: "RecipeCategoryId",
                principalSchema: "app",
                principalTable: "RecipeCategories",
                principalColumn: "RecipeCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeUsers",
                schema: "app",
                table: "Users",
                column: "FridgeId",
                principalSchema: "app",
                principalTable: "Fridges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersFridgeItems",
                schema: "app",
                table: "FridgeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeCategories_RecipeCategoryId",
                schema: "app",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_FridgeUsers",
                schema: "app",
                table: "Users");

            migrationBuilder.DropTable(
                name: "RecipeCategories",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_RecipeCategoryId",
                schema: "app",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeCategoryId",
                schema: "app",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "RecipeCategory",
                schema: "app",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeItems_Users_UserId",
                schema: "app",
                table: "FridgeItems",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Fridges_FridgeId",
                schema: "app",
                table: "Users",
                column: "FridgeId",
                principalSchema: "app",
                principalTable: "Fridges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
