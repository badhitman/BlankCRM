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
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
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
                    IdRemote = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", nullable: false),
                    ExchangeBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeInstrument = table.Column<int>(type: "INTEGER", nullable: true),
                    Currency = table.Column<int>(type: "INTEGER", nullable: true),
                    Class = table.Column<string>(type: "TEXT", nullable: false),
                    PriceStep = table.Column<decimal>(type: "TEXT", nullable: true),
                    VolumeStep = table.Column<decimal>(type: "TEXT", nullable: true),
                    MinVolume = table.Column<decimal>(type: "TEXT", nullable: true),
                    MaxVolume = table.Column<decimal>(type: "TEXT", nullable: true),
                    Multiplier = table.Column<decimal>(type: "TEXT", nullable: true),
                    Decimals = table.Column<int>(type: "INTEGER", nullable: true),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    SettlementDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CfiCode = table.Column<string>(type: "TEXT", nullable: false),
                    FaceValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    SettlementType = table.Column<int>(type: "INTEGER", nullable: true),
                    OptionStyle = table.Column<int>(type: "INTEGER", nullable: true),
                    PrimaryId = table.Column<string>(type: "TEXT", nullable: false),
                    UnderlyingSecurityId = table.Column<string>(type: "TEXT", nullable: false),
                    OptionType = table.Column<int>(type: "INTEGER", nullable: true),
                    Shortable = table.Column<bool>(type: "INTEGER", nullable: true),
                    UnderlyingSecurityType = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
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
                    ParentInstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Sedol = table.Column<string>(type: "TEXT", nullable: false),
                    Cusip = table.Column<string>(type: "TEXT", nullable: false),
                    Isin = table.Column<string>(type: "TEXT", nullable: false),
                    Ric = table.Column<string>(type: "TEXT", nullable: false),
                    Bloomberg = table.Column<string>(type: "TEXT", nullable: false),
                    IQFeed = table.Column<string>(type: "TEXT", nullable: false),
                    InteractiveBrokers = table.Column<int>(type: "INTEGER", nullable: true),
                    Plaza = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "IX_BoardsExchange_Name",
                table: "BoardsExchange",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_ExchangeBoardId",
                table: "Instruments",
                column: "ExchangeBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_Name",
                table: "Instruments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsExternalsIds_ParentInstrumentId",
                table: "InstrumentsExternalsIds",
                column: "ParentInstrumentId",
                unique: true);
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
