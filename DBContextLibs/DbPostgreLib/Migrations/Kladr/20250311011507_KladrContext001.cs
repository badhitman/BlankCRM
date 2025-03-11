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
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "AltnamesKLADR",
                schema: "public",
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
                    table.PrimaryKey("PK_AltnamesKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HousesKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CODE = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    KORP = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousesKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NamesMapsKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CODE = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    SHNAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SCNAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamesMapsKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectsKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CODE = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    STATUS = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectsKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocrbasesKLADR",
                schema: "public",
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
                    table.PrimaryKey("PK_SocrbasesKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreetsKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CODE = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetsKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempAltnamesKLADR",
                schema: "public",
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
                    table.PrimaryKey("PK_TempAltnamesKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempHousesKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CODE = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    KORP = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempHousesKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempNamesMapsKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CODE = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    SHNAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SCNAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempNamesMapsKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempObjectsKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CODE = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    STATUS = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempObjectsKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempSocrbasesKLADR",
                schema: "public",
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
                    table.PrimaryKey("PK_TempSocrbasesKLADR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempStreetsKLADR",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SOCR = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INDEX = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GNINMB = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    UNO = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OCATD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CODE = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempStreetsKLADR", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AltnamesKLADR_LEVEL",
                schema: "public",
                table: "AltnamesKLADR",
                column: "LEVEL");

            migrationBuilder.CreateIndex(
                name: "IX_AltnamesKLADR_NEWCODE",
                schema: "public",
                table: "AltnamesKLADR",
                column: "NEWCODE");

            migrationBuilder.CreateIndex(
                name: "IX_AltnamesKLADR_OLDCODE",
                schema: "public",
                table: "AltnamesKLADR",
                column: "OLDCODE");

            migrationBuilder.CreateIndex(
                name: "IX_HousesKLADR_CODE",
                schema: "public",
                table: "HousesKLADR",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HousesKLADR_GNINMB",
                schema: "public",
                table: "HousesKLADR",
                column: "GNINMB");

            migrationBuilder.CreateIndex(
                name: "IX_HousesKLADR_INDEX",
                schema: "public",
                table: "HousesKLADR",
                column: "INDEX");

            migrationBuilder.CreateIndex(
                name: "IX_HousesKLADR_OCATD",
                schema: "public",
                table: "HousesKLADR",
                column: "OCATD");

            migrationBuilder.CreateIndex(
                name: "IX_HousesKLADR_SOCR",
                schema: "public",
                table: "HousesKLADR",
                column: "SOCR");

            migrationBuilder.CreateIndex(
                name: "IX_HousesKLADR_UNO",
                schema: "public",
                table: "HousesKLADR",
                column: "UNO");

            migrationBuilder.CreateIndex(
                name: "IX_NamesMapsKLADR_CODE",
                schema: "public",
                table: "NamesMapsKLADR",
                column: "CODE");

            migrationBuilder.CreateIndex(
                name: "IX_NamesMapsKLADR_NAME",
                schema: "public",
                table: "NamesMapsKLADR",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_NamesMapsKLADR_SCNAME",
                schema: "public",
                table: "NamesMapsKLADR",
                column: "SCNAME");

            migrationBuilder.CreateIndex(
                name: "IX_NamesMapsKLADR_SHNAME",
                schema: "public",
                table: "NamesMapsKLADR",
                column: "SHNAME");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_CODE_STATUS",
                schema: "public",
                table: "ObjectsKLADR",
                columns: new[] { "CODE", "STATUS" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_GNINMB",
                schema: "public",
                table: "ObjectsKLADR",
                column: "GNINMB");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_INDEX",
                schema: "public",
                table: "ObjectsKLADR",
                column: "INDEX");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_NAME",
                schema: "public",
                table: "ObjectsKLADR",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_OCATD",
                schema: "public",
                table: "ObjectsKLADR",
                column: "OCATD");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_SOCR",
                schema: "public",
                table: "ObjectsKLADR",
                column: "SOCR");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_STATUS",
                schema: "public",
                table: "ObjectsKLADR",
                column: "STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectsKLADR_UNO",
                schema: "public",
                table: "ObjectsKLADR",
                column: "UNO");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbasesKLADR_KOD_T_ST",
                schema: "public",
                table: "SocrbasesKLADR",
                column: "KOD_T_ST");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbasesKLADR_LEVEL",
                schema: "public",
                table: "SocrbasesKLADR",
                column: "LEVEL");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbasesKLADR_SCNAME",
                schema: "public",
                table: "SocrbasesKLADR",
                column: "SCNAME");

            migrationBuilder.CreateIndex(
                name: "IX_SocrbasesKLADR_SOCRNAME",
                schema: "public",
                table: "SocrbasesKLADR",
                column: "SOCRNAME");

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_CODE",
                schema: "public",
                table: "StreetsKLADR",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_GNINMB",
                schema: "public",
                table: "StreetsKLADR",
                column: "GNINMB");

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_INDEX",
                schema: "public",
                table: "StreetsKLADR",
                column: "INDEX");

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_NAME",
                schema: "public",
                table: "StreetsKLADR",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_OCATD",
                schema: "public",
                table: "StreetsKLADR",
                column: "OCATD");

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_SOCR",
                schema: "public",
                table: "StreetsKLADR",
                column: "SOCR");

            migrationBuilder.CreateIndex(
                name: "IX_StreetsKLADR_UNO",
                schema: "public",
                table: "StreetsKLADR",
                column: "UNO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AltnamesKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "HousesKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NamesMapsKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ObjectsKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SocrbasesKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StreetsKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempAltnamesKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempHousesKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempNamesMapsKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempObjectsKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempSocrbasesKLADR",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempStreetsKLADR",
                schema: "public");
        }
    }
}
