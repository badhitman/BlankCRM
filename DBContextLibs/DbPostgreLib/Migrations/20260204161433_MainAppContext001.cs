using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations
{
    /// <inheritdoc />
    public partial class MainAppContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dialogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InitiatorContactsNormalized = table.Column<string>(type: "text", nullable: true),
                    InitiatorContacts = table.Column<string>(type: "text", nullable: true),
                    InitiatorHumanName = table.Column<string>(type: "text", nullable: true),
                    InitiatorIdentityId = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastMessageAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastOnlineAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeadlineUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    SessionTicketId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderUserIdentityId = table.Column<string>(type: "text", nullable: true),
                    DialogOwnerId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsInsideMessage = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Dialogs_DialogOwnerId",
                        column: x => x.DialogOwnerId,
                        principalTable: "Dialogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersDialogsJoins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JoinedDateUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OutDateUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "AttachesFilesOfMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageOwnerId = table.Column<int>(type: "integer", nullable: false),
                    FileAttachId = table.Column<int>(type: "integer", nullable: false),
                    FileAttachName = table.Column<string>(type: "text", nullable: true),
                    FileLength = table.Column<long>(type: "bigint", nullable: false),
                    FileTokenAccess = table.Column<string>(type: "text", nullable: true)
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
                column: "MessageOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_DeadlineUTC",
                table: "Dialogs",
                column: "DeadlineUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_InitiatorContacts",
                table: "Dialogs",
                column: "InitiatorContacts");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_InitiatorContactsNormalized",
                table: "Dialogs",
                column: "InitiatorContactsNormalized");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_InitiatorIdentityId",
                table: "Dialogs",
                column: "InitiatorIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_IsDisabled",
                table: "Dialogs",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_LastMessageAtUTC",
                table: "Dialogs",
                column: "LastMessageAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_LastOnlineAtUTC",
                table: "Dialogs",
                column: "LastOnlineAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_SessionTicketId",
                table: "Dialogs",
                column: "SessionTicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatedAtUTC",
                table: "Messages",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogOwnerId",
                table: "Messages",
                column: "DialogOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IsDisabled",
                table: "Messages",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderUserIdentityId",
                table: "Messages",
                column: "SenderUserIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_DialogJoinId",
                table: "UsersDialogsJoins",
                column: "DialogJoinId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_JoinedDateUTC",
                table: "UsersDialogsJoins",
                column: "JoinedDateUTC");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_OutDateUTC",
                table: "UsersDialogsJoins",
                column: "OutDateUTC");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDialogsJoins_UserIdentityId",
                table: "UsersDialogsJoins",
                column: "UserIdentityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachesFilesOfMessages");

            migrationBuilder.DropTable(
                name: "UsersDialogsJoins");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Dialogs");
        }
    }
}
