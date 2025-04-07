////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class ApiRusklimatComLayerContext : DbContext
{
    /// <inheritdoc/>
    public ApiRusklimatComLayerContext(DbContextOptions options)
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

    /// <summary>
    /// Товары
    /// </summary>
    public DbSet<ProductRusklimatModelDB> Products { get; set; }

    /// <summary>
    /// Свойства товаров
    /// </summary>
    public DbSet<PropertyRusklimatModelDB> PropertiesCatalog { get; set; }

    /// <summary>
    /// Единицы измерения
    /// </summary>
    public DbSet<UnitRusklimatModelDB> Units { get; set; }

    /// <summary>
    /// Категории
    /// </summary>
    public DbSet<CategoryRusklimatModelDB> Categories { get; set; }

    /// <summary>
    /// Свойства товаров
    /// </summary>
    public DbSet<ProductPropertyRusklimatModelDB> ProductsProperties { get; set; }

    /// <summary>
    /// Информация для товаров
    /// </summary>
    /// <remarks>
    /// массивы строк (ссылки или идентификаторы)
    /// </remarks>
    public DbSet<ProductInformationRusklimatModelDB> ProductsInformation { get; set; }

    /// <summary>
    /// Остатки на складах
    /// </summary>
    public DbSet<WarehouseRemainsRusklimatModelDB> WarehousesRemains { get; set; }

    /// <summary>
    /// Остатки
    /// </summary>
    public DbSet<RemainsRusklimatModelDB> Remains { get; set; }
}