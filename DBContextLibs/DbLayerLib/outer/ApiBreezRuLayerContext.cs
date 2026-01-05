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
    public async Task<TPaginationResponseStandardModel<ProductViewBreezRuModeld>> ProductsSelect(BreezRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseStandardModel<ProductViewBreezRuModeld> res = new(req);

        var q = from po in Products.Include(x => x.Images)
                where (po.Manual != null && EF.Functions.ILike(po.Manual, $"%{req.FindQuery}%")) ||
                (po.VideoYoutube != null && EF.Functions.ILike(po.VideoYoutube, $"%{req.FindQuery}%")) ||
                (po.UTP != null && EF.Functions.ILike(po.UTP, $"%{req.FindQuery}%")) ||
                (po.AccessoryNC != null && EF.Functions.ILike(po.AccessoryNC, $"%{req.FindQuery}%")) ||
                (po.NarujNC != null && EF.Functions.ILike(po.NarujNC, $"%{req.FindQuery}%")) ||
                (po.Article != null && EF.Functions.ILike(po.Article, $"%{req.FindQuery}%")) ||
                (po.BimModel != null && EF.Functions.ILike(po.BimModel, $"%{req.FindQuery}%")) ||
                (po.Booklet != null && EF.Functions.ILike(po.Booklet, $"%{req.FindQuery}%")) ||
                (po.Brand != null && EF.Functions.ILike(po.Brand, $"%{req.FindQuery}%")) ||
                (po.VnutrNC != null && EF.Functions.ILike(po.VnutrNC, $"%{req.FindQuery}%")) ||
                (po.Title != null && EF.Functions.ILike(po.Title, $"%{req.FindQuery}%")) ||
                (po.Series != null && EF.Functions.ILike(po.Series, $"%{req.FindQuery}%")) ||
                (po.NC != null && EF.Functions.ILike(po.NC, $"%{req.FindQuery}%")) ||
                (po.Description != null && EF.Functions.ILike(po.Description, $"%{req.FindQuery}%"))
                join l in Leftovers on po.Article equals l.Article into grouping
                from l in grouping.DefaultIfEmpty()
                select new { product = po, leftover = l };

        res.TotalRowsCount = await q.CountAsync(token);
        res.Response = req.SortingDirection == DirectionsEnum.Up
            ? [.. (await q.OrderBy(x => x.product.Title).Skip(req.PageSize * req.PageNum).Take(req.PageSize).ToListAsync(cancellationToken: token)).Select(x => ProductViewBreezRuModeld.Build(x.product, x.leftover))]
            : [.. (await q.OrderByDescending(x => x.product.Title).Skip(req.PageSize * req.PageNum).Take(req.PageSize).ToListAsync(cancellationToken: token)).Select(x => ProductViewBreezRuModeld.Build(x.product, x.leftover))];

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
    public DbSet<TechPropertyProductBreezRuModelDB> PropsTechsProducts { get; set; }
}