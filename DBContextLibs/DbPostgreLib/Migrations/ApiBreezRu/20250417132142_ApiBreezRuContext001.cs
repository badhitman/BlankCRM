using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.ApiBreezRu
{
    /// <inheritdoc />
    public partial class ApiBreezRuContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CHPU = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CHPU = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leftovers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasePrice = table.Column<string>(type: "text", nullable: true),
                    CurrencyBasePrice = table.Column<string>(type: "text", nullable: true),
                    RIC = table.Column<string>(type: "text", nullable: true),
                    CurrencyRIC = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CodeNC = table.Column<string>(type: "text", nullable: true),
                    Article = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    Stock = table.Column<string>(type: "text", nullable: true),
                    TimeLastUpdate = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leftovers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PriceRIC = table.Column<string>(type: "text", nullable: true),
                    PriceCurrencyRIC = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NC = table.Column<string>(type: "text", nullable: true),
                    VnutrNC = table.Column<string>(type: "text", nullable: true),
                    NarujNC = table.Column<string>(type: "text", nullable: true),
                    AccessoryNC = table.Column<string>(type: "text", nullable: true),
                    Article = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Brand = table.Column<string>(type: "text", nullable: true),
                    UTP = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Booklet = table.Column<string>(type: "text", nullable: true),
                    Manual = table.Column<string>(type: "text", nullable: true),
                    BimModel = table.Column<string>(type: "text", nullable: true),
                    VideoYoutube = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechsCategories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechsProducts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NC = table.Column<string>(type: "text", nullable: true),
                    VnutrNC = table.Column<string>(type: "text", nullable: true),
                    NarujNC = table.Column<string>(type: "text", nullable: true),
                    AccessoryNC = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechsProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImagesProducts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropsTechsCategories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    Group = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Filter = table.Column<string>(type: "text", nullable: true),
                    FilterType = table.Column<string>(type: "text", nullable: true),
                    Required = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    TechId = table.Column<int>(type: "integer", nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropsTechsCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropsTechsCategories_TechsCategories_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "TechsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropsTechsProducts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    Group = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Filter = table.Column<string>(type: "text", nullable: true),
                    FilterType = table.Column<string>(type: "text", nullable: true),
                    Required = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    IdChar = table.Column<int>(type: "integer", nullable: false),
                    TypeParameter = table.Column<string>(type: "text", nullable: false),
                    Show = table.Column<string>(type: "text", nullable: false),
                    First = table.Column<string>(type: "text", nullable: false),
                    SubCategory = table.Column<string>(type: "text", nullable: false),
                    Analog = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropsTechsProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropsTechsProducts_TechsProducts_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "TechsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagesProducts_Name",
                schema: "public",
                table: "ImagesProducts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesProducts_ProductId",
                schema: "public",
                table: "ImagesProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_Article",
                schema: "public",
                table: "Leftovers",
                column: "Article");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CodeNC",
                schema: "public",
                table: "Leftovers",
                column: "CodeNC");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CreatedAt",
                schema: "public",
                table: "Leftovers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CurrencyBasePrice",
                schema: "public",
                table: "Leftovers",
                column: "CurrencyBasePrice");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_CurrencyRIC",
                schema: "public",
                table: "Leftovers",
                column: "CurrencyRIC");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_Quantity",
                schema: "public",
                table: "Leftovers",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_Stock",
                schema: "public",
                table: "Leftovers",
                column: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_TimeLastUpdate",
                schema: "public",
                table: "Leftovers",
                column: "TimeLastUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Leftovers_UpdatedAt",
                schema: "public",
                table: "Leftovers",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AccessoryNC",
                schema: "public",
                table: "Products",
                column: "AccessoryNC");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Article",
                schema: "public",
                table: "Products",
                column: "Article");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Brand",
                schema: "public",
                table: "Products",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "public",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedAt",
                schema: "public",
                table: "Products",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NarujNC",
                schema: "public",
                table: "Products",
                column: "NarujNC");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NC",
                schema: "public",
                table: "Products",
                column: "NC");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PriceRIC",
                schema: "public",
                table: "Products",
                column: "PriceRIC");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Series",
                schema: "public",
                table: "Products",
                column: "Series");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Title",
                schema: "public",
                table: "Products",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdatedAt",
                schema: "public",
                table: "Products",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UTP",
                schema: "public",
                table: "Products",
                column: "UTP");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VnutrNC",
                schema: "public",
                table: "Products",
                column: "VnutrNC");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsCategories_ParentId",
                schema: "public",
                table: "PropsTechsCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_Analog",
                schema: "public",
                table: "PropsTechsProducts",
                column: "Analog");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_First",
                schema: "public",
                table: "PropsTechsProducts",
                column: "First");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_IdChar",
                schema: "public",
                table: "PropsTechsProducts",
                column: "IdChar");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_ParentId",
                schema: "public",
                table: "PropsTechsProducts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_Show",
                schema: "public",
                table: "PropsTechsProducts",
                column: "Show");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_SubCategory",
                schema: "public",
                table: "PropsTechsProducts",
                column: "SubCategory");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_TypeParameter",
                schema: "public",
                table: "PropsTechsProducts",
                column: "TypeParameter");

            migrationBuilder.CreateIndex(
                name: "IX_PropsTechsProducts_Value",
                schema: "public",
                table: "PropsTechsProducts",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_TechsProducts_AccessoryNC",
                schema: "public",
                table: "TechsProducts",
                column: "AccessoryNC");

            migrationBuilder.CreateIndex(
                name: "IX_TechsProducts_CreatedAt",
                schema: "public",
                table: "TechsProducts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TechsProducts_NarujNC",
                schema: "public",
                table: "TechsProducts",
                column: "NarujNC");

            migrationBuilder.CreateIndex(
                name: "IX_TechsProducts_NC",
                schema: "public",
                table: "TechsProducts",
                column: "NC");

            migrationBuilder.CreateIndex(
                name: "IX_TechsProducts_UpdatedAt",
                schema: "public",
                table: "TechsProducts",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TechsProducts_VnutrNC",
                schema: "public",
                table: "TechsProducts",
                column: "VnutrNC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brands",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ImagesProducts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Leftovers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PropsTechsCategories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PropsTechsProducts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TechsCategories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TechsProducts",
                schema: "public");
        }
    }
}
