using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachFileId",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "AttachesFilesOfMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageOwnerId = table.Column<int>(type: "integer", nullable: false),
                    FileAttachId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachesFilesOfMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachesFilesOfMessages_Messages_MessageOwnerId",
                        column: x => x.MessageOwnerId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachesFilesOfMessages_MessageOwnerId",
                table: "AttachesFilesOfMessages",
                column: "MessageOwnerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachesFilesOfMessages");

            migrationBuilder.AddColumn<int>(
                name: "AttachFileId",
                table: "Messages",
                type: "integer",
                nullable: true);
        }
    }
}
