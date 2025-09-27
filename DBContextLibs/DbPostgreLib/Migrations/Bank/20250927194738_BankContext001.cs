using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Bank
{
    /// <inheritdoc />
    public partial class BankContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectionsBanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BankInterface = table.Column<int>(type: "integer", nullable: false),
                    LastChecked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionsBanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomersBanksIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserIdentityId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Inn = table.Column<string>(type: "text", nullable: true),
                    BankIdentifyType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersBanksIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomingMerchantsPaymentsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpDate = table.Column<string>(type: "text", nullable: true),
                    Pan = table.Column<string>(type: "text", nullable: true),
                    CardId = table.Column<string>(type: "text", nullable: true),
                    RebillId = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    PaymentId = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<string>(type: "text", nullable: true),
                    OrderJoinId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingMerchantsPaymentsTBank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsForReceiptsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cash = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Electronic = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AdvancePayment = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Credit = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Provision = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsForReceiptsTBank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrForInitPaymentTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeQR = table.Column<int>(type: "integer", nullable: false),
                    DataQR = table.Column<string>(type: "text", nullable: true),
                    TerminalKey = table.Column<string>(type: "text", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorCode = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    ApiException = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrForInitPaymentTBank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankConnectionId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TariffName = table.Column<string>(type: "text", nullable: false),
                    TariffCode = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MainFlag = table.Column<string>(type: "text", nullable: false),
                    BankBik = table.Column<string>(type: "text", nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsTBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsTBank_ConnectionsBanks_BankConnectionId",
                        column: x => x.BankConnectionId,
                        principalTable: "ConnectionsBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransfersBanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerBankId = table.Column<int>(type: "integer", nullable: true),
                    BankConnectionId = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Sender = table.Column<string>(type: "text", nullable: false),
                    Receiver = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransfersBanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransfersBanks_ConnectionsBanks_BankConnectionId",
                        column: x => x.BankConnectionId,
                        principalTable: "ConnectionsBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransfersBanks_CustomersBanksIds_CustomerBankId",
                        column: x => x.CustomerBankId,
                        principalTable: "CustomersBanksIds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReceiptsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentsId = table.Column<int>(type: "integer", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Taxation = table.Column<int>(type: "integer", nullable: false),
                    EmailCompany = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptsTBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptsTBank_PaymentsForReceiptsTBank_PaymentsId",
                        column: x => x.PaymentsId,
                        principalTable: "PaymentsForReceiptsTBank",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentsInitResultsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiptId = table.Column<int>(type: "integer", nullable: false),
                    QRPaymentId = table.Column<int>(type: "integer", nullable: true),
                    OrderJoinId = table.Column<int>(type: "integer", nullable: true),
                    InitiatorUserId = table.Column<string>(type: "text", nullable: false),
                    PayerUserId = table.Column<string>(type: "text", nullable: false),
                    TerminalKey = table.Column<string>(type: "text", nullable: true),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorCode = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    ApiException = table.Column<string>(type: "text", nullable: true),
                    CreatedDateTimeUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    PaymentId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    PaymentURL = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsInitResultsTBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsInitResultsTBank_QrForInitPaymentTBank_QRPaymentId",
                        column: x => x.QRPaymentId,
                        principalTable: "QrForInitPaymentTBank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentsInitResultsTBank_ReceiptsTBank_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "ReceiptsTBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptsItemsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiptId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Tax = table.Column<int>(type: "integer", nullable: false),
                    Ean13 = table.Column<string>(type: "text", nullable: true),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: true),
                    PaymentObject = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptsItemsTBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptsItemsTBank_ReceiptsTBank_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "ReceiptsTBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgentsForReceiptItemsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiptItemId = table.Column<int>(type: "integer", nullable: false),
                    PhonesJsonSource = table.Column<string>(type: "text", nullable: true),
                    ReceiverPhonesJsonSource = table.Column<string>(type: "text", nullable: true),
                    TransferPhonesJsonSource = table.Column<string>(type: "text", nullable: true),
                    AgentSign = table.Column<int>(type: "integer", nullable: true),
                    OperationName = table.Column<string>(type: "text", nullable: true),
                    Phones = table.Column<string[]>(type: "text[]", nullable: true),
                    ReceiverPhones = table.Column<string[]>(type: "text[]", nullable: true),
                    TransferPhones = table.Column<List<string>>(type: "text[]", nullable: true),
                    OperatorName = table.Column<string>(type: "text", nullable: true),
                    OperatorAddress = table.Column<string>(type: "text", nullable: true),
                    OperatorInn = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentsForReceiptItemsTBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentsForReceiptItemsTBank_ReceiptsItemsTBank_ReceiptItemId",
                        column: x => x.ReceiptItemId,
                        principalTable: "ReceiptsItemsTBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuppliersForReceiptItemsTBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiptItemId = table.Column<int>(type: "integer", nullable: false),
                    PhonesJsonSource = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Inn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliersForReceiptItemsTBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuppliersForReceiptItemsTBank_ReceiptsItemsTBank_ReceiptIte~",
                        column: x => x.ReceiptItemId,
                        principalTable: "ReceiptsItemsTBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_AccountNumber",
                table: "AccountsTBank",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_AccountType",
                table: "AccountsTBank",
                column: "AccountType");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_ActivationDate",
                table: "AccountsTBank",
                column: "ActivationDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_BankBik",
                table: "AccountsTBank",
                column: "BankBik");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_BankConnectionId",
                table: "AccountsTBank",
                column: "BankConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_CreatedOn",
                table: "AccountsTBank",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_Currency",
                table: "AccountsTBank",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_IsActive",
                table: "AccountsTBank",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_MainFlag",
                table: "AccountsTBank",
                column: "MainFlag");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_Name",
                table: "AccountsTBank",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_Status",
                table: "AccountsTBank",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTBank_TariffCode",
                table: "AccountsTBank",
                column: "TariffCode");

            migrationBuilder.CreateIndex(
                name: "IX_AgentsForReceiptItemsTBank_ReceiptItemId",
                table: "AgentsForReceiptItemsTBank",
                column: "ReceiptItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionsBanks_BankInterface",
                table: "ConnectionsBanks",
                column: "BankInterface");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionsBanks_LastChecked",
                table: "ConnectionsBanks",
                column: "LastChecked");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersBanksIds_BankIdentifyType",
                table: "CustomersBanksIds",
                column: "BankIdentifyType");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersBanksIds_Inn",
                table: "CustomersBanksIds",
                column: "Inn");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersBanksIds_Name",
                table: "CustomersBanksIds",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_Amount",
                table: "IncomingMerchantsPaymentsTBank",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_CardId",
                table: "IncomingMerchantsPaymentsTBank",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_ExpDate",
                table: "IncomingMerchantsPaymentsTBank",
                column: "ExpDate");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_OrderId",
                table: "IncomingMerchantsPaymentsTBank",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_OrderJoinId",
                table: "IncomingMerchantsPaymentsTBank",
                column: "OrderJoinId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_PaymentId",
                table: "IncomingMerchantsPaymentsTBank",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_RebillId",
                table: "IncomingMerchantsPaymentsTBank",
                column: "RebillId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingMerchantsPaymentsTBank_Status",
                table: "IncomingMerchantsPaymentsTBank",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_Amount",
                table: "PaymentsInitResultsTBank",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_CreatedDateTimeUTC",
                table: "PaymentsInitResultsTBank",
                column: "CreatedDateTimeUTC");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_ErrorCode",
                table: "PaymentsInitResultsTBank",
                column: "ErrorCode");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_InitiatorUserId",
                table: "PaymentsInitResultsTBank",
                column: "InitiatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_OrderId",
                table: "PaymentsInitResultsTBank",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_OrderJoinId",
                table: "PaymentsInitResultsTBank",
                column: "OrderJoinId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_PaymentId",
                table: "PaymentsInitResultsTBank",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_QRPaymentId",
                table: "PaymentsInitResultsTBank",
                column: "QRPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_ReceiptId",
                table: "PaymentsInitResultsTBank",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_Status",
                table: "PaymentsInitResultsTBank",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_Success",
                table: "PaymentsInitResultsTBank",
                column: "Success");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsInitResultsTBank_TerminalKey",
                table: "PaymentsInitResultsTBank",
                column: "TerminalKey");

            migrationBuilder.CreateIndex(
                name: "IX_QrForInitPaymentTBank_ErrorCode",
                table: "QrForInitPaymentTBank",
                column: "ErrorCode");

            migrationBuilder.CreateIndex(
                name: "IX_QrForInitPaymentTBank_Success",
                table: "QrForInitPaymentTBank",
                column: "Success");

            migrationBuilder.CreateIndex(
                name: "IX_QrForInitPaymentTBank_TerminalKey",
                table: "QrForInitPaymentTBank",
                column: "TerminalKey");

            migrationBuilder.CreateIndex(
                name: "IX_QrForInitPaymentTBank_TypeQR",
                table: "QrForInitPaymentTBank",
                column: "TypeQR");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptsItemsTBank_ReceiptId",
                table: "ReceiptsItemsTBank",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptsTBank_PaymentsId",
                table: "ReceiptsTBank",
                column: "PaymentsId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliersForReceiptItemsTBank_ReceiptItemId",
                table: "SuppliersForReceiptItemsTBank",
                column: "ReceiptItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_Amount",
                table: "TransfersBanks",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_BankConnectionId",
                table: "TransfersBanks",
                column: "BankConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_Currency",
                table: "TransfersBanks",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_CustomerBankId",
                table: "TransfersBanks",
                column: "CustomerBankId");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_Receiver",
                table: "TransfersBanks",
                column: "Receiver");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_Sender",
                table: "TransfersBanks",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_Timestamp",
                table: "TransfersBanks",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TransfersBanks_TransactionId",
                table: "TransfersBanks",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsTBank");

            migrationBuilder.DropTable(
                name: "AgentsForReceiptItemsTBank");

            migrationBuilder.DropTable(
                name: "IncomingMerchantsPaymentsTBank");

            migrationBuilder.DropTable(
                name: "PaymentsInitResultsTBank");

            migrationBuilder.DropTable(
                name: "SuppliersForReceiptItemsTBank");

            migrationBuilder.DropTable(
                name: "TransfersBanks");

            migrationBuilder.DropTable(
                name: "QrForInitPaymentTBank");

            migrationBuilder.DropTable(
                name: "ReceiptsItemsTBank");

            migrationBuilder.DropTable(
                name: "ConnectionsBanks");

            migrationBuilder.DropTable(
                name: "CustomersBanksIds");

            migrationBuilder.DropTable(
                name: "ReceiptsTBank");

            migrationBuilder.DropTable(
                name: "PaymentsForReceiptsTBank");
        }
    }
}
