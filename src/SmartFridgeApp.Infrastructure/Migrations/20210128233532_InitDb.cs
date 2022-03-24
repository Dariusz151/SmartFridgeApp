using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.EnsureSchema(
                name: "internal");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "app",
                columns: table => new
                {
                    CategoryId = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Fridges",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    Desc = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fridges", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "internal",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OccurredOn = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    ProcessedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodProducts",
                schema: "app",
                columns: table => new
                {
                    FoodProductId = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    CategoryId = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodProducts", x => x.FoodProductId);
                    table.ForeignKey(
                        name: "FK_FoodProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "app",
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: false),
                    FridgeId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Fridges_FridgeId",
                        column: x => x.FridgeId,
                        principalSchema: "app",
                        principalTable: "Fridges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                schema: "app",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 5000, nullable: true),
                    RequiredTime = table.Column<int>(nullable: false, defaultValue: -1),
                    LevelOfDifficulty = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)3),
                    RecipeCategoryId = table.Column<short>(nullable: true),
                    FoodProducts = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_RecipeCategories_RecipeCategoryId",
                        column: x => x.RecipeCategoryId,
                        principalSchema: "app",
                        principalTable: "RecipeCategories",
                        principalColumn: "RecipeCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FridgeItems",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodProductId = table.Column<short>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    Value = table.Column<float>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    EnteredAt = table.Column<DateTime>(nullable: false),
                    IsConsumed = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FridgeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FridgeItems_FoodProducts_FoodProductId",
                        column: x => x.FoodProductId,
                        principalSchema: "app",
                        principalTable: "FoodProducts",
                        principalColumn: "FoodProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FridgeItems_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodProducts_CategoryId",
                schema: "app",
                table: "FoodProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeItems_FoodProductId",
                schema: "app",
                table: "FridgeItems",
                column: "FoodProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeItems_UserId",
                schema: "app",
                table: "FridgeItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeCategoryId",
                schema: "app",
                table: "Recipes",
                column: "RecipeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FridgeId",
                schema: "app",
                table: "Users",
                column: "FridgeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FridgeItems",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Recipes",
                schema: "app");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "internal");

            migrationBuilder.DropTable(
                name: "FoodProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "app");

            migrationBuilder.DropTable(
                name: "RecipeCategories",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Fridges",
                schema: "app");
        }
    }
}
