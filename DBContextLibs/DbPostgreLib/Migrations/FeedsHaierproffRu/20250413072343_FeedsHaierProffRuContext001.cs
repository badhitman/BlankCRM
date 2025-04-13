using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.FeedsHaierProffRu
{
    /// <inheritdoc />
    public partial class FeedsHaierProffRuContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ProductsFeedsRss",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    ParentCategory = table.Column<string>(type: "text", nullable: true),
                    AllArticles = table.Column<string>(type: "text", nullable: true),
                    ImageLink = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsFeedsRss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilesFeedsRss",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesFeedsRss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilesFeedsRss_ProductsFeedsRss_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "ProductsFeedsRss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionsOptionsFeedsRss",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionsOptionsFeedsRss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionsOptionsFeedsRss_ProductsFeedsRss_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "ProductsFeedsRss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionsFeedsRss",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionsFeedsRss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionsFeedsRss_SectionsOptionsFeedsRss_SectionId",
                        column: x => x.SectionId,
                        principalSchema: "public",
                        principalTable: "SectionsOptionsFeedsRss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilesFeedsRss_ProductId",
                schema: "public",
                table: "FilesFeedsRss",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionsFeedsRss_Name",
                schema: "public",
                table: "OptionsFeedsRss",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_OptionsFeedsRss_SectionId",
                schema: "public",
                table: "OptionsFeedsRss",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionsFeedsRss_Value",
                schema: "public",
                table: "OptionsFeedsRss",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFeedsRss_AllArticles",
                schema: "public",
                table: "ProductsFeedsRss",
                column: "AllArticles");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFeedsRss_Category",
                schema: "public",
                table: "ProductsFeedsRss",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFeedsRss_CreatedAt",
                schema: "public",
                table: "ProductsFeedsRss",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFeedsRss_ParentCategory",
                schema: "public",
                table: "ProductsFeedsRss",
                column: "ParentCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFeedsRss_Price",
                schema: "public",
                table: "ProductsFeedsRss",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFeedsRss_UpdatedAt",
                schema: "public",
                table: "ProductsFeedsRss",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SectionsOptionsFeedsRss_Name",
                schema: "public",
                table: "SectionsOptionsFeedsRss",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SectionsOptionsFeedsRss_ProductId",
                schema: "public",
                table: "SectionsOptionsFeedsRss",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesFeedsRss",
                schema: "public");

            migrationBuilder.DropTable(
                name: "OptionsFeedsRss",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SectionsOptionsFeedsRss",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProductsFeedsRss",
                schema: "public");
        }
    }
}
