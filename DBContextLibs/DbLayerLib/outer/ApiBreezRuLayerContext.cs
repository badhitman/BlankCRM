////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class ApiBreezRuLayerContext : DbContext
{
    /// <inheritdoc/>
    public ApiBreezRuLayerContext(DbContextOptions options)
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

    /// <summary>
    /// остатки на складах
    /// </summary>
    public DbSet<BreezRuLeftoverModelDB> Leftovers { get; set; }

    /// <summary>
    /// Brands
    /// </summary>
    public DbSet<BrandBreezRuModelDB> Brands { get; set; }

    /// <summary>
    /// Categories
    /// </summary>
    public DbSet<CategoryBreezRuModelDB> Categories { get; set; }

    /// <summary>
    /// Products
    /// </summary>
    public DbSet<ProductBreezRuModelDB> Products { get; set; }

    /// <summary>
    /// ImagesProducts
    /// </summary>
    public DbSet<ImageProductBreezRuModelDB> ImagesProducts { get; set; }


    /// <summary>
    /// TechsCategories
    /// </summary>
    public DbSet<TechCategoryBreezRuModelDB> TechsCategories { get; set; }

    /// <summary>
    /// PropsTechsCategories
    /// </summary>
    public DbSet<TechPropertyCategoryBreezRuModelDB> PropsTechsCategories { get; set; }

    /// <summary>
    /// TechsProducts
    /// </summary>
    public DbSet<TechProductBreezRuModelDB> TechsProducts { get; set; }

    /// <summary>
    /// PropsTechsProducts
    /// </summary>
    public DbSet<PropertyTechProductBreezRuModelDB> PropsTechsProducts { get; set; }
}