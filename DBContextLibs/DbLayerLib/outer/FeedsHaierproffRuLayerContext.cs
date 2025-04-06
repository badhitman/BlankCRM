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
    /// ProductsFeedsRss
    /// </summary>
    public DbSet<FeedItemHaierModelDB> ProductsFeedsRss { get; set; }

    /// <summary>
    /// FilesFeedsRss
    /// </summary>
    public DbSet<FileFeedItemHaierModelDB> FilesFeedsRss { get; set; }

    /// <summary>
    /// SectionOptionFeedItemHaierModelDB
    /// </summary>
    public DbSet<SectionOptionFeedItemHaierModelDB> SectionsOptionsFeedsRss { get; set; }

    /// <summary>
    /// OptionsFeedsRss
    /// </summary>
    public DbSet<OptionFeedItemHaierModelDB> OptionsFeedsRss { get; set; }
}