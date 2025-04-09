using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.ApiBreezRu
{
    /// <inheritdoc />
    public partial class ApiBreezRuContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Leftovers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasePrice = table.Column<string>(type: "text", nullable: true),
                    CurrencyBasePrice = table.Column<string>(type: "text", nullable: true),
                    RIC = table.Column<string>(type: "text", nullable: true),
                    CurrencyRIC = table.Column<string>(type: "text", nullable: true),
                    LoadedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CodeNC = table.Column<string>(type: "text", nullable: true),
                    Articul = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    Stock = table.Column<string>(type: "text", nullable: true),
                    TimeLastUpdate = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leftovers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_Articul",
                schema: "public",
                table: "Leftovers",
                column: "Articul");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CodeNC",
                schema: "public",
                table: "Leftovers",
                column: "CodeNC");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CurrencyBasePrice",
                schema: "public",
                table: "Leftovers",
                column: "CurrencyBasePrice");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CurrencyRIC",
                schema: "public",
                table: "Leftovers",
                column: "CurrencyRIC");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_LoadedDateTime",
                schema: "public",
                table: "Leftovers",
                column: "LoadedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_Quantity",
                schema: "public",
                table: "Leftovers",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_Stock",
                schema: "public",
                table: "Leftovers",
                column: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_TimeLastUpdate",
                schema: "public",
                table: "Leftovers",
                column: "TimeLastUpdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leftovers",
                schema: "public");
        }
    }
}
