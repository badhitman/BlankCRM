using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RowsOrdersRetails_OrderId",
                table: "RowsOrdersRetails");

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrdersRetails_OrderId_OfferId",
                table: "RowsOrdersRetails",
                columns: new[] { "OrderId", "OfferId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RowsOrdersRetails_OrderId_OfferId",
                table: "RowsOrdersRetails");

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrdersRetails_OrderId",
                table: "RowsOrdersRetails",
                column: "OrderId");
        }
    }
}
