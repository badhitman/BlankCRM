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
                name: "IX_RulesFilesAccess_StoreFileId",
                table: "RulesFilesAccess",
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
                name: "RulesFilesAccess");

            migrationBuilder.DropTable(
                name: "CloudFiles");
        }
    }
}
