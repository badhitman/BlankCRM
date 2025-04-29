////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class NLogsLayerContext : DbContext
{
    /// <summary>
    /// FileName
    /// </summary>
    private static readonly string _ctxName = nameof(NLogsLayerContext);

    /// <summary>
    /// db Path
    /// </summary>
    public static string DbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _ctxName, $"{(AppDomain.CurrentDomain.FriendlyName.Equals("ef", StringComparison.OrdinalIgnoreCase) ? "NLogsData" : AppDomain.CurrentDomain.FriendlyName)}.db3");


    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public NLogsLayerContext(DbContextOptions options)
        : base(options)
    {
        FileInfo _fi = new(DbPath);

        if (_fi.Directory?.Exists != true)
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);
        //#if DEBUG
        //  Database.EnsureCreated();
        //#else
        Database.Migrate();
        //#endif
    }
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NLogRecordModelDB>().HasIndex(p => p.RecordTime);
        modelBuilder.Entity<NLogRecordModelDB>().HasIndex(p => p.ApplicationName);
        modelBuilder.Entity<NLogRecordModelDB>().HasIndex(p => p.ContextPrefix);
        modelBuilder.Entity<NLogRecordModelDB>().HasIndex(p => p.RecordLevel);
        modelBuilder.Entity<NLogRecordModelDB>().HasIndex(p => p.Logger);
        modelBuilder.Entity<NLogRecordModelDB>().HasKey(p => p.Id);

    }
    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
#if DEBUG
        options.EnableSensitiveDataLogging(true);
        //options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
    }

    /// <summary>
    /// Логи
    /// </summary>
    public DbSet<NLogRecordModelDB> Logs { get; set; } = default!;
}