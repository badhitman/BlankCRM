using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Realtime
{
    /// <inheritdoc />
    public partial class RealtimeContext003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HelpdeskId",
                table: "Dialogs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_HelpdeskId",
                table: "Dialogs",
                column: "HelpdeskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dialogs_HelpdeskId",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "HelpdeskId",
                table: "Dialogs");
        }
    }
}
