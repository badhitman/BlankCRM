////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class CommerceLayerContext : DbContext
{
    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public CommerceLayerContext(DbContextOptions options)
        : base(options)
    {
        //#if DEBUG
        //        Database.EnsureCreated();
        //#else
        Database.Migrate();
        //#endif
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //#if DEBUG
        //        options.EnableSensitiveDataLogging(true);
        //        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        //#endif
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        TimeSpanToTicksConverter converter = new();
        DateOnlyToStringConverter converter2 = new();

        modelBuilder
           .Entity<CalendarScheduleModelDB>()
           .Property(e => e.DateScheduleCalendar)
           .HasConversion(converter2);

        modelBuilder
            .Entity<WeeklyScheduleModelDB>()
            .Property(e => e.StartPart)
            .HasConversion(converter);

        modelBuilder
           .Entity<WeeklyScheduleModelDB>()
           .Property(e => e.EndPart)
           .HasConversion(converter);

        modelBuilder
            .Entity<CalendarScheduleModelDB>()
            .Property(e => e.StartPart)
            .HasConversion(converter);

        modelBuilder
           .Entity<CalendarScheduleModelDB>()
           .Property(e => e.EndPart)
           .HasConversion(converter);
    }

    /// <summary>
    /// Расписание (по дням недели)
    /// </summary>
    public DbSet<WeeklyScheduleModelDB> WeeklySchedules { get; set; } = default!;

    /// <summary>
    /// Расписание на определённую дату (приоритетное)
    /// </summary>
    public DbSet<CalendarScheduleModelDB> CalendarsSchedules { get; set; } = default!;



    /// <summary>
    /// Организации
    /// </summary>
    public DbSet<OrganizationModelDB> Organizations { get; set; } = default!;

    /// <summary>
    /// Банковские реквизиты организаций
    /// </summary>
    public DbSet<BankDetailsModelDB> BanksDetails { get; set; } = default!;

    /// <summary>
    /// Филиалы (Офисы)
    /// </summary>
    public DbSet<OfficeOrganizationModelDB> Offices { get; set; } = default!;

    /// <summary>
    /// Сотрудники компаний
    /// </summary>
    public DbSet<UserOrganizationModelDB> Units { get; set; } = default!;

    /// <summary>
    /// Подрядчики
    /// </summary>
    /// <remarks>
    /// Связь организации с офером
    /// </remarks>
    public DbSet<OrganizationContractorModel> Contractors { get; set; } = default!;


    /// <summary>
    /// Номенклатура
    /// </summary>
    public DbSet<NomenclatureModelDB> Nomenclatures { get; set; } = default!;

    /// <summary>
    /// Offers
    /// </summary>
    public DbSet<OfferModelDB> Offers { get; set; } = default!;

    /// <summary>
    /// Правила формирования цены
    /// </summary>
    public DbSet<PriceRuleForOfferModelDB> PricesRules { get; set; } = default!;

    /// <summary>
    /// Документы поступления
    /// </summary>
    public DbSet<WarehouseDocumentModelDB> WarehouseDocuments { get; set; } = default!;

    /// <summary>
    /// Rows of warehouse documents
    /// </summary>
    public DbSet<RowOfWarehouseDocumentModelDB> RowsWarehouses { get; set; } = default!;



    /// <summary>
    /// Регистры учёта остатков оферов в разрезе складов (топиков)
    /// </summary>
    public DbSet<OfferAvailabilityModelDB> OffersAvailability { get; set; } = default!;

    /// <summary>
    /// Locker offers availability
    /// </summary>
    public DbSet<LockTransactionModelDB> LockTransactions { get; set; } = default!;



    /// <summary>
    /// Заказы на услуги (бронь/запись)
    /// </summary>
    public DbSet<RecordsAttendanceModelDB> AttendancesReg { get; set; } = default!;



    /// <summary>
    /// Заказы товаров со складов
    /// </summary>
    public DbSet<OrderDocumentModelDB> OrdersB2B { get; set; } = default!;

    /// <summary>
    /// Строки заказов
    /// </summary>
    public DbSet<RowOfOrderDocumentModelDB> RowsOrders { get; set; } = default!;

    /// <summary>
    /// Офисы/филиалы организаций в заказе
    /// </summary>
    public DbSet<TabOfficeForOrderModelDb> OfficesForOrders { get; set; } = default!;


    /// <summary>
    /// Payments documents
    /// </summary>
    public DbSet<PaymentDocumentModelDb> PaymentsB2B { get; set; } = default!;


    #region Retail

    #region Order`s
    /// <summary>
    /// RetailOrders
    /// </summary>
    public DbSet<DocumentRetailModelDB> RetailOrders { get; set; } = default!;

    /// <summary>
    /// RowsRetailsOrders
    /// </summary>
    public DbSet<RowOfRetailOrderDocumentModelDB> RowsRetailsOrders { get; set; } = default!;

    /// <summary>
    /// DeliveriesOrdersLinks
    /// </summary>
    public DbSet<RetailDeliveryOrderLinkModelDB> DeliveriesOrdersLinks { get; set; } = default!;

    /// <summary>
    /// DeliveriesOrdersLinks
    /// </summary>
    public DbSet<OrderStatusRetailDocumentModelDB> OrdersStatuses { get; set; } = default!;
    #endregion

    #region Delivery
    /// <summary>
    /// DeliveryDocuments
    /// </summary>
    public DbSet<DeliveryDocumentRetailModelDB> DeliveryRetailDocuments { get; set; } = default!;

    /// <summary>
    /// RowsDeliveryRetailDocuments
    /// </summary>
    public DbSet<RowOfDeliveryRetailDocumentModelDB> RowsDeliveryRetailDocuments { get; set; } = default!;

    /// <summary>
    /// Хроника статусов доставки
    /// </summary>
    public DbSet<DeliveryStatusRetailDocumentModelDB> DeliveryStatusesRetailDocuments { get; set; } = default!;
    #endregion

    #region Payment`s
    /// <summary>
    /// Оплаты
    /// </summary>
    public DbSet<PaymentRetailDocumentModelDB> PaymentsRetailDocuments { get; set; } = default!;

    /// <summary>
    /// Связи платежей с заказами (retail)
    /// </summary>
    public DbSet<PaymentOrderRetailLinkModelDB> PaymentsOrdersLinks { get; set; } = default!;
    #endregion

    #region Wallet`s
    /// <summary>
    /// Типы кошельков
    /// </summary>
    public DbSet<WalletRetailTypeModelDB> WalletsRetailTypes { get; set; } = default!;

    /// <summary>
    /// Кошельки
    /// </summary>
    public DbSet<WalletRetailModelDB> WalletsRetail { get; set; } = default!;
    #endregion

    #region Conversion`s
    /// <summary>
    /// Операции конвертации/перевода между кошельками
    /// </summary>
    public DbSet<WalletConversionRetailDocumentModelDB> ConversionsDocumentsWalletsRetail { get; set; } = default!;

    /// <summary>
    /// ConversionsOrdersLinksRetail
    /// </summary>
    public DbSet<ConversionOrderRetailLinkModelDB> ConversionsOrdersLinksRetail { get; set; } = default!;
    #endregion

    #endregion
}