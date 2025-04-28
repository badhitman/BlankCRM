////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class StockSharpAppLayerContext : DbContext
{
    /// <summary>
    /// FileName
    /// </summary>
    private static readonly string _ctxName = nameof(StockSharpAppContext);

    /// <summary>
    /// db Path
    /// </summary>
    public static string DbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _ctxName, $"{(AppDomain.CurrentDomain.FriendlyName.Equals("ef", StringComparison.OrdinalIgnoreCase) ? "StockSharpAppData" : AppDomain.CurrentDomain.FriendlyName)}.db3");


    /// <inheritdoc/>
    public StockSharpAppLayerContext(DbContextOptions options)
        : base(options)
    {
        FileInfo _fi = new(DbPath);

        if (_fi.Directory?.Exists != true)
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);
        //if ()
        //    Database.EnsureCreated();
        //else
        Database.Migrate();
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
#if DEBUG
        options.EnableSensitiveDataLogging(true);
        // options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
        base.OnConfiguring(options);
    }

    public DbSet<OrderStockSharpModelDB> Orders { get; set; }

    /// <inheritdoc/>
    public DbSet<ExchangeStockSharpModelDB> Exchanges { get; set; }

    /// <inheritdoc/>
    public DbSet<BoardStockSharpModelDB> Boards { get; set; }

    /// <inheritdoc/>
    public DbSet<InstrumentStockSharpModelDB> Instruments { get; set; }

    /// <inheritdoc/>
    public DbSet<PortfolioTradeModelDB> Portfolios { get; set; } = default!;
}