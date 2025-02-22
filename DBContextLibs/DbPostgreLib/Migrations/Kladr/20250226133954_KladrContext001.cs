using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Kladr
{
    /// <inheritdoc />
    public partial class KladrContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AltnameKLADRModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OLDCODE = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    NEWCODE = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    LEVEL = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltnameKLADRModelDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NameMapKLADRModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SHNAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SCNAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CODE = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameMapKLADRModelDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectKLADRModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STATUS = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
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
                    table.PrimaryKey("PK_ObjectKLADRModelDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocrbaseKLADRModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LEVEL = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SCNAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SOCRNAME = table.Column<string>(type: "character varying(29)", maxLength: 29, nullable: false),
                    KOD_T_ST = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocrbaseKLADRModelDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreetKLADRModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_StreetKLADRModelDB", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AltnameKLADRModelDB_LEVEL",
                table: "AltnameKLADRModelDB",
                column: "LEVEL");

            migrationBuilder.CreateIndex(
                name: "IX_AltnameKLADRModelDB_NEWCODE",
                table: "AltnameKLADRModelDB",
                column: "NEWCODE");

            migrationBuilder.CreateIndex(
                name: "IX_AltnameKLADRModelDB_OLDCODE",
                table: "AltnameKLADRModelDB",
                column: "OLDCODE");

            migrationBuilder.CreateIndex(
                name: "IX_NameMapKLADRModelDB_CODE",
                table: "NameMapKLADRModelDB",
                column: "CODE");

            migrationBuilder.CreateIndex(
                name: "IX_NameMapKLADRModelDB_NAME",
                table: "NameMapKLADRModelDB",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_NameMapKLADRModelDB_SCNAME",
                table: "NameMapKLADRModelDB",
                column: "SCNAME");

            migrationBuilder.CreateIndex(
                name: "IX_NameMapKLADRModelDB_SHNAME",
                table: "NameMapKLADRModelDB",
                column: "SHNAME");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_CODE",
                table: "ObjectKLADRModelDB",
                column: "CODE");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_GNINMB",
                table: "ObjectKLADRModelDB",
                column: "GNINMB");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_INDEX",
                table: "ObjectKLADRModelDB",
                column: "INDEX");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_NAME",
                table: "ObjectKLADRModelDB",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_OCATD",
                table: "ObjectKLADRModelDB",
                column: "OCATD");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_SOCR",
                table: "ObjectKLADRModelDB",
                column: "SOCR");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_STATUS",
                table: "ObjectKLADRModelDB",
                column: "STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectKLADRModelDB_UNO",
                table: "ObjectKLADRModelDB",
                column: "UNO");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbaseKLADRModelDB_KOD_T_ST",
                table: "SocrbaseKLADRModelDB",
                column: "KOD_T_ST");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbaseKLADRModelDB_LEVEL",
                table: "SocrbaseKLADRModelDB",
                column: "LEVEL");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbaseKLADRModelDB_SCNAME",
                table: "SocrbaseKLADRModelDB",
                column: "SCNAME");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbaseKLADRModelDB_SOCRNAME",
                table: "SocrbaseKLADRModelDB",
                column: "SOCRNAME");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_CODE",
                table: "StreetKLADRModelDB",
                column: "CODE");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_GNINMB",
                table: "StreetKLADRModelDB",
                column: "GNINMB");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_INDEX",
                table: "StreetKLADRModelDB",
                column: "INDEX");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_NAME",
                table: "StreetKLADRModelDB",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_OCATD",
                table: "StreetKLADRModelDB",
                column: "OCATD");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_SOCR",
                table: "StreetKLADRModelDB",
                column: "SOCR");

            migrationBuilder.CreateIndex(
                name: "IX_StreetKLADRModelDB_UNO",
                table: "StreetKLADRModelDB",
                column: "UNO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AltnameKLADRModelDB");

            migrationBuilder.DropTable(
                name: "NameMapKLADRModelDB");

            migrationBuilder.DropTable(
                name: "ObjectKLADRModelDB");

            migrationBuilder.DropTable(
                name: "SocrbaseKLADRModelDB");

            migrationBuilder.DropTable(
                name: "StreetKLADRModelDB");
        }
    }
}
