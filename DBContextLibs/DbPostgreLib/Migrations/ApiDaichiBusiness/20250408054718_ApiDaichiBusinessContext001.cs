using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.ApiDaichiBusiness
{
    /// <inheritdoc />
    public partial class ApiDaichiBusinessContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ParametersCatalog",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    XML_ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    ID = table.Column<string>(type: "text", nullable: false),
                    MAIN_SECTION = table.Column<string>(type: "text", nullable: true),
                    BRAND = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametersCatalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    XML_ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    KeyIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    XML_ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    IS_DEFAULT = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    CODE = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    VALUE = table.Column<string>(type: "text", nullable: false),
                    GROUP = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attributes_ParametersCatalog_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "ParametersCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotosParameters",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotosParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotosParameters_ParametersCatalog_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "ParametersCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionsParameters",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionsParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionsParameters_ParametersCatalog_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "ParametersCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParamsProducts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BRAND = table.Column<string>(type: "text", nullable: true),
                    ATTR_L_SERIA = table.Column<string>(type: "text", nullable: true),
                    ATTR_L_GOODGROUP = table.Column<string>(type: "text", nullable: true),
                    ATTR_L_GOODTYPE = table.Column<string>(type: "text", nullable: true),
                    ATTR_RUS_NAME_AX = table.Column<string>(type: "text", nullable: true),
                    ATTR_L_IN_UNIT_TYPE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParamsProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParamsProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricesProducts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<string>(type: "text", nullable: true),
                    CURRENCY = table.Column<string>(type: "text", nullable: true),
                    XML_ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricesProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilityGoods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    STORE_AMOUNT = table.Column<int>(type: "integer", nullable: false),
                    DELIVERY_AMOUNT = table.Column<int>(type: "integer", nullable: false),
                    STORE_LIMIT = table.Column<int>(type: "integer", nullable: true),
                    DELIVERY_LIMIT = table.Column<int>(type: "integer", nullable: true),
                    STORE_HIDE_MORE_LIMIT = table.Column<bool>(type: "boolean", nullable: true),
                    DELIVERY_HIDE_MORE_LIMIT = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailabilityGoods_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailabilityGoods_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "public",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_ParentId",
                schema: "public",
                table: "Attributes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityGoods_ProductId",
                schema: "public",
                table: "AvailabilityGoods",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityGoods_StoreId",
                schema: "public",
                table: "AvailabilityGoods",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ParametersCatalog_NAME",
                schema: "public",
                table: "ParametersCatalog",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_ParametersCatalog_XML_ID",
                schema: "public",
                table: "ParametersCatalog",
                column: "XML_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ParamsProducts_ProductId",
                schema: "public",
                table: "ParamsProducts",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotosParameters_Name",
                schema: "public",
                table: "PhotosParameters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PhotosParameters_ParentId",
                schema: "public",
                table: "PhotosParameters",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PricesProducts_NAME",
                schema: "public",
                table: "PricesProducts",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_PricesProducts_PRICE",
                schema: "public",
                table: "PricesProducts",
                column: "PRICE");

            migrationBuilder.CreateIndex(
                name: "IX_PricesProducts_ProductId",
                schema: "public",
                table: "PricesProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PricesProducts_XML_ID",
                schema: "public",
                table: "PricesProducts",
                column: "XML_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NAME",
                schema: "public",
                table: "Products",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_Products_XML_ID",
                schema: "public",
                table: "Products",
                column: "XML_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SectionsParameters_Name",
                schema: "public",
                table: "SectionsParameters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SectionsParameters_ParentId",
                schema: "public",
                table: "SectionsParameters",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_NAME",
                schema: "public",
                table: "Stores",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_XML_ID",
                schema: "public",
                table: "Stores",
                column: "XML_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attributes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AvailabilityGoods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ParamsProducts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PhotosParameters",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PricesProducts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SectionsParameters",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ParametersCatalog",
                schema: "public");
        }
    }
}
