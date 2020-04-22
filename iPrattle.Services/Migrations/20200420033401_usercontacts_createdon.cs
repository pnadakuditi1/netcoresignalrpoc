using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iPrattle.Services.Migrations
{
    public partial class usercontacts_createdon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserContact_UserId",
                table: "UserContact");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserContact",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserContact_UserId_ContactId",
                table: "UserContact",
                columns: new[] { "UserId", "ContactId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserContact_UserId_ContactId",
                table: "UserContact");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserContact");

            migrationBuilder.CreateIndex(
                name: "IX_UserContact_UserId",
                table: "UserContact",
                column: "UserId");
        }
    }
}
