////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class StockSharpAppContext(DbContextOptions<StockSharpAppContext> options) : StockSharpAppLayerContext(options)
{
    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        options
            .UseSqlite($"Filename={DbPath}");
    }
}