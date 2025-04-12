////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class FeedsHaierProffRuLayerContext : DbContext
{
    /// <inheritdoc/>
    public FeedsHaierProffRuLayerContext(DbContextOptions options)
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
    public async Task<TPaginationResponseModel<ProductHaierModelDB>> ProductsSelectAsync(HaierRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseModel<ProductHaierModelDB> res = new(req);

        IQueryable<ProductHaierModelDB> q = from po in ProductsFeedsRss
                                            where EF.Functions.ILike(po.Name!, $"%{req.SimpleRequest}%") ||
                                                (po.AllArticles != null && EF.Functions.ILike(po.AllArticles, $"%{req.SimpleRequest}%")) ||
                                                (po.Category != null && EF.Functions.ILike(po.Category, $"%{req.SimpleRequest}%")) ||
                                                (po.ParentCategory != null && EF.Functions.ILike(po.ParentCategory, $"%{req.SimpleRequest}%"))
                                            select po;

        res.TotalRowsCount = await q.CountAsync(token);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ProductHaierModelDB, List<FileFeedItemHaierModelDB>?> v = q
            .Skip(req.PageSize * req.PageNum)
            .Take(req.PageSize)
            .Include(x => x.SectionsOptions)
            .Include(x => x.Files)
            ;

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ProductHaierModelDB, List<FileFeedItemHaierModelDB>?> pq(IOrderedQueryable<ProductHaierModelDB> oq)
            => oq.Skip(req.PageSize * req.PageNum)
            .Take(req.PageSize)
        .Include(x => x.SectionsOptions)
            .Include(x => x.Files);

        res.Response = req.SortingDirection == DirectionsEnum.Up
            ? await pq(q.OrderBy(x => x.Name)).ToListAsync(cancellationToken: token)
            : await pq(q.OrderByDescending(x => x.Name)).ToListAsync(cancellationToken: token);

        return res;
    }

    /// <summary>
    /// ProductsFeedsRss
    /// </summary>
    public DbSet<ProductHaierModelDB> ProductsFeedsRss { get; set; }

    /// <summary>
    /// FilesFeedsRss
    /// </summary>
    public DbSet<FileFeedItemHaierModelDB> FilesFeedsRss { get; set; }

    /// <summary>
    /// SectionOptionFeedItemHaierModelDB
    /// </summary>
    public DbSet<SectionOptionHaierModelDB> SectionsOptionsFeedsRss { get; set; }

    /// <summary>
    /// OptionsFeedsRss
    /// </summary>
    public DbSet<OptionHaierModelDB> OptionsFeedsRss { get; set; }
}