////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public partial class IndexingServiceLayerContext : DbContext
{
    /// <inheritdoc/>
    public IndexingServiceLayerContext(DbContextOptions options)
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
//#if DEBUG
//        options.EnableSensitiveDataLogging(true);
//        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
//#endif
    }

    #region Word
    /// <inheritdoc/>
    public DbSet<ParagraphWordIndexFileModelDB> ParagraphsWordIndexesFiles { get; set; } = default!;


    /// <inheritdoc/>
    public DbSet<TableWordIndexFileModelDB> TablesWordIndexesFiles { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<CellTableWordIndexFileModelDB> DataTablesWordIndexesFiles { get; set; } = default!;
    #endregion

    #region Excel
    /// <inheritdoc/>
    public DbSet<SheetExcelIndexFileModelDB> SheetsExcelIndexesFiles { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<CellTableExcelIndexFileModelDB> DataTablesExcelIndexesFiles { get; set; } = default!;
    #endregion
}
