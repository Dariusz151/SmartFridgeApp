using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Migrations
{
    public partial class addConsumedFoodItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Users_UserId",
                table: "FoodItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FoodItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConsumedFoodItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FoodItemName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    ConsumedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedFoodItems", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Users_UserId",
                table: "FoodItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Users_UserId",
                table: "FoodItems");

            migrationBuilder.DropTable(
                name: "ConsumedFoodItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FoodItems",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Users_UserId",
                table: "FoodItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
