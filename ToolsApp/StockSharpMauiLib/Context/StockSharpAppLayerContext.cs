////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class StockSharpAppLayerContext : DbContext
{
    /// <inheritdoc/>
    public StockSharpAppLayerContext(DbContextOptions options)
        : base(options)
    {
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
        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
    }

    public DbSet<InstrumentTradeModelDB> Instruments { get; set; }

    public DbSet<ExchangeBoardModelDB> BoardsExchange { get; set; }

    public DbSet<InstrumentExternalIdModelDB> InstrumentsExternalsIds { get; set; }
}