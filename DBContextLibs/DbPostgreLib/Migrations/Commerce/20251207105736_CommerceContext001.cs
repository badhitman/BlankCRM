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
                name: "DeliveryRetailServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortIndex = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryRetailServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LockerName = table.Column<string>(type: "text", nullable: false),
                    LockerId = table.Column<int>(type: "integer", nullable: false),
                    LockerAreaId = table.Column<int>(type: "integer", nullable: false)
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
                    Name = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                name: "RetailOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateDocument = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BuyerIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusDocument = table.Column<int>(type: "integer", nullable: false),
                    AuthorIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: true),
                    HelpDeskId = table.Column<int>(type: "integer", nullable: true),
                    Version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetailOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletsRetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortIndex = table.Column<int>(type: "integer", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletsRetailTypes", x => x.Id);
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
                    WritingOffWarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    Name = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    BankAddress = table.Column<string>(type: "text", nullable: false),
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
                    AddressUserComment = table.Column<string>(type: "text", nullable: true),
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
                name: "OrdersB2B",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusDocument = table.Column<int>(type: "integer", nullable: false),
                    AuthorIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: true),
                    HelpDeskId = table.Column<int>(type: "integer", nullable: true),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersB2B", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersB2B_Organizations_OrganizationId",
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
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "DeliveryRetailDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryType = table.Column<int>(type: "integer", nullable: false),
                    DeliveryPaymentUponReceipt = table.Column<bool>(type: "boolean", nullable: false),
                    RecipientIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    DeliveryCode = table.Column<string>(type: "text", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "numeric", nullable: false),
                    WeightShipping = table.Column<decimal>(type: "numeric", nullable: false),
                    KladrCode = table.Column<string>(type: "text", nullable: true),
                    KladrTitle = table.Column<string>(type: "text", nullable: true),
                    AddressUserComment = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    AuthorIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryRetailDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryRetailDocuments_RetailOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "RetailOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WalletsRetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserIdentityId = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    WalletTypeId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletsRetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletsRetail_WalletsRetailTypes_WalletTypeId",
                        column: x => x.WalletTypeId,
                        principalTable: "WalletsRetailTypes",
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
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusDocument = table.Column<int>(type: "integer", nullable: false),
                    AuthorIdentityUserId = table.Column<string>(type: "text", nullable: false),
                    ExternalDocumentId = table.Column<string>(type: "text", nullable: true),
                    HelpDeskId = table.Column<int>(type: "integer", nullable: true),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false)
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
                name: "CalendarsSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateScheduleCalendar = table.Column<string>(type: "character varying(10)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ContextName = table.Column<string>(type: "text", nullable: true),
                    NormalizedNameUpper = table.Column<string>(type: "text", nullable: true),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: true),
                    OfferId = table.Column<int>(type: "integer", nullable: true),
                    StartPart = table.Column<long>(type: "bigint", nullable: false),
                    EndPart = table.Column<long>(type: "bigint", nullable: false),
                    QueueCapacity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarsSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarsSchedules_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarsSchedules_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
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
                    Name = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                name: "RowsRetailsOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowsRetailsOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RowsRetailsOrders_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsRetailsOrders_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsRetailsOrders_RetailOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "RetailOrders",
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
                name: "WeeklySchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weekday = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SortIndex = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ContextName = table.Column<string>(type: "text", nullable: true),
                    NormalizedNameUpper = table.Column<string>(type: "text", nullable: true),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: true),
                    OfferId = table.Column<int>(type: "integer", nullable: true),
                    StartPart = table.Column<long>(type: "bigint", nullable: false),
                    EndPart = table.Column<long>(type: "bigint", nullable: false),
                    QueueCapacity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfficesForOrders",
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
                    table.PrimaryKey("PK_OfficesForOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficesForOrders_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficesForOrders_OrdersB2B_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersB2B",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsB2B",
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
                    table.PrimaryKey("PK_PaymentsB2B", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsB2B_OrdersB2B_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersB2B",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatusesRetailDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateOperation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "integer", nullable: false),
                    DeliveryDocumentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatusesRetailDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryStatusesRetailDocuments_DeliveryRetailDocuments_Del~",
                        column: x => x.DeliveryDocumentId,
                        principalTable: "DeliveryRetailDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RowsDeliveryRetailDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentId = table.Column<int>(type: "integer", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    NomenclatureId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowsDeliveryRetailDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RowsDeliveryRetailDocuments_DeliveryRetailDocuments_Documen~",
                        column: x => x.DocumentId,
                        principalTable: "DeliveryRetailDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsDeliveryRetailDocuments_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsDeliveryRetailDocuments_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversionsDocumentsWalletsRetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateDocument = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FromWalletId = table.Column<int>(type: "integer", nullable: false),
                    FromWalletSum = table.Column<decimal>(type: "numeric", nullable: false),
                    ToWalletId = table.Column<int>(type: "integer", nullable: false),
                    ToWalletSum = table.Column<decimal>(type: "numeric", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionsDocumentsWalletsRetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversionsDocumentsWalletsRetail_WalletsRetail_FromWalletId",
                        column: x => x.FromWalletId,
                        principalTable: "WalletsRetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversionsDocumentsWalletsRetail_WalletsRetail_ToWalletId",
                        column: x => x.ToWalletId,
                        principalTable: "WalletsRetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsRetailDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatePayment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TypePayment = table.Column<int>(type: "integer", nullable: false),
                    StatusPayment = table.Column<int>(type: "integer", nullable: false),
                    PaymentSource = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    WalletId = table.Column<int>(type: "integer", nullable: false),
                    AuthorUserIdentity = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsRetailDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsRetailDocuments_WalletsRetail_WalletId",
                        column: x => x.WalletId,
                        principalTable: "WalletsRetail",
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
                        name: "FK_RowsOrders_OfficesForOrders_OfficeOrderTabId",
                        column: x => x.OfficeOrderTabId,
                        principalTable: "OfficesForOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowsOrders_OrdersB2B_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersB2B",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_CreatedAtUTC",
                table: "AttendancesReg",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_ExternalDocumentId_HelpDeskId_AuthorIdentity~",
                table: "AttendancesReg",
                columns: new[] { "ExternalDocumentId", "HelpDeskId", "AuthorIdentityUserId", "StatusDocument" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesReg_LastUpdatedAtUTC",
                table: "AttendancesReg",
                column: "LastUpdatedAtUTC");

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
                name: "IX_CalendarsSchedules_ContextName",
                table: "CalendarsSchedules",
                column: "ContextName");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_DateScheduleCalendar",
                table: "CalendarsSchedules",
                column: "DateScheduleCalendar");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_IsDisabled",
                table: "CalendarsSchedules",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_Name",
                table: "CalendarsSchedules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_NomenclatureId",
                table: "CalendarsSchedules",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_NormalizedNameUpper",
                table: "CalendarsSchedules",
                column: "NormalizedNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_OfferId",
                table: "CalendarsSchedules",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_SortIndex_ParentId_ContextName",
                table: "CalendarsSchedules",
                columns: new[] { "SortIndex", "ParentId", "ContextName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarsSchedules_StartPart_EndPart",
                table: "CalendarsSchedules",
                columns: new[] { "StartPart", "EndPart" });

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
                name: "IX_ConversionsDocumentsWalletsRetail_CreatedAtUTC",
                table: "ConversionsDocumentsWalletsRetail",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_DateDocument",
                table: "ConversionsDocumentsWalletsRetail",
                column: "DateDocument");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_FromWalletId",
                table: "ConversionsDocumentsWalletsRetail",
                column: "FromWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_LastUpdatedAtUTC",
                table: "ConversionsDocumentsWalletsRetail",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_Name",
                table: "ConversionsDocumentsWalletsRetail",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionsDocumentsWalletsRetail_ToWalletId",
                table: "ConversionsDocumentsWalletsRetail",
                column: "ToWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_AddressUserComment",
                table: "DeliveryRetailDocuments",
                column: "AddressUserComment");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_AuthorIdentityUserId",
                table: "DeliveryRetailDocuments",
                column: "AuthorIdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_CreatedAtUTC",
                table: "DeliveryRetailDocuments",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_DeliveryCode",
                table: "DeliveryRetailDocuments",
                column: "DeliveryCode");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_DeliveryPaymentUponReceipt",
                table: "DeliveryRetailDocuments",
                column: "DeliveryPaymentUponReceipt");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_DeliveryType",
                table: "DeliveryRetailDocuments",
                column: "DeliveryType");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_KladrCode",
                table: "DeliveryRetailDocuments",
                column: "KladrCode");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_KladrTitle",
                table: "DeliveryRetailDocuments",
                column: "KladrTitle");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_LastUpdatedAtUTC",
                table: "DeliveryRetailDocuments",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_OrderId",
                table: "DeliveryRetailDocuments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailDocuments_RecipientIdentityUserId",
                table: "DeliveryRetailDocuments",
                column: "RecipientIdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailServices_CreatedAtUTC",
                table: "DeliveryRetailServices",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailServices_IsDisabled",
                table: "DeliveryRetailServices",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRetailServices_LastUpdatedAtUTC",
                table: "DeliveryRetailServices",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusesRetailDocuments_CreatedAtUTC",
                table: "DeliveryStatusesRetailDocuments",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusesRetailDocuments_DateOperation",
                table: "DeliveryStatusesRetailDocuments",
                column: "DateOperation");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusesRetailDocuments_DeliveryDocumentId",
                table: "DeliveryStatusesRetailDocuments",
                column: "DeliveryDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusesRetailDocuments_DeliveryStatus",
                table: "DeliveryStatusesRetailDocuments",
                column: "DeliveryStatus");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusesRetailDocuments_LastUpdatedAtUTC",
                table: "DeliveryStatusesRetailDocuments",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusesRetailDocuments_Name",
                table: "DeliveryStatusesRetailDocuments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_LockerId_LockerName_LockerAreaId",
                table: "LockTransactions",
                columns: new[] { "LockerId", "LockerName", "LockerAreaId" },
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
                name: "IX_Offices_OrganizationId",
                table: "Offices",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficesForOrders_OfficeId",
                table: "OfficesForOrders",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficesForOrders_OrderId",
                table: "OfficesForOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficesForOrders_WarehouseId",
                table: "OfficesForOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersB2B_CreatedAtUTC",
                table: "OrdersB2B",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersB2B_ExternalDocumentId_HelpDeskId_AuthorIdentityUserI~",
                table: "OrdersB2B",
                columns: new[] { "ExternalDocumentId", "HelpDeskId", "AuthorIdentityUserId", "StatusDocument" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersB2B_LastUpdatedAtUTC",
                table: "OrdersB2B",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersB2B_Name",
                table: "OrdersB2B",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersB2B_OrganizationId",
                table: "OrdersB2B",
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
                name: "IX_PaymentsB2B_OrderId",
                table: "PaymentsB2B",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_AuthorUserIdentity",
                table: "PaymentsRetailDocuments",
                column: "AuthorUserIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_CreatedAtUTC",
                table: "PaymentsRetailDocuments",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_DatePayment",
                table: "PaymentsRetailDocuments",
                column: "DatePayment");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_LastUpdatedAtUTC",
                table: "PaymentsRetailDocuments",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_Name",
                table: "PaymentsRetailDocuments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_PaymentSource",
                table: "PaymentsRetailDocuments",
                column: "PaymentSource");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_StatusPayment",
                table: "PaymentsRetailDocuments",
                column: "StatusPayment");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_TypePayment",
                table: "PaymentsRetailDocuments",
                column: "TypePayment");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRetailDocuments_WalletId",
                table: "PaymentsRetailDocuments",
                column: "WalletId");

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
                name: "IX_RetailOrders_BuyerIdentityUserId",
                table: "RetailOrders",
                column: "BuyerIdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RetailOrders_CreatedAtUTC",
                table: "RetailOrders",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_RetailOrders_DateDocument",
                table: "RetailOrders",
                column: "DateDocument");

            migrationBuilder.CreateIndex(
                name: "IX_RetailOrders_ExternalDocumentId_HelpDeskId_AuthorIdentityUs~",
                table: "RetailOrders",
                columns: new[] { "ExternalDocumentId", "HelpDeskId", "AuthorIdentityUserId", "StatusDocument" });

            migrationBuilder.CreateIndex(
                name: "IX_RetailOrders_LastUpdatedAtUTC",
                table: "RetailOrders",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_RetailOrders_Name",
                table: "RetailOrders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RetailOrders_WarehouseId",
                table: "RetailOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsDeliveryRetailDocuments_DocumentId",
                table: "RowsDeliveryRetailDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsDeliveryRetailDocuments_NomenclatureId",
                table: "RowsDeliveryRetailDocuments",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsDeliveryRetailDocuments_OfferId",
                table: "RowsDeliveryRetailDocuments",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsDeliveryRetailDocuments_Quantity",
                table: "RowsDeliveryRetailDocuments",
                column: "Quantity");

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
                name: "IX_RowsRetailsOrders_NomenclatureId",
                table: "RowsRetailsOrders",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsRetailsOrders_OfferId",
                table: "RowsRetailsOrders",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsRetailsOrders_OrderId",
                table: "RowsRetailsOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RowsRetailsOrders_Quantity",
                table: "RowsRetailsOrders",
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
                name: "IX_WalletsRetail_CreatedAtUTC",
                table: "WalletsRetail",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_WalletsRetail_LastUpdatedAtUTC",
                table: "WalletsRetail",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_WalletsRetail_UserIdentityId",
                table: "WalletsRetail",
                column: "UserIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletsRetail_WalletTypeId",
                table: "WalletsRetail",
                column: "WalletTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletsRetailTypes_CreatedAtUTC",
                table: "WalletsRetailTypes",
                column: "CreatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_WalletsRetailTypes_LastUpdatedAtUTC",
                table: "WalletsRetailTypes",
                column: "LastUpdatedAtUTC");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_DeliveryDate",
                table: "WarehouseDocuments",
                column: "DeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDocuments_ExternalDocumentId",
                table: "WarehouseDocuments",
                column: "ExternalDocumentId");

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
                name: "IX_WarehouseDocuments_WritingOffWarehouseId",
                table: "WarehouseDocuments",
                column: "WritingOffWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_ContextName",
                table: "WeeklySchedules",
                column: "ContextName");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_IsDisabled",
                table: "WeeklySchedules",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_Name",
                table: "WeeklySchedules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_NomenclatureId",
                table: "WeeklySchedules",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_NormalizedNameUpper",
                table: "WeeklySchedules",
                column: "NormalizedNameUpper");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_OfferId",
                table: "WeeklySchedules",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_SortIndex_ParentId_ContextName",
                table: "WeeklySchedules",
                columns: new[] { "SortIndex", "ParentId", "ContextName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_StartPart_EndPart",
                table: "WeeklySchedules",
                columns: new[] { "StartPart", "EndPart" });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_Weekday",
                table: "WeeklySchedules",
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
                name: "CalendarsSchedules");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "ConversionsDocumentsWalletsRetail");

            migrationBuilder.DropTable(
                name: "DeliveryRetailServices");

            migrationBuilder.DropTable(
                name: "DeliveryStatusesRetailDocuments");

            migrationBuilder.DropTable(
                name: "LockTransactions");

            migrationBuilder.DropTable(
                name: "OffersAvailability");

            migrationBuilder.DropTable(
                name: "PaymentsB2B");

            migrationBuilder.DropTable(
                name: "PaymentsRetailDocuments");

            migrationBuilder.DropTable(
                name: "PricesRules");

            migrationBuilder.DropTable(
                name: "RowsDeliveryRetailDocuments");

            migrationBuilder.DropTable(
                name: "RowsOrders");

            migrationBuilder.DropTable(
                name: "RowsRetailsOrders");

            migrationBuilder.DropTable(
                name: "RowsWarehouses");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "WeeklySchedules");

            migrationBuilder.DropTable(
                name: "WalletsRetail");

            migrationBuilder.DropTable(
                name: "DeliveryRetailDocuments");

            migrationBuilder.DropTable(
                name: "OfficesForOrders");

            migrationBuilder.DropTable(
                name: "WarehouseDocuments");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "WalletsRetailTypes");

            migrationBuilder.DropTable(
                name: "RetailOrders");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "OrdersB2B");

            migrationBuilder.DropTable(
                name: "Nomenclatures");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
