using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AttachesFilesOfMessages_MessageOwnerId",
                table: "AttachesFilesOfMessages");

            migrationBuilder.CreateIndex(
                name: "IX_AttachesFilesOfMessages_MessageOwnerId",
                table: "AttachesFilesOfMessages",
                column: "MessageOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AttachesFilesOfMessages_MessageOwnerId",
                table: "AttachesFilesOfMessages");

            migrationBuilder.CreateIndex(
                name: "IX_AttachesFilesOfMessages_MessageOwnerId",
                table: "AttachesFilesOfMessages",
                column: "MessageOwnerId",
                unique: true);
        }
    }
}
