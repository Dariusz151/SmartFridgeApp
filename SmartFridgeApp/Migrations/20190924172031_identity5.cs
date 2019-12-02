using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartFridgeApp.Migrations
{
    public partial class identity5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserGroups_UserGroupId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_AppUsers_UserId",
                table: "FoodItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_UserGroupId",
                table: "User",
                newName: "IX_User_UserGroupId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserGroupsId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserGroupsId",
                table: "AspNetUsers",
                column: "UserGroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserGroups_UserGroupsId",
                table: "AspNetUsers",
                column: "UserGroupsId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_User_UserId",
                table: "FoodItems",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserGroups_UserGroupId",
                table: "User",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserGroups_UserGroupsId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_User_UserId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserGroups_UserGroupId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserGroupsId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserGroupsId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "AppUsers");

            migrationBuilder.RenameIndex(
                name: "IX_User_UserGroupId",
                table: "AppUsers",
                newName: "IX_AppUsers_UserGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserGroups_UserGroupId",
                table: "AppUsers",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_AppUsers_UserId",
                table: "FoodItems",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
