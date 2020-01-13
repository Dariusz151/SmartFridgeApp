using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class ChangesInFridgeItemStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FridgeItems");

            migrationBuilder.AddColumn<int>(
                name: "FoodProductId",
                table: "FridgeItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FridgeItems_FoodProductId",
                table: "FridgeItems",
                column: "FoodProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeItems_FoodProducts_FoodProductId",
                table: "FridgeItems",
                column: "FoodProductId",
                principalTable: "FoodProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FridgeItems_FoodProducts_FoodProductId",
                table: "FridgeItems");

            migrationBuilder.DropIndex(
                name: "IX_FridgeItems_FoodProductId",
                table: "FridgeItems");

            migrationBuilder.DropColumn(
                name: "FoodProductId",
                table: "FridgeItems");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FridgeItems",
                nullable: true);
        }
    }
}
