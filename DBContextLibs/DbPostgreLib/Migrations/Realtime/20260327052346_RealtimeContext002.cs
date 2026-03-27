using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Realtime
{
    /// <inheritdoc />
    public partial class RealtimeContext002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirebaseCloudMessagingToken",
                table: "Dialogs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_FirebaseCloudMessagingToken",
                table: "Dialogs",
                column: "FirebaseCloudMessagingToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dialogs_FirebaseCloudMessagingToken",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "FirebaseCloudMessagingToken",
                table: "Dialogs");
        }
    }
}
