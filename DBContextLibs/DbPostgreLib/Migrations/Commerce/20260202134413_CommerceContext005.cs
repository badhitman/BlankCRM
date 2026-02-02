using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentsRetailDocuments_TypePayment",
                table: "PaymentsRetailDocuments");

            migrationBuilder.DropColumn(
                name: "TypePayment",
                table: "PaymentsRetailDocuments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypePayment",
                table: "PaymentsRetailDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_TypePayment",
                table: "PaymentsRetailDocuments",
                column: "TypePayment");
        }
    }
}
