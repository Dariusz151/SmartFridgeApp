using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class updateOutboxCommandSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "internal");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "app",
                newName: "OutboxMessages",
                newSchema: "internal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "internal",
                newName: "OutboxMessages",
                newSchema: "app");
        }
    }
}
