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
                    CountryCode = table.Column<int>(type: "INTEGER", nullable: false)
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
                    ExchangeId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExchangeBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
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
                        name: "FK_Instruments_Boards_ExchangeBoardId",
                        column: x => x.ExchangeBoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalsIdsInstruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentInstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sedol = table.Column<string>(type: "TEXT", nullable: true),
                    Cusip = table.Column<string>(type: "TEXT", nullable: true),
                    Isin = table.Column<string>(type: "TEXT", nullable: true),
                    Ric = table.Column<string>(type: "TEXT", nullable: true),
                    Bloomberg = table.Column<string>(type: "TEXT", nullable: true),
                    IQFeed = table.Column<string>(type: "TEXT", nullable: true),
                    InteractiveBrokers = table.Column<int>(type: "INTEGER", nullable: true),
                    Plaza = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalsIdsInstruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalsIdsInstruments_Instruments_ParentInstrumentId",
                        column: x => x.ParentInstrumentId,
                        principalTable: "Instruments",
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
                name: "IX_Boards_LastAtUpdatedUTC",
                table: "Boards",
                column: "LastAtUpdatedUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_Name",
                table: "Exchanges",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_Bloomberg",
                table: "ExternalsIdsInstruments",
                column: "Bloomberg");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_Cusip",
                table: "ExternalsIdsInstruments",
                column: "Cusip");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_InteractiveBrokers",
                table: "ExternalsIdsInstruments",
                column: "InteractiveBrokers");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_IQFeed",
                table: "ExternalsIdsInstruments",
                column: "IQFeed");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_Isin",
                table: "ExternalsIdsInstruments",
                column: "Isin");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_LastAtUpdatedUTC",
                table: "ExternalsIdsInstruments",
                column: "LastAtUpdatedUTC");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_ParentInstrumentId",
                table: "ExternalsIdsInstruments",
                column: "ParentInstrumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_Plaza",
                table: "ExternalsIdsInstruments",
                column: "Plaza");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_Ric",
                table: "ExternalsIdsInstruments",
                column: "Ric");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalsIdsInstruments_Sedol",
                table: "ExternalsIdsInstruments",
                column: "Sedol");

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
                name: "IX_Instruments_ExchangeBoardId",
                table: "Instruments",
                column: "ExchangeBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_IdRemote",
                table: "Instruments",
                column: "IdRemote");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_IsFavorite",
                table: "Instruments",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_LastAtUpdatedUTC",
                table: "Instruments",
                column: "LastAtUpdatedUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_PrimaryId",
                table: "Instruments",
                column: "PrimaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_UnderlyingSecurityId",
                table: "Instruments",
                column: "UnderlyingSecurityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalsIdsInstruments");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Exchanges");
        }
    }
}
