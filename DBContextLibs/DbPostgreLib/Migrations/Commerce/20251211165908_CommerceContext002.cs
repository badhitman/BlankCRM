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
                name: "IX_DeliveriesOrdersLinks_OrderDocumentId",
                table: "DeliveriesOrdersLinks");

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "DeliveryRetailDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_WarehouseId",
                table: "DeliveryRetailDocuments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveriesOrdersLinks_OrderDocumentId_DeliveryDocumentId",
                table: "DeliveriesOrdersLinks",
                columns: new[] { "OrderDocumentId", "DeliveryDocumentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryRetailDocuments_WarehouseId",
                table: "DeliveryRetailDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DeliveriesOrdersLinks_OrderDocumentId_DeliveryDocumentId",
                table: "DeliveriesOrdersLinks");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "DeliveryRetailDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveriesOrdersLinks_OrderDocumentId",
                table: "DeliveriesOrdersLinks",
                column: "OrderDocumentId");
        }
    }
}
