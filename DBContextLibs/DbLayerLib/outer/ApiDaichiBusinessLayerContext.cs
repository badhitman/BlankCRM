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

    /// <summary>
    /// ProductsSelect
    /// </summary>
    public async Task<TPaginationResponseModel<ProductDaichiModelDB>> ProductsSelect(DaichiRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseModel<ProductDaichiModelDB> res = new(req);

        IQueryable<ProductDaichiModelDB> q = from po in Products
                                             where EF.Functions.ILike(po.NAME, $"%{req.FindQuery}%") ||
                                             EF.Functions.ILike(po.XML_ID, $"%{req.FindQuery}%")
                                             select po;

        res.TotalRowsCount = await q.CountAsync(token);
        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ProductDaichiModelDB, List<PriceProductDaichiModelDB>?> pq(IOrderedQueryable<ProductDaichiModelDB> oq) 
            => oq.Skip(req.PageSize * req.PageNum)
            .Take(req.PageSize)
            .Include(x => x.Params)
            .Include(x => x.StoreAvailability)
            .Include(x => x.Prices);

        res.Response = req.SortingDirection == DirectionsEnum.Up
            ? await pq(q.OrderBy(x => x.NAME)).ToListAsync(cancellationToken: token)
            : await pq(q.OrderByDescending(x => x.NAME)).ToListAsync(cancellationToken: token);

        return res;
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