using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Commerce
{
    /// <inheritdoc />
    public partial class CommerceContext001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LockerName = table.Column<string>(type: "text", nullable: false),
                    LockerId = table.Column<int>(type: "integer", nullable: false),
                    RubricId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nomenclatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseUnit = table.Column<int>(type: "integer", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ContextName = table.Column<string>(type: "text", nullable: true),
                    NormalizedNameUpper = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nomenclatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewName = table.Column<string>(type: "text", nullable: true),
                    NewLegalAddress = table.Column<string>(type: "text", nullable: true),
                    NewINN = table.Column<string>(type: "text", nullable: true),
                    NewKPP = table.Column<string>(type: "text", nullable: true),
                    NewOGRN = table.Column<string>(type: "text", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    LegalAddress = table.Column<string>(type: "text", nullable: false),
                    INN = table.Column<string>(type: "text", nullable: false),
                    KPP = table.Column<string>(type: "text", nullable: true),
                    OGRN = table.Column<string>(type: "text", nullable: false),
                    BankMainAccount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NormalizedUpperName = table.Column<string>(type: "text", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: true),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    QuantitiesTemplate = table.Column<string>(type: "text", nullable: true),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    OfferUnit = table.Column<int>(type: "integer", nullable: false),
                    Multiplicity = table.Column<decimal>(type: "numeric", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BanksDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    BankBIC = table.Column<string>(type: "text", nullable: false),
                    CurrentAccount = table.Column<string>(type: "text", nullable: false),
                    CorrespondentAccount = table.Column<string>(type: "text", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanksDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BanksDetails_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    KladrCode = table.Column<string>(type: "text", nullable: false),
                    KladrTitle = table.Column<string>(type: "text", nullable: false),
                    AddressUserComment = table.Column<string>(type: "text", nullable: false),
                    Contacts = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offices_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StatusDocument = table.Column<int>(type: "integer", nullable: false),
                    AuthorIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: true),
                    Information = table.Column<string>(type: "text", nullable: true),
                    HelpdeskId = table.Column<int>(type: "integer", nullable: true),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    UserStatus = table.Column<int>(type: "integer", nullable: false),
                    UserPersonIdentityId = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendancesReg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    DateExecute = table.Column<DateOnly>(type: "date", nullable: false),
                    StartPart = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndPart = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ContextName = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StatusDocument = table.Column<int>(type: "integer", nullable: false),
                    AuthorIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: true),
                    Information = table.Column<string>(type: "text", nullable: true),
                    HelpdeskId = table.Column<int>(type: "integer", nullable: true),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancesReg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendancesReg_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendancesReg_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendancesReg_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contractors_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contractors_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffersAvailability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffersAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffersAvailability_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OffersAvailability_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricesRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    QuantityRule = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceRule = table.Column<decimal>(type: "numeric", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricesRules_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RowsWarehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarehouseDocumentId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowsWarehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RowsWarehouses_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsWarehouses_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsWarehouses_WarehouseDocuments_WarehouseDocumentId",
                        column: x => x.WarehouseDocumentId,
                        principalTable: "WarehouseDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkScheduleBaseModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: true),
                    OfferId = table.Column<int>(type: "integer", nullable: true),
                    StartPart = table.Column<long>(type: "bigint", nullable: false),
                    EndPart = table.Column<long>(type: "bigint", nullable: false),
                    QueueCapacity = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    DateScheduleCalendar = table.Column<string>(type: "character varying(10)", nullable: true),
                    Weekday = table.Column<int>(type: "integer", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastAtUpdatedUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ContextName = table.Column<string>(type: "text", nullable: true),
                    NormalizedNameUpper = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkScheduleBaseModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkScheduleBaseModelDB_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkScheduleBaseModelDB_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfficesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficesOrders_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficesOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RowsOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfficeOrderTabId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowsOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RowsOrders_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsOrders_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsOrders_OfficesOrders_OfficeOrderTabId",
                        column: x => x.OfficeOrderTabId,
                        principalTable: "OfficesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_Name",
                table: "AttendancesReg",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_NomenclatureId",
                table: "AttendancesReg",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_OfferId",
                table: "AttendancesReg",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_OrganizationId",
                table: "AttendancesReg",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BanksDetails_BankBIC_CorrespondentAccount_CurrentAccount",
                table: "BanksDetails",
                columns: new[] { "BankBIC", "CorrespondentAccount", "CurrentAccount" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BanksDetails_IsDisabled",
                table: "BanksDetails",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_BanksDetails_Name",
                table: "BanksDetails",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BanksDetails_OrganizationId",
                table: "BanksDetails",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_OfferId",
                table: "Contractors",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_OrganizationId_OfferId",
                table: "Contractors",
                columns: new[] { "OrganizationId", "OfferId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_LockerId_LockerName_RubricId",
                table: "LockTransactions",
                columns: new[] { "LockerId", "LockerName", "RubricId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nomenclatures_ContextName",
                table: "Nomenclatures",
                column: "ContextName");

            migrationBuilder.CreateIndex(
                name: "IX_Nomenclatures_IsDisabled",
                table: "Nomenclatures",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Nomenclatures_Name",
                table: "Nomenclatures",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Nomenclatures_NormalizedNameUpper",
                table: "Nomenclatures",
                column: "NormalizedNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_Nomenclatures_SortIndex_ParentId_ContextName",
                table: "Nomenclatures",
                columns: new[] { "SortIndex", "ParentId", "ContextName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_IsDisabled",
                table: "Offers",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Name",
                table: "Offers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_NomenclatureId",
                table: "Offers",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_OffersAvailability_NomenclatureId",
                table: "OffersAvailability",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_OffersAvailability_OfferId",
                table: "OffersAvailability",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OffersAvailability_Quantity",
                table: "OffersAvailability",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_OffersAvailability_WarehouseId_OfferId",
                table: "OffersAvailability",
                columns: new[] { "WarehouseId", "OfferId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_Name",
                table: "Offices",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_OrganizationId",
                table: "Offices",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficesOrders_OfficeId",
                table: "OfficesOrders",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficesOrders_OrderId",
                table: "OfficesOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficesOrders_WarehouseId",
                table: "OfficesOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Name",
                table: "Orders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrganizationId",
                table: "Orders",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_INN",
                table: "Organizations",
                column: "INN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IsDisabled",
                table: "Organizations",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                table: "Organizations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OGRN",
                table: "Organizations",
                column: "OGRN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Name",
                table: "Payments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PricesRules_IsDisabled",
                table: "PricesRules",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_PricesRules_Name",
                table: "PricesRules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PricesRules_OfferId_QuantityRule",
                table: "PricesRules",
                columns: new[] { "OfferId", "QuantityRule" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrders_NomenclatureId",
                table: "RowsOrders",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrders_OfferId",
                table: "RowsOrders",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrders_OfficeOrderTabId",
                table: "RowsOrders",
                column: "OfficeOrderTabId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrders_OfficeOrderTabId_OfferId",
                table: "RowsOrders",
                columns: new[] { "OfficeOrderTabId", "OfferId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrders_OrderId",
                table: "RowsOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsOrders_Quantity",
                table: "RowsOrders",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_RowsWarehouses_NomenclatureId",
                table: "RowsWarehouses",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsWarehouses_OfferId",
                table: "RowsWarehouses",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsWarehouses_Quantity",
                table: "RowsWarehouses",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_RowsWarehouses_WarehouseDocumentId",
                table: "RowsWarehouses",
                column: "WarehouseDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsWarehouses_WarehouseDocumentId_OfferId",
                table: "RowsWarehouses",
                columns: new[] { "WarehouseDocumentId", "OfferId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_OrganizationId_UserPersonIdentityId",
                table: "Units",
                columns: new[] { "OrganizationId", "UserPersonIdentityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_UserPersonIdentityId",
                table: "Units",
                column: "UserPersonIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UserStatus",
                table: "Units",
                column: "UserStatus");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_DeliveryDate",
                table: "WarehouseDocuments",
                column: "DeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_IsDisabled",
                table: "WarehouseDocuments",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_Name",
                table: "WarehouseDocuments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_NormalizedUpperName",
                table: "WarehouseDocuments",
                column: "NormalizedUpperName");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_WarehouseId",
                table: "WarehouseDocuments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_ContextName",
                table: "WorkScheduleBaseModelDB",
                column: "ContextName");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_DateScheduleCalendar",
                table: "WorkScheduleBaseModelDB",
                column: "DateScheduleCalendar");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_IsDisabled",
                table: "WorkScheduleBaseModelDB",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_Name",
                table: "WorkScheduleBaseModelDB",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_NomenclatureId",
                table: "WorkScheduleBaseModelDB",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_NormalizedNameUpper",
                table: "WorkScheduleBaseModelDB",
                column: "NormalizedNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_OfferId",
                table: "WorkScheduleBaseModelDB",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_SortIndex_ParentId_ContextName",
                table: "WorkScheduleBaseModelDB",
                columns: new[] { "SortIndex", "ParentId", "ContextName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_StartPart_EndPart",
                table: "WorkScheduleBaseModelDB",
                columns: new[] { "StartPart", "EndPart" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleBaseModelDB_Weekday",
                table: "WorkScheduleBaseModelDB",
                column: "Weekday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendancesReg");

            migrationBuilder.DropTable(
                name: "BanksDetails");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "LockTransactions");

            migrationBuilder.DropTable(
                name: "OffersAvailability");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PricesRules");

            migrationBuilder.DropTable(
                name: "RowsOrders");

            migrationBuilder.DropTable(
                name: "RowsWarehouses");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "WorkScheduleBaseModelDB");

            migrationBuilder.DropTable(
                name: "OfficesOrders");

            migrationBuilder.DropTable(
                name: "WarehouseDocuments");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Nomenclatures");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
