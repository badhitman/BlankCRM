////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public partial class StorageLayerContext : DbContext
{
    /// <inheritdoc/>
    public StorageLayerContext(DbContextOptions options)
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

    /// <summary>
    /// Параметры
    /// </summary>
    public DbSet<StorageCloudParameterModelDB> CloudProperties { get; set; } = default!;

    /// <summary>
    /// Тэги
    /// </summary>
    public DbSet<TagModelDB> CloudTags { get; set; } = default!;


    /// <summary>
    /// Файлы
    /// </summary>
    public DbSet<StorageFileModelDB> CloudFiles { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<AccessFileRuleModelDB> RulesFilesAccess { get; set; } = default!;

    #region Word
    /// <inheritdoc/>
    public DbSet<ParagraphWordIndexFileModel> ParagraphsWordIndexesFiles { get; set; } = default!;


    /// <inheritdoc/>
    public DbSet<TableWordIndexFileModel> TablesWordIndexesFiles { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<CellTableWordIndexFileModel> DataTablesWordIndexesFiles { get; set; } = default!;
    #endregion

    #region Excel
    /// <inheritdoc/>
    public DbSet<SheetExcelIndexFileModel> SheetsExcelIndexesFiles { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<CellTableExcelIndexFileModel> DataTablesExcelIndexesFiles { get; set; } = default!;
    #endregion
}