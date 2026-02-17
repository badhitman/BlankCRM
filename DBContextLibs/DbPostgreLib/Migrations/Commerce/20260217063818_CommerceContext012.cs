using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryDocumentsRetail_DeliveryType",
                table: "DeliveryDocumentsRetail");

            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "DeliveryDocumentsRetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryType",
                table: "DeliveryDocumentsRetail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentsRetail_DeliveryType",
                table: "DeliveryDocumentsRetail",
                column: "DeliveryType");
        }
    }
}
