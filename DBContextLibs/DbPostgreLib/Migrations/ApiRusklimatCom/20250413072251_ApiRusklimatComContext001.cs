using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.ApiRusklimatCom
{
    /// <inheritdoc />
    public partial class ApiRusklimatComContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Parent = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertiesCatalog",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesCatalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Remains",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Total = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NameFull = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    IntAbbr = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RemainsId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NSCode = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    VendorCode = table.Column<string>(type: "text", nullable: true),
                    Brand = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ClientPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    InternetPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Exclusive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Remains_RemainsId",
                        column: x => x.RemainsId,
                        principalSchema: "public",
                        principalTable: "Remains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehousesRemains",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    RemainValue = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehousesRemains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehousesRemains_Remains_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "Remains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsInformation",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    TypeInfo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsInformation_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsProperties",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    PropertyKey = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsProperties_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Parent",
                schema: "public",
                table: "Categories",
                column: "Parent");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedAt",
                schema: "public",
                table: "Products",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RemainsId",
                schema: "public",
                table: "Products",
                column: "RemainsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdatedAt",
                schema: "public",
                table: "Products",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInformation_Name",
                schema: "public",
                table: "ProductsInformation",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInformation_ProductId",
                schema: "public",
                table: "ProductsInformation",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInformation_TypeInfo",
                schema: "public",
                table: "ProductsInformation",
                column: "TypeInfo");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProperties_ProductId",
                schema: "public",
                table: "ProductsProperties",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProperties_PropertyKey",
                schema: "public",
                table: "ProductsProperties",
                column: "PropertyKey");

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesCatalog_Sort",
                schema: "public",
                table: "PropertiesCatalog",
                column: "Sort");

            migrationBuilder.CreateIndex(
                name: "IX_Remains_Total",
                schema: "public",
                table: "Remains",
                column: "Total");

            migrationBuilder.CreateIndex(
                name: "IX_WarehousesRemains_Name",
                schema: "public",
                table: "WarehousesRemains",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WarehousesRemains_ParentId",
                schema: "public",
                table: "WarehousesRemains",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehousesRemains_RemainValue",
                schema: "public",
                table: "WarehousesRemains",
                column: "RemainValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProductsInformation",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProductsProperties",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PropertiesCatalog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WarehousesRemains",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Remains",
                schema: "public");
        }
    }
}
