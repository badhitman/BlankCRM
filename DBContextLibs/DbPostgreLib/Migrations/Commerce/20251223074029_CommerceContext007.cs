using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumWeekOfYear",
                table: "OrdersRetail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersRetail_NumWeekOfYear",
                table: "OrdersRetail",
                column: "NumWeekOfYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrdersRetail_NumWeekOfYear",
                table: "OrdersRetail");

            migrationBuilder.DropColumn(
                name: "NumWeekOfYear",
                table: "OrdersRetail");
        }
    }
}
