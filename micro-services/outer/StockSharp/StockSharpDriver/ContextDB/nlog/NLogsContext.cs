////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class NLogsContext(DbContextOptions<NLogsContext> options) : NLogsLayerContext(options)
{
    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        options

#if DEBUG
            .UseSqlite(@"Data Source=c:\Users\User\source\repos\BlankCRM\micro-services\outer\StockSharp\StockSharpDriver\bin\Debug\net6.0\logs-database.db3;");
#else
            .UseSqlite($"Data Source=logs-database.db3;");
#endif

    }
}