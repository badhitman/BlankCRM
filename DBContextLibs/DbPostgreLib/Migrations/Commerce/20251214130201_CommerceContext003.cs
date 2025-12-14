using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ConversionsDocumentsWalletsRetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "ConversionsDocumentsWalletsRetail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_IsDisabled",
                table: "ConversionsDocumentsWalletsRetail",
                column: "IsDisabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_IsDisabled",
                table: "ConversionsDocumentsWalletsRetail");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ConversionsDocumentsWalletsRetail");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "ConversionsDocumentsWalletsRetail");
        }
    }
}
