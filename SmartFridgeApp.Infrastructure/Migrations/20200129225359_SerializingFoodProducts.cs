using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class SerializingFoodProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeFoodProduct");

            migrationBuilder.AddColumn<string>(
                name: "FoodProducts",
                schema: "app",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodProducts",
                schema: "app",
                table: "Recipes");

            migrationBuilder.CreateTable(
                name: "RecipeFoodProduct",
                columns: table => new
                {
                    FoodProductId = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeFoodProduct", x => new { x.FoodProductId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeFoodProduct_FoodProducts_FoodProductId",
                        column: x => x.FoodProductId,
                        principalSchema: "app",
                        principalTable: "FoodProducts",
                        principalColumn: "FoodProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeFoodProduct_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "app",
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeFoodProduct_RecipeId",
                table: "RecipeFoodProduct",
                column: "RecipeId");
        }
    }
}
