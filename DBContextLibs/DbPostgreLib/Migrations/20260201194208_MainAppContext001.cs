using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dialogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastMessageAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastReadAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeadlineUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    InitiatorContacts = table.Column<string>(type: "text", nullable: true),
                    InitiatorHumanName = table.Column<string>(type: "text", nullable: true),
                    InitiatorIdentityId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_DeadlineUTC",
                table: "Dialogs",
                column: "DeadlineUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_InitiatorContacts",
                table: "Dialogs",
                column: "InitiatorContacts");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_InitiatorIdentityId",
                table: "Dialogs",
                column: "InitiatorIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_IsDisabled",
                table: "Dialogs",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_LastMessageAtUTC",
                table: "Dialogs",
                column: "LastMessageAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_LastReadAtUTC",
                table: "Dialogs",
                column: "LastReadAtUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dialogs");
        }
    }
}
