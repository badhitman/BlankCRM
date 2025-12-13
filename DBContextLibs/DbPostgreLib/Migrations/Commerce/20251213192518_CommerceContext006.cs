using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StatusDocument",
                table: "RetailOrders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StatusDocument",
                table: "OrdersB2B",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StatusDocument",
                table: "AttendancesReg",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "OrdersStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateOperation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusDocument = table.Column<int>(type: "integer", nullable: false),
                    OrderDocumentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersStatuses_RetailOrders_OrderDocumentId",
                        column: x => x.OrderDocumentId,
                        principalTable: "RetailOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsOrdersLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmountPayment = table.Column<decimal>(type: "numeric", nullable: false),
                    OrderDocumentId = table.Column<int>(type: "integer", nullable: false),
                    PaymentDocumentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsOrdersLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsOrdersLinks_PaymentsRetailDocuments_PaymentDocument~",
                        column: x => x.PaymentDocumentId,
                        principalTable: "PaymentsRetailDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentsOrdersLinks_RetailOrders_OrderDocumentId",
                        column: x => x.OrderDocumentId,
                        principalTable: "RetailOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersStatuses_CreatedAtUTC",
                table: "OrdersStatuses",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersStatuses_DateOperation",
                table: "OrdersStatuses",
                column: "DateOperation");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersStatuses_LastUpdatedAtUTC",
                table: "OrdersStatuses",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersStatuses_Name",
                table: "OrdersStatuses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersStatuses_OrderDocumentId",
                table: "OrdersStatuses",
                column: "OrderDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersStatuses_StatusDocument",
                table: "OrdersStatuses",
                column: "StatusDocument");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsOrdersLinks_OrderDocumentId_PaymentDocumentId",
                table: "PaymentsOrdersLinks",
                columns: new[] { "OrderDocumentId", "PaymentDocumentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsOrdersLinks_PaymentDocumentId",
                table: "PaymentsOrdersLinks",
                column: "PaymentDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersStatuses");

            migrationBuilder.DropTable(
                name: "PaymentsOrdersLinks");

            migrationBuilder.AlterColumn<int>(
                name: "StatusDocument",
                table: "RetailOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusDocument",
                table: "OrdersB2B",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusDocument",
                table: "AttendancesReg",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
