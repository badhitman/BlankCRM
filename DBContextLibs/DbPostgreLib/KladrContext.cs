////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using DbLayerLib;
using SharedLib;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class KladrContext(DbContextOptions<KladrContext> options) : KladrLayerContext(options)
{
    /// <inheritdoc/>
    public override async Task EmptyTemplateTables()
    {
        await Database.ExecuteSqlAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<StreetTempKLADRModelDB>()}");
        await Database.ExecuteSqlAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<AltnameTempKLADRModelDB>()}");
        await Database.ExecuteSqlAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<NameMapTempKLADRModelDB>()}");
        await Database.ExecuteSqlAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<ObjectTempKLADRModelDB>()}");
        await Database.ExecuteSqlAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<SocrbaseTempKLADRModelDB>()}");
        await Database.ExecuteSqlAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<HouseTempKLADRModelDB>()}");
    }
}