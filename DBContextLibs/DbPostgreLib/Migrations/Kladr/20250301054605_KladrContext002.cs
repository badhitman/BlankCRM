using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Kladr
{
    /// <inheritdoc />
    public partial class KladrContext002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HouseKLADRModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KORP = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CODE = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseKLADRModelDB", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_CODE",
                table: "HouseKLADRModelDB",
                column: "CODE");

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_GNINMB",
                table: "HouseKLADRModelDB",
                column: "GNINMB");

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_INDEX",
                table: "HouseKLADRModelDB",
                column: "INDEX");

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_NAME",
                table: "HouseKLADRModelDB",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_OCATD",
                table: "HouseKLADRModelDB",
                column: "OCATD");

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_SOCR",
                table: "HouseKLADRModelDB",
                column: "SOCR");

            migrationBuilder.CreateIndex(
                name: "IX_HouseKLADRModelDB_UNO",
                table: "HouseKLADRModelDB",
                column: "UNO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseKLADRModelDB");
        }
    }
}
