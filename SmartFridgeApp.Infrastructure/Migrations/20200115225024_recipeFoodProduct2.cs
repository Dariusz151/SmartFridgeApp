using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class recipeFoodProduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Fridges",
                newName: "Fridges",
                newSchema: "app");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "app",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "WelcomeEmailWasSent",
                schema: "app",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WelcomeEmailWasSent",
                schema: "app",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Fridges",
                schema: "app",
                newName: "Fridges");
        }
    }
}
