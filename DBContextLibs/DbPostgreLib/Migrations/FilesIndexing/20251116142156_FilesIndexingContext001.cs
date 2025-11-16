using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.FilesIndexing
{
    /// <inheritdoc />
    public partial class FilesIndexingContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParagraphsWordIndexesFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: true),
                    ParagraphId = table.Column<string>(type: "text", nullable: true),
                    StoreFileId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParagraphsWordIndexesFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheetsExcelIndexesFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    StoreFileId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheetsExcelIndexesFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TablesWordIndexesFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoreFileId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablesWordIndexesFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataTablesExcelIndexesFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SheetExcelFileId = table.Column<int>(type: "integer", nullable: false),
                    StoreFileId = table.Column<int>(type: "integer", nullable: false),
                    RowNum = table.Column<long>(type: "bigint", nullable: false),
                    ColNum = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTablesExcelIndexesFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTablesExcelIndexesFiles_SheetsExcelIndexesFiles_SheetEx~",
                        column: x => x.SheetExcelFileId,
                        principalTable: "SheetsExcelIndexesFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataTablesWordIndexesFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableWordFileId = table.Column<int>(type: "integer", nullable: false),
                    ParagraphId = table.Column<string>(type: "text", nullable: true),
                    StoreFileId = table.Column<int>(type: "integer", nullable: false),
                    RowNum = table.Column<long>(type: "bigint", nullable: false),
                    ColNum = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTablesWordIndexesFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTablesWordIndexesFiles_TablesWordIndexesFiles_TableWord~",
                        column: x => x.TableWordFileId,
                        principalTable: "TablesWordIndexesFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataTablesExcelIndexesFiles_Data",
                table: "DataTablesExcelIndexesFiles",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_DataTablesExcelIndexesFiles_SheetExcelFileId",
                table: "DataTablesExcelIndexesFiles",
                column: "SheetExcelFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTablesExcelIndexesFiles_StoreFileId",
                table: "DataTablesExcelIndexesFiles",
                column: "StoreFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTablesWordIndexesFiles_Data",
                table: "DataTablesWordIndexesFiles",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_DataTablesWordIndexesFiles_StoreFileId",
                table: "DataTablesWordIndexesFiles",
                column: "StoreFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTablesWordIndexesFiles_TableWordFileId",
                table: "DataTablesWordIndexesFiles",
                column: "TableWordFileId");

            migrationBuilder.CreateIndex(
                name: "IX_ParagraphsWordIndexesFiles_Data",
                table: "ParagraphsWordIndexesFiles",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_ParagraphsWordIndexesFiles_SortIndex",
                table: "ParagraphsWordIndexesFiles",
                column: "SortIndex");

            migrationBuilder.CreateIndex(
                name: "IX_ParagraphsWordIndexesFiles_StoreFileId",
                table: "ParagraphsWordIndexesFiles",
                column: "StoreFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SheetsExcelIndexesFiles_SortIndex",
                table: "SheetsExcelIndexesFiles",
                column: "SortIndex");

            migrationBuilder.CreateIndex(
                name: "IX_SheetsExcelIndexesFiles_StoreFileId",
                table: "SheetsExcelIndexesFiles",
                column: "StoreFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SheetsExcelIndexesFiles_Title",
                table: "SheetsExcelIndexesFiles",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_TablesWordIndexesFiles_SortIndex",
                table: "TablesWordIndexesFiles",
                column: "SortIndex");

            migrationBuilder.CreateIndex(
                name: "IX_TablesWordIndexesFiles_StoreFileId",
                table: "TablesWordIndexesFiles",
                column: "StoreFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataTablesExcelIndexesFiles");

            migrationBuilder.DropTable(
                name: "DataTablesWordIndexesFiles");

            migrationBuilder.DropTable(
                name: "ParagraphsWordIndexesFiles");

            migrationBuilder.DropTable(
                name: "SheetsExcelIndexesFiles");

            migrationBuilder.DropTable(
                name: "TablesWordIndexesFiles");
        }
    }
}
