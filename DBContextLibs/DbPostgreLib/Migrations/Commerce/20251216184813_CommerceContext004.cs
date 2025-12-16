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
            migrationBuilder.AddColumn<decimal>(
                name: "WeightOffer",
                table: "RowsWarehouses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightOffer",
                table: "RowsOrdersRetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightOffer",
                table: "RowsOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightOffer",
                table: "RowsDeliveryDocumentsRetail",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightOffer",
                table: "OffersAvailability",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Offers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightOffer",
                table: "RowsWarehouses");

            migrationBuilder.DropColumn(
                name: "WeightOffer",
                table: "RowsOrdersRetails");

            migrationBuilder.DropColumn(
                name: "WeightOffer",
                table: "RowsOrders");

            migrationBuilder.DropColumn(
                name: "WeightOffer",
                table: "RowsDeliveryDocumentsRetail");

            migrationBuilder.DropColumn(
                name: "WeightOffer",
                table: "OffersAvailability");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Offers");
        }
    }
}
