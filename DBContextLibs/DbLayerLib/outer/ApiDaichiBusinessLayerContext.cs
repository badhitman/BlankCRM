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
        // options.EnableSensitiveDataLogging(true);
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
    public DbSet<ProductDaichiModelDB> Products { get; set; }

    /// <inheritdoc/>
    public DbSet<ParamsProductDaichiModelDB> ParamsProducts { get; set; }

    /// <inheritdoc/>
    public DbSet<AvailabilityProductsDaichiModelDB> AvailabilityGoods { get; set; }

    /// <inheritdoc/>
    public DbSet<PriceProductDaichiModelDB> PricesProducts { get; set; }

    /// <inheritdoc/>
    public DbSet<AttributeParameterDaichiModelDB> Attributes { get; set; }

    /// <inheritdoc/>
    public DbSet<ParameterEntryDaichiModelDB> ParametersCatalog { get; set; }

    /// <inheritdoc/>
    public DbSet<SectionParameterDaichiModelDB> SectionsParameters { get; set; }

    /// <inheritdoc/>
    public DbSet<PhotoParameterDaichiModelDB> PhotosParameters { get; set; }
}