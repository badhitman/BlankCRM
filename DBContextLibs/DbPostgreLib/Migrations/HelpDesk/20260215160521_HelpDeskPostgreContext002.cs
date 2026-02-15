using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.HelpDesk
{
    /// <inheritdoc />
    public partial class HelpDeskPostgreContext002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrefixName",
                table: "Rubrics",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_PrefixName",
                table: "Rubrics",
                column: "PrefixName");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_NormalizedNameUpper",
                table: "Articles",
                column: "NormalizedNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ProjectId",
                table: "Articles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SortIndex",
                table: "Articles",
                column: "SortIndex");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rubrics_PrefixName",
                table: "Rubrics");

            migrationBuilder.DropIndex(
                name: "IX_Articles_NormalizedNameUpper",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ProjectId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_SortIndex",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PrefixName",
                table: "Rubrics");
        }
    }
}
