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
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempStreetsKLADR)}\"");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempAltnamesKLADR)}\"");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempNamesMapsKLADR)}\"");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempObjectsKLADR)}\"");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempSocrbasesKLADR)}\"");
        await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempHousesKLADR)}\"");
    }
}