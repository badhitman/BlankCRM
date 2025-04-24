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
                name: "ExchangeBoards",
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
                    table.PrimaryKey("PK_ExchangeBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdRemote = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ExchangeBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruments_ExchangeBoards_ExchangeBoardId",
                        column: x => x.ExchangeBoardId,
                        principalTable: "ExchangeBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeBoards_Name",
                table: "ExchangeBoards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_ExchangeBoardId",
                table: "Instruments",
                column: "ExchangeBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_Name",
                table: "Instruments",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "ExchangeBoards");
        }
    }
}
