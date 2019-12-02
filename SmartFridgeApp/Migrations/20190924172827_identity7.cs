using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Migrations
{
    public partial class identity7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_AspNetUsers_ApplicationUserId1",
                table: "FoodItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_ApplicationUserId1",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "FoodItems");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "FoodItems",
                nullable: true,
                oldClrType: typeof(Guid));

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

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "FoodItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "FoodItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_ApplicationUserId1",
                table: "FoodItems",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_AspNetUsers_ApplicationUserId1",
                table: "FoodItems",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
