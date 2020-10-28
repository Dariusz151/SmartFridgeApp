using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    public partial class SpecifiedRecipeEntityTypeBuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RequiredTime",
                schema: "app",
                table: "Recipes",
                nullable: false,
                defaultValue: -1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "LevelOfDifficulty",
                schema: "app",
                table: "Recipes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)3,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RequiredTime",
                schema: "app",
                table: "Recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: -1);

            migrationBuilder.AlterColumn<int>(
                name: "LevelOfDifficulty",
                schema: "app",
                table: "Recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)3);
        }
    }
}
