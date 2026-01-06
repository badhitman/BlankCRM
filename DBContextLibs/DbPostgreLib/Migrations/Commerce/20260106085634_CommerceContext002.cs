using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RowsDeliveryDocumentsRetail_DocumentId",
                table: "RowsDeliveryDocumentsRetail");

            migrationBuilder.CreateIndex(
                name: "IX_RowsDeliveryDocumentsRetail_DocumentId_OfferId",
                table: "RowsDeliveryDocumentsRetail",
                columns: new[] { "DocumentId", "OfferId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RowsDeliveryDocumentsRetail_DocumentId_OfferId",
                table: "RowsDeliveryDocumentsRetail");

            migrationBuilder.CreateIndex(
                name: "IX_RowsDeliveryDocumentsRetail_DocumentId",
                table: "RowsDeliveryDocumentsRetail",
                column: "DocumentId");
        }
    }
}
