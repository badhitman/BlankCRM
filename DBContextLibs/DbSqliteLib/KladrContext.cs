////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class KladrContext(DbContextOptions<KladrContext> options) : KladrLayerContext(options)
{
    /// <inheritdoc/>
    public override async Task EmptyTemplateTables()
    {
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE public.\"{nameof(TempStreetsKLADR)}\" RESTART IDENTITY RESTRICT");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE public.\"{nameof(TempAltnamesKLADR)}\" RESTART IDENTITY RESTRICT");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE public.\"{nameof(TempNamesMapsKLADR)}\" RESTART IDENTITY RESTRICT");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE public.\"{nameof(TempObjectsKLADR)}\" RESTART IDENTITY RESTRICT");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE public.\"{nameof(TempSocrbasesKLADR)}\" RESTART IDENTITY RESTRICT");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE public.\"{nameof(TempHousesKLADR)}\" RESTART IDENTITY RESTRICT");
    }
}