////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class BankLayerContext : DbContext
{
    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public BankLayerContext(DbContextOptions options)
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
#if DEBUG
        options.EnableSensitiveDataLogging(true);
        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
    }

    /// <summary>
    /// Bank`s connection`s
    /// </summary>
    public DbSet<BankConnectionModelDB> ConnectionsBanks { get; set; } = default!;

    /// <summary>
    /// Customer`s banks Id`s
    /// </summary>
    public DbSet<CustomerBankIdModelDB> CustomersBanksIds { get; set; } = default!;

    /// <summary>
    /// Transfer`s bank`s
    /// </summary>
    public DbSet<BankTransferModelDB> TransfersBanks { get; set; } = default!;

    #region TBank    
    /// <summary>
    /// TBank account`s
    /// </summary>
    public DbSet<TBankAccountModelDB> AccountsTBank { get; set; } = default!;

    #region Merchant
    /// <summary>
    /// Incoming merchants-payments
    /// </summary>
    public DbSet<IncomingMerchantPaymentTBankModelDB> IncomingMerchantsPaymentsTBank { get; set; } = default!;

    #region Init payment TBank
    /// <inheritdoc/>
    public DbSet<ReceiptTBankModelDB> ReceiptsTBank { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<ReceiptItemModelDB> ReceiptsItemsTBank { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<SupplierInfoModelDB> SuppliersForReceiptItemsTBank { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<AgentDataModelDB> AgentsForReceiptItemsTBank { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<PaymentsForReceiptTBankModelDB> PaymentsForReceiptsTBank { get; set; } = default!;


    /// <inheritdoc/>
    public DbSet<PaymentInitTBankResultModelDB> PaymentInitResultsTBank { get; set; } = default!;
    #endregion
    #endregion
    #endregion
}