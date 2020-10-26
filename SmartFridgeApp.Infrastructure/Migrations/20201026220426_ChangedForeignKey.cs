using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class ChangedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersFridgeItems",
                schema: "app",
                table: "FridgeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FridgeUsers",
                schema: "app",
                table: "Users");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FridgeItems_Users_UserId",
                schema: "app",
                table: "FridgeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Fridges_FridgeId",
                schema: "app",
                table: "Users");

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
                name: "FK_FridgeUsers",
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
