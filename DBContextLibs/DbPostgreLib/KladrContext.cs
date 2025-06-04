////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using DbLayerLib;
using SharedLib;
using Microsoft.Extensions.Logging;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class KladrContext(DbContextOptions<KladrContext> options, ILogger<KladrContext> loigger) : KladrLayerContext(options, loigger)
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "<CustomAttr>")]
    public override async Task EmptyTemplateTablesAsync(bool forTemp = true)
    {
        if (forTemp)
        {
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<StreetTempKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<AltnameTempKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<NameMapTempKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<ObjectTempKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<SocrbaseTempKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<HouseTempKLADRModelDB>()}");
        }
        else
        {
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<StreetKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<AltnameKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<NameMapKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<ObjectKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<SocrbaseKLADRModelDB>()}");
            await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {this.GetTableNameWithScheme<HouseKLADRModelDB>()}");
        }
    }
}