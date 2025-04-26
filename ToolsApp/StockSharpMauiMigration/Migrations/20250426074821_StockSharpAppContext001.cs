using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockSharpMauiMigration.Migrations
{
    /// <inheritdoc />
    public partial class StockSharpAppContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardsExchange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardsExchange", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExchangeBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    IdRemote = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", nullable: false),
                    Class = table.Column<string>(type: "TEXT", nullable: false),
                    CfiCode = table.Column<string>(type: "TEXT", nullable: false),
                    PrimaryId = table.Column<string>(type: "TEXT", nullable: false),
                    UnderlyingSecurityId = table.Column<string>(type: "TEXT", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false),
                    TypeInstrument = table.Column<int>(type: "INTEGER", nullable: true),
                    Currency = table.Column<int>(type: "INTEGER", nullable: true),
                    Multiplier = table.Column<decimal>(type: "TEXT", nullable: true),
                    Decimals = table.Column<int>(type: "INTEGER", nullable: true),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    SettlementDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    FaceValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    SettlementType = table.Column<int>(type: "INTEGER", nullable: true),
                    OptionStyle = table.Column<int>(type: "INTEGER", nullable: true),
                    OptionType = table.Column<int>(type: "INTEGER", nullable: true),
                    Shortable = table.Column<bool>(type: "INTEGER", nullable: true),
                    UnderlyingSecurityType = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruments_BoardsExchange_ExchangeBoardId",
                        column: x => x.ExchangeBoardId,
                        principalTable: "BoardsExchange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentsExternalsIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ParentInstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Sedol = table.Column<string>(type: "TEXT", nullable: false),
                    Cusip = table.Column<string>(type: "TEXT", nullable: false),
                    Isin = table.Column<string>(type: "TEXT", nullable: false),
                    Ric = table.Column<string>(type: "TEXT", nullable: false),
                    Bloomberg = table.Column<string>(type: "TEXT", nullable: false),
                    IQFeed = table.Column<string>(type: "TEXT", nullable: false),
                    InteractiveBrokers = table.Column<int>(type: "INTEGER", nullable: true),
                    Plaza = table.Column<string>(type: "TEXT", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentsExternalsIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstrumentsExternalsIds_Instruments_ParentInstrumentId",
                        column: x => x.ParentInstrumentId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardsExchange_LastAtUpdatedUTC",
                table: "BoardsExchange",
                column: "LastAtUpdatedUTC");

            migrationBuilder.CreateIndex(
                name: "IX_BoardsExchange_Name",
                table: "BoardsExchange",
                column: "Name");

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

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_Bloomberg",
                table: "InstrumentsExternalsIds",
                column: "Bloomberg");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_Cusip",
                table: "InstrumentsExternalsIds",
                column: "Cusip");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_InteractiveBrokers",
                table: "InstrumentsExternalsIds",
                column: "InteractiveBrokers");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_IQFeed",
                table: "InstrumentsExternalsIds",
                column: "IQFeed");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_Isin",
                table: "InstrumentsExternalsIds",
                column: "Isin");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_LastAtUpdatedUTC",
                table: "InstrumentsExternalsIds",
                column: "LastAtUpdatedUTC");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_ParentInstrumentId",
                table: "InstrumentsExternalsIds",
                column: "ParentInstrumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_Plaza",
                table: "InstrumentsExternalsIds",
                column: "Plaza");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_Ric",
                table: "InstrumentsExternalsIds",
                column: "Ric");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_Sedol",
                table: "InstrumentsExternalsIds",
                column: "Sedol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstrumentsExternalsIds");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "BoardsExchange");
        }
    }
}
