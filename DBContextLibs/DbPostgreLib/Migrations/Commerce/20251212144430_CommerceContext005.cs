using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryRetailServices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryRetailServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SortIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryRetailServices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailServices_CreatedAtUTC",
                table: "DeliveryRetailServices",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailServices_IsDisabled",
                table: "DeliveryRetailServices",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailServices_LastUpdatedAtUTC",
                table: "DeliveryRetailServices",
                column: "LastUpdatedAtUTC");
        }
    }
}
