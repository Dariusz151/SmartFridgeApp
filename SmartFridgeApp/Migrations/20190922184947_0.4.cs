using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Migrations
{
    public partial class _04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "ConsumedFoodItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "ConsumedFoodItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ConsumedFoodItems");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ConsumedFoodItems");
        }
    }
}
