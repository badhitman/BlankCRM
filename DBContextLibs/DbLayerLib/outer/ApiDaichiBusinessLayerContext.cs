////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class ApiDaichiBusinessLayerContext : DbContext
{
    /// <inheritdoc/>
    public ApiDaichiBusinessLayerContext(DbContextOptions options)
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

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
    }

    /// <inheritdoc/>
    public DbSet<StoreDaichiModelDB> Stores { get; set; }

    /// <inheritdoc/>
    public DbSet<AvailabilityGoodsDaichiModelDB> AvailabilityGoods { get; set; }

    /// <inheritdoc/>
    public DbSet<PriceGoodsDaichiModelDB> PricesGoods { get; set; }

    /// <inheritdoc/>
    public DbSet<PriceDaichiModelDB> Prices { get; set; }

    /// <inheritdoc/>
    public DbSet<GoodsDaichiModelDB> Goods { get; set; }

    /// <inheritdoc/>
    public DbSet<AttributeValueDaichiModelDB> AttributesValues { get; set; }

    /// <inheritdoc/>
    public DbSet<AttributeDaichiModelDB> Attributes { get; set; }

    /// <inheritdoc/>
    public DbSet<GroupAttributeDaichiModelDB> GroupsAttributes { get; set; }

    /// <inheritdoc/>
    public DbSet<ParameterEntryDaichiModelDB> ParametersCatalog { get; set; }
}
