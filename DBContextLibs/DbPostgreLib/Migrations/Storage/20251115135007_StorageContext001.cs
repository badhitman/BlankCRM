using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Storage
{
    /// <inheritdoc />
    public partial class StorageContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NormalizedFileNameUpper = table.Column<string>(type: "text", nullable: true),
                    FileLength = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    ReferrerMain = table.Column<string>(type: "text", nullable: true),
                    ApplicationName = table.Column<string>(type: "text", nullable: true),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    PrefixPropertyName = table.Column<string>(type: "text", nullable: true),
                    OwnerPrimaryKey = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorIdentityId = table.Column<string>(type: "text", nullable: false),
                    PointId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationName = table.Column<string>(type: "text", nullable: true),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    PrefixPropertyName = table.Column<string>(type: "text", nullable: true),
                    OwnerPrimaryKey = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SerializedDataJson = table.Column<string>(type: "text", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationName = table.Column<string>(type: "text", nullable: true),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    PrefixPropertyName = table.Column<string>(type: "text", nullable: true),
                    OwnerPrimaryKey = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NormalizedTagNameUpper = table.Column<string>(type: "text", nullable: true),
                    TagName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudTags", x => x.Id);
                });

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
                    table.ForeignKey(
                        name: "FK_ParagraphsWordIndexesFiles_CloudFiles_StoreFileId",
                        column: x => x.StoreFileId,
                        principalTable: "CloudFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulesFilesAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoreFileId = table.Column<int>(type: "integer", nullable: false),
                    AccessRuleType = table.Column<int>(type: "integer", nullable: false),
                    Option = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesFilesAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulesFilesAccess_CloudFiles_StoreFileId",
                        column: x => x.StoreFileId,
                        principalTable: "CloudFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_SheetsExcelIndexesFiles_CloudFiles_StoreFileId",
                        column: x => x.StoreFileId,
                        principalTable: "CloudFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_TablesWordIndexesFiles_CloudFiles_StoreFileId",
                        column: x => x.StoreFileId,
                        principalTable: "CloudFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_DataTablesExcelIndexesFiles_CloudFiles_StoreFileId",
                        column: x => x.StoreFileId,
                        principalTable: "CloudFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_DataTablesWordIndexesFiles_CloudFiles_StoreFileId",
                        column: x => x.StoreFileId,
                        principalTable: "CloudFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataTablesWordIndexesFiles_TablesWordIndexesFiles_TableWord~",
                        column: x => x.TableWordFileId,
                        principalTable: "TablesWordIndexesFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_ApplicationName_PropertyName",
                table: "CloudFiles",
                columns: new[] { "ApplicationName", "PropertyName" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_AuthorIdentityId",
                table: "CloudFiles",
                column: "AuthorIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_CreatedAt",
                table: "CloudFiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_FileName",
                table: "CloudFiles",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_NormalizedFileNameUpper",
                table: "CloudFiles",
                column: "NormalizedFileNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_PointId",
                table: "CloudFiles",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_PrefixPropertyName_OwnerPrimaryKey",
                table: "CloudFiles",
                columns: new[] { "PrefixPropertyName", "OwnerPrimaryKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_ReferrerMain",
                table: "CloudFiles",
                column: "ReferrerMain");

            migrationBuilder.CreateIndex(
                name: "IX_CloudProperties_ApplicationName_PropertyName",
                table: "CloudProperties",
                columns: new[] { "ApplicationName", "PropertyName" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudProperties_CreatedAt",
                table: "CloudProperties",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CloudProperties_PrefixPropertyName_OwnerPrimaryKey",
                table: "CloudProperties",
                columns: new[] { "PrefixPropertyName", "OwnerPrimaryKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudProperties_TypeName",
                table: "CloudProperties",
                column: "TypeName");

            migrationBuilder.CreateIndex(
                name: "IX_CloudTags_ApplicationName_PropertyName",
                table: "CloudTags",
                columns: new[] { "ApplicationName", "PropertyName" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudTags_CreatedAt",
                table: "CloudTags",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CloudTags_NormalizedTagNameUpper",
                table: "CloudTags",
                column: "NormalizedTagNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_CloudTags_PrefixPropertyName_OwnerPrimaryKey",
                table: "CloudTags",
                columns: new[] { "PrefixPropertyName", "OwnerPrimaryKey" });

            migrationBuilder.CreateIndex(
                name: "IX_TagNameOwnerKeyUnique",
                table: "CloudTags",
                columns: new[] { "NormalizedTagNameUpper", "OwnerPrimaryKey", "ApplicationName" },
                unique: true);

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
                name: "IX_RulesFilesAccess_StoreFileId",
                table: "RulesFilesAccess",
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
                name: "CloudProperties");

            migrationBuilder.DropTable(
                name: "CloudTags");

            migrationBuilder.DropTable(
                name: "DataTablesExcelIndexesFiles");

            migrationBuilder.DropTable(
                name: "DataTablesWordIndexesFiles");

            migrationBuilder.DropTable(
                name: "ParagraphsWordIndexesFiles");

            migrationBuilder.DropTable(
                name: "RulesFilesAccess");

            migrationBuilder.DropTable(
                name: "SheetsExcelIndexesFiles");

            migrationBuilder.DropTable(
                name: "TablesWordIndexesFiles");

            migrationBuilder.DropTable(
                name: "CloudFiles");
        }
    }
}
