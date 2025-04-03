using System;
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
                name: "Goods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    XML_ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    KeyIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupsAttributes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CURRENCY = table.Column<string>(type: "text", nullable: true),
                    XML_ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
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
                name: "PricesGoods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GoodsId = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricesGoods_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalSchema: "public",
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attributes_GroupsAttributes_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "public",
                        principalTable: "GroupsAttributes",
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
                    GoodsId = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    STORE_AMOUNT = table.Column<int>(type: "integer", nullable: false),
                    DELIVERY_AMOUNT = table.Column<int>(type: "integer", nullable: false),
                    STORE_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    STORE_HIDE_MORE_LIMIT = table.Column<bool>(type: "boolean", nullable: false),
                    DELIVERY_HIDE_MORE_LIMIT = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailabilityGoods_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalSchema: "public",
                        principalTable: "Goods",
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

            migrationBuilder.CreateTable(
                name: "AttributesValues",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AttributeId = table.Column<int>(type: "integer", nullable: false),
                    GoodsId = table.Column<int>(type: "integer", nullable: false),
                    ValueAttribute = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributesValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributesValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "public",
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributesValues_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalSchema: "public",
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_GroupId",
                schema: "public",
                table: "Attributes",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Name",
                schema: "public",
                table: "Attributes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesValues_AttributeId",
                schema: "public",
                table: "AttributesValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesValues_GoodsId",
                schema: "public",
                table: "AttributesValues",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesValues_Name",
                schema: "public",
                table: "AttributesValues",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityGoods_GoodsId",
                schema: "public",
                table: "AvailabilityGoods",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityGoods_StoreId",
                schema: "public",
                table: "AvailabilityGoods",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_NAME",
                schema: "public",
                table: "Goods",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_XML_ID",
                schema: "public",
                table: "Goods",
                column: "XML_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupsAttributes_Name",
                schema: "public",
                table: "GroupsAttributes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_NAME",
                schema: "public",
                table: "Prices",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_XML_ID",
                schema: "public",
                table: "Prices",
                column: "XML_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PricesGoods_CreatedAt",
                schema: "public",
                table: "PricesGoods",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PricesGoods_GoodsId",
                schema: "public",
                table: "PricesGoods",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_PricesGoods_PRICE",
                schema: "public",
                table: "PricesGoods",
                column: "PRICE");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_NAME",
                schema: "public",
                table: "Stores",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_XML_ID",
                schema: "public",
                table: "Stores",
                column: "XML_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributesValues",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AvailabilityGoods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Prices",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PricesGoods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Attributes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Goods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "GroupsAttributes",
                schema: "public");
        }
    }
}
