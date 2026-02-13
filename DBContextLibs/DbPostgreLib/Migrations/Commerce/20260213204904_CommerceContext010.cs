using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoodsFilesConfigs_OwnerId_OwnerTypeName",
                table: "GoodsFilesConfigs");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsFilesConfigs_FileId_OwnerId_OwnerTypeName",
                table: "GoodsFilesConfigs",
                columns: new[] { "FileId", "OwnerId", "OwnerTypeName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoodsFilesConfigs_FileId_OwnerId_OwnerTypeName",
                table: "GoodsFilesConfigs");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsFilesConfigs_OwnerId_OwnerTypeName",
                table: "GoodsFilesConfigs",
                columns: new[] { "OwnerId", "OwnerTypeName" },
                unique: true);
        }
    }
}
