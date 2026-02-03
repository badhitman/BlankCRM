using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastReadAtUTC",
                table: "Dialogs",
                newName: "LastOnlineAtUTC");

            migrationBuilder.RenameIndex(
                name: "IX_Dialogs_LastReadAtUTC",
                table: "Dialogs",
                newName: "IX_Dialogs_LastOnlineAtUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastOnlineAtUTC",
                table: "Dialogs",
                newName: "LastReadAtUTC");

            migrationBuilder.RenameIndex(
                name: "IX_Dialogs_LastOnlineAtUTC",
                table: "Dialogs",
                newName: "IX_Dialogs_LastReadAtUTC");
        }
    }
}
