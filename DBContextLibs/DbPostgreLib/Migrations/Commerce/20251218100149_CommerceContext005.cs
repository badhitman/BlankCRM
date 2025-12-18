using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisabledPaymentsTypesForWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    WalletTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisabledPaymentsTypesForWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisabledPaymentsTypesForWallets_WalletsRetailTypes_WalletTy~",
                        column: x => x.WalletTypeId,
                        principalTable: "WalletsRetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisabledPaymentsTypesForWallets_WalletTypeId",
                table: "DisabledPaymentsTypesForWallets",
                column: "WalletTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisabledPaymentsTypesForWallets");
        }
    }
}
