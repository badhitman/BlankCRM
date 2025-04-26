////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class StockSharpAppContext(DbContextOptions<StockSharpAppContext> options) : StockSharpAppLayerContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        options
            .UseSqlite($"Filename={DbPath}", b => b.MigrationsAssembly("StockSharpMauiMigration"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
         modelBuilder.Entity<InstrumentTradeModelDB>()
            .HasOne(a => a.ExternalId)
            .WithOne(a => a.ParentInstrument)
            .HasForeignKey<InstrumentExternalIdModelDB>(c => c.ParentInstrumentId);
         
    }
}