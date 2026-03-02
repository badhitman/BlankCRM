using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbPostgreLib.Migrations.Constructor
{
    /// <inheritdoc />
    public partial class ConstructorContext002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectsUse_UserId",
                table: "ProjectsUse");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsUse_UserId_ProjectId",
                table: "ProjectsUse",
                columns: new[] { "UserId", "ProjectId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectsUse_UserId_ProjectId",
                table: "ProjectsUse");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsUse_UserId",
                table: "ProjectsUse",
                column: "UserId",
                unique: true);
        }
    }
}
