using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersDialogsJoins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserIdentityId = table.Column<string>(type: "text", nullable: false),
                    DialogJoinId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDialogsJoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersDialogsJoins_Dialogs_DialogJoinId",
                        column: x => x.DialogJoinId,
                        principalTable: "Dialogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_DialogJoinId",
                table: "UsersDialogsJoins",
                column: "DialogJoinId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_UserIdentityId_DialogJoinId",
                table: "UsersDialogsJoins",
                columns: new[] { "UserIdentityId", "DialogJoinId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersDialogsJoins");
        }
    }
}
