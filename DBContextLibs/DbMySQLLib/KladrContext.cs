////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class KladrContext(DbContextOptions<KladrContext> options, ILogger<KladrContext> logger) : KladrLayerContext(options, logger)
{
    /// <inheritdoc/>
    public override async Task EmptyTemplateTablesAsync(bool forTemplate = true)
    {
        if (forTemplate)
        {
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempStreetsKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempAltnamesKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempNamesMapsKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempObjectsKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempSocrbasesKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(TempHousesKLADR)}\"");
        }
        else
        {
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(StreetsKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(AltnamesKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(NamesMapsKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(ObjectsKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(SocrbasesKLADR)}\"");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(HousesKLADR)}\"");
        }
    }
}