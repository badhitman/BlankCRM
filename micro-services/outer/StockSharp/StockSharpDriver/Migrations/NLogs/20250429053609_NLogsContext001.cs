using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockSharpDriver.Migrations.NLogs
{
    public partial class NLogsContext001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationName = table.Column<string>(type: "TEXT", nullable: true),
                    ContextPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    RecordTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RecordLevel = table.Column<string>(type: "TEXT", nullable: true),
                    RecordMessage = table.Column<string>(type: "TEXT", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "TEXT", nullable: true),
                    Logger = table.Column<string>(type: "TEXT", nullable: true),
                    CallSite = table.Column<string>(type: "TEXT", nullable: true),
                    StackTrace = table.Column<string>(type: "TEXT", nullable: true),
                    AllEventProperties = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ApplicationName",
                table: "Logs",
                column: "ApplicationName");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ContextPrefix",
                table: "Logs",
                column: "ContextPrefix");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Logger",
                table: "Logs",
                column: "Logger");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RecordLevel",
                table: "Logs",
                column: "RecordLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RecordTime",
                table: "Logs",
                column: "RecordTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
