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
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
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
                name: "ProductsInformation",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId1 = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    TypeInfo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsInformation_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Remains",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId1 = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Total = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remains_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductsProperties",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId1 = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    PropertyId1 = table.Column<string>(type: "text", nullable: true),
                    PropertyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsProperties_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductsProperties_PropertiesCatalog_PropertyId1",
                        column: x => x.PropertyId1,
                        principalSchema: "public",
                        principalTable: "PropertiesCatalog",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInformation_Name",
                schema: "public",
                table: "ProductsInformation",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInformation_ProductId1",
                schema: "public",
                table: "ProductsInformation",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProperties_ProductId1",
                schema: "public",
                table: "ProductsProperties",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProperties_PropertyId1",
                schema: "public",
                table: "ProductsProperties",
                column: "PropertyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Remains_ProductId1",
                schema: "public",
                table: "Remains",
                column: "ProductId1");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsInformation",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProductsProperties",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WarehousesRemains",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PropertiesCatalog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Remains",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");
        }
    }
}
