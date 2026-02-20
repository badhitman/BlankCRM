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
    /// ProductsSelect
    /// </summary>
    public async Task<TPaginationResponseStandardModel<ProductRusklimatModelDB>> ProductsSelect(RusklimatRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseStandardModel<ProductRusklimatModelDB> res = new(req);

        IQueryable<ProductRusklimatModelDB> q = from po in Products
                                                where EF.Functions.ILike(po.Name!, $"%{req.FindQuery}%") ||
                                                (po.Brand != null && EF.Functions.ILike(po.Brand, $"%{req.FindQuery}%")) ||
                                                (po.NSCode != null && EF.Functions.ILike(po.NSCode, $"%{req.FindQuery}%")) ||
                                                (po.VendorCode != null && EF.Functions.ILike(po.VendorCode, $"%{req.FindQuery}%"))
                                                select po;

        res.TotalRowsCount = await q.CountAsync(token);
        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ProductRusklimatModelDB, List<ProductPropertyRusklimatModelDB>?> pq(IOrderedQueryable<ProductRusklimatModelDB> oq)
            => oq.Skip(req.PageSize * req.PageNum)
            .Take(req.PageSize)
            .Include(x => x.Information)
            .Include(x => x.Remains)
            .Include(x => x.Properties);

        res.Response = req.SortingDirection == DirectionsEnum.Up
            ? await pq(q.OrderBy(x => x.Name)).ToListAsync(cancellationToken: token)
            : await pq(q.OrderByDescending(x => x.Name)).ToListAsync(cancellationToken: token);

        return res;
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