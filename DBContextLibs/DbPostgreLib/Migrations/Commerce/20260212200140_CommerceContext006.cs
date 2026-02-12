using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "OffersAvailability",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Offers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GoodsFilesConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    OwnerTypeName = table.Column<int>(type: "integer", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    FullDescription = table.Column<string>(type: "text", nullable: true),
                    IsGallery = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsFilesConfigs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OffersAvailability_OrganizationId",
                table: "OffersAvailability",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OrganizationId",
                table: "Offers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsFilesConfigs_IsDisabled",
                table: "GoodsFilesConfigs",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsFilesConfigs_IsGallery",
                table: "GoodsFilesConfigs",
                column: "IsGallery");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsFilesConfigs_Name",
                table: "GoodsFilesConfigs",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsFilesConfigs_OwnerId_OwnerTypeName",
                table: "GoodsFilesConfigs",
                columns: new[] { "OwnerId", "OwnerTypeName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Organizations_OrganizationId",
                table: "Offers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OffersAvailability_Organizations_OrganizationId",
                table: "OffersAvailability",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Organizations_OrganizationId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_OffersAvailability_Organizations_OrganizationId",
                table: "OffersAvailability");

            migrationBuilder.DropTable(
                name: "GoodsFilesConfigs");

            migrationBuilder.DropIndex(
                name: "IX_OffersAvailability_OrganizationId",
                table: "OffersAvailability");

            migrationBuilder.DropIndex(
                name: "IX_Offers_OrganizationId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "OffersAvailability");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Offers");
        }
    }
}
