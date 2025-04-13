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
    /// ProductsSelect
    /// </summary>
    public async Task<TPaginationResponseModel<ProductViewBreezRuModeld>> ProductsSelect(BreezRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseModel<ProductViewBreezRuModeld> res = new(req);

        var q = from po in Products
                where (po.Manual != null && EF.Functions.ILike(po.Manual, $"%{req.SimpleRequest}%")) ||
                (po.VideoYoutube != null && EF.Functions.ILike(po.VideoYoutube, $"%{req.SimpleRequest}%")) ||
                (po.UTP != null && EF.Functions.ILike(po.UTP, $"%{req.SimpleRequest}%")) ||
                (po.AccessoryNC != null && EF.Functions.ILike(po.AccessoryNC, $"%{req.SimpleRequest}%")) ||
                (po.NarujNC != null && EF.Functions.ILike(po.NarujNC, $"%{req.SimpleRequest}%")) ||
                (po.Article != null && EF.Functions.ILike(po.Article, $"%{req.SimpleRequest}%")) ||
                (po.BimModel != null && EF.Functions.ILike(po.BimModel, $"%{req.SimpleRequest}%")) ||
                (po.Booklet != null && EF.Functions.ILike(po.Booklet, $"%{req.SimpleRequest}%")) ||
                (po.Brand != null && EF.Functions.ILike(po.Brand, $"%{req.SimpleRequest}%")) ||
                (po.VnutrNC != null && EF.Functions.ILike(po.VnutrNC, $"%{req.SimpleRequest}%")) ||
                (po.Title != null && EF.Functions.ILike(po.Title, $"%{req.SimpleRequest}%")) ||
                (po.Series != null && EF.Functions.ILike(po.Series, $"%{req.SimpleRequest}%")) ||
                (po.NC != null && EF.Functions.ILike(po.NC, $"%{req.SimpleRequest}%")) ||
                (po.Description != null && EF.Functions.ILike(po.Description, $"%{req.SimpleRequest}%"))
                join l in Leftovers on po.Article equals l.Article into grouping
                from l in grouping.DefaultIfEmpty()
                select new { product = po, leftover = l };

        res.TotalRowsCount = await q.CountAsync(token);
        res.Response = req.SortingDirection == DirectionsEnum.Up
            ? [.. (await q.OrderBy(x => x.product.Title).Skip(req.PageSize * req.PageNum).Take(req.PageSize).Include(x => x.product.Images).ToListAsync(cancellationToken: token)).Select(x => ProductViewBreezRuModeld.Build(x.product, x.leftover))]
            : [.. (await q.OrderByDescending(x => x.product.Title).Skip(req.PageSize * req.PageNum).Take(req.PageSize).Include(x => x.product.Images).ToListAsync(cancellationToken: token)).Select(x => ProductViewBreezRuModeld.Build(x.product, x.leftover))];

        return res;
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