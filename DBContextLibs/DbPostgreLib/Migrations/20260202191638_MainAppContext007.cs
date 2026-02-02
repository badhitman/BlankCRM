using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitiatorContactsNormalized",
                table: "Dialogs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_InitiatorContactsNormalized",
                table: "Dialogs",
                column: "InitiatorContactsNormalized");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dialogs_InitiatorContactsNormalized",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "InitiatorContactsNormalized",
                table: "Dialogs");
        }
    }
}
