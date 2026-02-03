using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersDialogsJoins_UserIdentityId_DialogJoinId",
                table: "UsersDialogsJoins");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedDateUTC",
                table: "UsersDialogsJoins",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OutDateUTC",
                table: "UsersDialogsJoins",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_JoinedDateUTC",
                table: "UsersDialogsJoins",
                column: "JoinedDateUTC");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_OutDateUTC",
                table: "UsersDialogsJoins",
                column: "OutDateUTC");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_UserIdentityId",
                table: "UsersDialogsJoins",
                column: "UserIdentityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersDialogsJoins_JoinedDateUTC",
                table: "UsersDialogsJoins");

            migrationBuilder.DropIndex(
                name: "IX_UsersDialogsJoins_OutDateUTC",
                table: "UsersDialogsJoins");

            migrationBuilder.DropIndex(
                name: "IX_UsersDialogsJoins_UserIdentityId",
                table: "UsersDialogsJoins");

            migrationBuilder.DropColumn(
                name: "JoinedDateUTC",
                table: "UsersDialogsJoins");

            migrationBuilder.DropColumn(
                name: "OutDateUTC",
                table: "UsersDialogsJoins");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_UserIdentityId_DialogJoinId",
                table: "UsersDialogsJoins",
                columns: new[] { "UserIdentityId", "DialogJoinId" },
                unique: true);
        }
    }
}
