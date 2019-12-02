using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Migrations
{
    public partial class identity4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "FoodItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_ApplicationUserId",
                table: "FoodItems",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_AspNetUsers_ApplicationUserId",
                table: "FoodItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_AspNetUsers_ApplicationUserId",
                table: "FoodItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_ApplicationUserId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "FoodItems");
        }
    }
}
