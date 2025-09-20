using System;
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
                    Token = table.Column<string>(type: "text", nullable: true),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Inn = table.Column<string>(type: "text", nullable: true),
                    BankIdentifyType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersBanksIds", x => x.Id);
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
                    CustomerBankId1 = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_TransfersBanks_CustomersBanksIds_CustomerBankId1",
                        column: x => x.CustomerBankId1,
                        principalTable: "CustomersBanksIds",
                        principalColumn: "Id");
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
                name: "IX_TransfersBanks_CustomerBankId1",
                table: "TransfersBanks",
                column: "CustomerBankId1");

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
                name: "TransfersBanks");

            migrationBuilder.DropTable(
                name: "ConnectionsBanks");

            migrationBuilder.DropTable(
                name: "CustomersBanksIds");
        }
    }
}
