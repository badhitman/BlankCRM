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
                name: "Goods",
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
                    table.PrimaryKey("PK_Goods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Articul",
                schema: "public",
                table: "Goods",
                column: "Articul");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_CodeNC",
                schema: "public",
                table: "Goods",
                column: "CodeNC");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_CurrencyBasePrice",
                schema: "public",
                table: "Goods",
                column: "CurrencyBasePrice");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_CurrencyRIC",
                schema: "public",
                table: "Goods",
                column: "CurrencyRIC");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_LoadedDateTime",
                schema: "public",
                table: "Goods",
                column: "LoadedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Quantity",
                schema: "public",
                table: "Goods",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Stock",
                schema: "public",
                table: "Goods",
                column: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_TimeLastUpdate",
                schema: "public",
                table: "Goods",
                column: "TimeLastUpdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goods",
                schema: "public");
        }
    }
}
