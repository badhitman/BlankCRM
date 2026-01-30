using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypePaymentId",
                table: "PaymentsRetailDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTypeId",
                table: "DeliveryDocumentsRetail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_TypePaymentId",
                table: "PaymentsRetailDocuments",
                column: "TypePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentsRetail_DeliveryTypeId",
                table: "DeliveryDocumentsRetail",
                column: "DeliveryTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentsRetailDocuments_TypePaymentId",
                table: "PaymentsRetailDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryDocumentsRetail_DeliveryTypeId",
                table: "DeliveryDocumentsRetail");

            migrationBuilder.DropColumn(
                name: "TypePaymentId",
                table: "PaymentsRetailDocuments");

            migrationBuilder.DropColumn(
                name: "DeliveryTypeId",
                table: "DeliveryDocumentsRetail");
        }
    }
}
