using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DeliveryDocumentsRetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageSize",
                table: "DeliveryDocumentsRetail",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DeliveryDocumentsRetail");

            migrationBuilder.DropColumn(
                name: "PackageSize",
                table: "DeliveryDocumentsRetail");
        }
    }
}
