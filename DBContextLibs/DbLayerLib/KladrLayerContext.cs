////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbLayerLib;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class KladrLayerContext : DbContext
{
    /// <inheritdoc/>
    public KladrLayerContext(DbContextOptions options)
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

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
    }

    /// <summary>
    /// Очистка временных таблиц
    /// </summary>
    public abstract Task EmptyTemplateTables(bool forTemplate = true);

    /// <summary>
    /// Переместить данные из временных таблиц в прод
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "<CustomAttr>")]
    public async Task FlushTempKladr()
    {
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await Database.BeginTransactionAsync();

        await TempAltnamesKLADR.ExecuteDeleteAsync();
        await TempNamesMapsKLADR.ExecuteDeleteAsync();
        await TempObjectsKLADR.ExecuteDeleteAsync();
        await TempSocrbasesKLADR.ExecuteDeleteAsync();
        await TempStreetsKLADR.ExecuteDeleteAsync();
        await TempHousesKLADR.ExecuteDeleteAsync();

        await Database.ExecuteSqlRawAsync($"INSERT INTO {this.GetTableNameWithScheme<StreetKLADRModelDB>()} SELECT * FROM {this.GetTableNameWithScheme<StreetTempKLADRModelDB>()}");
        await Database.ExecuteSqlRawAsync($"INSERT INTO {this.GetTableNameWithScheme<AltnameKLADRModelDB>()} SELECT * FROM {this.GetTableNameWithScheme<AltnameTempKLADRModelDB>()}");
        await Database.ExecuteSqlRawAsync($"INSERT INTO {this.GetTableNameWithScheme<NameMapKLADRModelDB>()} SELECT * FROM {this.GetTableNameWithScheme<NameMapTempKLADRModelDB>()}");
        await Database.ExecuteSqlRawAsync($"INSERT INTO {this.GetTableNameWithScheme<ObjectKLADRModelDB>()} SELECT * FROM {this.GetTableNameWithScheme<ObjectTempKLADRModelDB>()}");
        await Database.ExecuteSqlRawAsync($"INSERT INTO {this.GetTableNameWithScheme<SocrbaseKLADRModelDB>()} SELECT * FROM {this.GetTableNameWithScheme<SocrbaseTempKLADRModelDB>()}");
        await Database.ExecuteSqlRawAsync($"INSERT INTO {this.GetTableNameWithScheme<HouseKLADRModelDB>()} SELECT * FROM {this.GetTableNameWithScheme<HouseTempKLADRModelDB>()}");

        await transaction.CommitAsync();
    }


    /// <summary>
    /// Altnames содержит сведения о соответствии кодов старых и новых наименований (обозначений домов) в случаях переподчинения 
    /// и “сложного” переименования адресных объектов (когда коды записей со старым и новым наименованиями не совпадают).
    /// </summary>
    /// <remarks>
    /// Возможные варианты “сложного” переименования:
    /// улица разделилась на несколько новых улиц;
    /// несколько улиц объединились в одну новую улицу;
    /// населенный пункт стал улицей другого города(населенного пункта);
    /// улица населенного пункта стала улицей другого города(населенного пункта).
    /// В этих случаях производятся следующие действия:
    /// вводятся новые объекты в файлы Kladr.dbf, Street.dbf и Doma.dbf;
    /// старые объекты переводятся в разряд неактуальных;
    /// в файл Altnames вводятся записи, содержащие соответствие старых и новых кодов адресных объектов.
    /// </remarks>
    public DbSet<AltnameKLADRModelDB> AltnamesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<NameMapKLADRModelDB> NamesMapsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<ObjectKLADRModelDB> ObjectsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<SocrbaseKLADRModelDB> SocrbasesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<StreetKLADRModelDB> StreetsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<HouseKLADRModelDB> HousesKLADR { get; set; } = default!;



    /// <inheritdoc/>
    public DbSet<AltnameTempKLADRModelDB> TempAltnamesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<NameMapTempKLADRModelDB> TempNamesMapsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<ObjectTempKLADRModelDB> TempObjectsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<SocrbaseTempKLADRModelDB> TempSocrbasesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<StreetTempKLADRModelDB> TempStreetsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<HouseTempKLADRModelDB> TempHousesKLADR { get; set; } = default!;
}