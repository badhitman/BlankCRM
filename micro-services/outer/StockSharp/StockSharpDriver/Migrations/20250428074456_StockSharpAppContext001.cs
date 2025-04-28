using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockSharpDriver.Migrations
{
    public partial class StockSharpAppContext001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExchangeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    IdRemote = table.Column<string>(type: "TEXT", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    ShortName = table.Column<string>(type: "TEXT", nullable: true),
                    TypeInstrument = table.Column<int>(type: "INTEGER", nullable: true),
                    Currency = table.Column<int>(type: "INTEGER", nullable: true),
                    Class = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier = table.Column<decimal>(type: "TEXT", nullable: true),
                    Decimals = table.Column<int>(type: "INTEGER", nullable: true),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    SettlementDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CfiCode = table.Column<string>(type: "TEXT", nullable: true),
                    FaceValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    SettlementType = table.Column<int>(type: "INTEGER", nullable: true),
                    OptionStyle = table.Column<int>(type: "INTEGER", nullable: true),
                    PrimaryId = table.Column<string>(type: "TEXT", nullable: true),
                    UnderlyingSecurityId = table.Column<string>(type: "TEXT", nullable: true),
                    OptionType = table.Column<int>(type: "INTEGER", nullable: true),
                    Shortable = table.Column<bool>(type: "INTEGER", nullable: true),
                    UnderlyingSecurityType = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruments_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<int>(type: "INTEGER", nullable: true),
                    Currency = table.Column<int>(type: "INTEGER", nullable: true),
                    ClientCode = table.Column<string>(type: "TEXT", nullable: true),
                    DepoName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IdPK = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LatencyRegistration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    LatencyCancellation = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    LatencyEdition = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Id = table.Column<long>(type: "INTEGER", nullable: true),
                    StringId = table.Column<string>(type: "TEXT", nullable: true),
                    BoardId = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<long>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    CancelledTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    MatchedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LocalTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Volume = table.Column<decimal>(type: "TEXT", nullable: false),
                    VisibleVolume = table.Column<decimal>(type: "TEXT", nullable: true),
                    Side = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<long>(type: "INTEGER", nullable: true),
                    IsSystem = table.Column<bool>(type: "INTEGER", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    TimeInForce = table.Column<int>(type: "INTEGER", nullable: true),
                    Commission = table.Column<decimal>(type: "TEXT", nullable: true),
                    CommissionCurrency = table.Column<string>(type: "TEXT", nullable: true),
                    UserOrderId = table.Column<string>(type: "TEXT", nullable: true),
                    BrokerCode = table.Column<string>(type: "TEXT", nullable: true),
                    ClientCode = table.Column<string>(type: "TEXT", nullable: true),
                    Currency = table.Column<int>(type: "INTEGER", nullable: true),
                    IsMarketMaker = table.Column<bool>(type: "INTEGER", nullable: true),
                    MarginMode = table.Column<int>(type: "INTEGER", nullable: true),
                    Slippage = table.Column<decimal>(type: "TEXT", nullable: true),
                    IsManual = table.Column<bool>(type: "INTEGER", nullable: true),
                    AveragePrice = table.Column<decimal>(type: "TEXT", nullable: true),
                    Yield = table.Column<decimal>(type: "TEXT", nullable: true),
                    MinVolume = table.Column<decimal>(type: "TEXT", nullable: true),
                    PositionEffect = table.Column<int>(type: "INTEGER", nullable: true),
                    PostOnly = table.Column<bool>(type: "INTEGER", nullable: true),
                    SeqNum = table.Column<long>(type: "INTEGER", nullable: false),
                    Leverage = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdPK);
                    table.ForeignKey(
                        name: "FK_Orders_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_Code",
                table: "Boards",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_ExchangeId",
                table: "Boards",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_Name",
                table: "Exchanges",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_BoardId",
                table: "Instruments",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_CfiCode",
                table: "Instruments",
                column: "CfiCode");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_Class",
                table: "Instruments",
                column: "Class");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_Code",
                table: "Instruments",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_IdRemote",
                table: "Instruments",
                column: "IdRemote");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_IsFavorite",
                table: "Instruments",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_LastUpdatedAtUTC",
                table: "Instruments",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_PrimaryId",
                table: "Instruments",
                column: "PrimaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_UnderlyingSecurityId",
                table: "Instruments",
                column: "UnderlyingSecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BoardId",
                table: "Orders",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BrokerCode",
                table: "Orders",
                column: "BrokerCode");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InstrumentId",
                table: "Orders",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LastUpdatedAtUTC",
                table: "Orders",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PortfolioId",
                table: "Orders",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StringId",
                table: "Orders",
                column: "StringId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TransactionId",
                table: "Orders",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_BoardId",
                table: "Portfolios",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_IsFavorite",
                table: "Portfolios",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_LastUpdatedAtUTC",
                table: "Portfolios",
                column: "LastUpdatedAtUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Exchanges");
        }
    }
}
