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
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public NLogsLayerContext(DbContextOptions options)
        : base(options)
    {
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