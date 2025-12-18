using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext005_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DisabledPaymentsTypesForWallets_PaymentType_WalletTypeId",
                table: "DisabledPaymentsTypesForWallets",
                columns: new[] { "PaymentType", "WalletTypeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DisabledPaymentsTypesForWallets_PaymentType_WalletTypeId",
                table: "DisabledPaymentsTypesForWallets");
        }
    }
}
