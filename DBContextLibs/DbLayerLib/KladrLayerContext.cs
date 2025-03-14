////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using DbLayerLib;
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

    #region Регион
    /// <summary>
    /// 1.1 города в регионе
    /// </summary>
    public async Task<ObjectKLADRModelDB[]> CitiesInRegion(string regionCode)
    {
        return await ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}000___000__") && !EF.Functions.Like(x.CODE, $"{regionCode}___000_____"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в регионе
                $" code LIKE '{codeRegion}000___000__' AND code NOT LIKE '{codeRegion}___000_____' ORDER BY name");*/
    }

    /// <summary>
    /// 1.2 нас. пункты в регионе
    /// </summary>
    public async Task<ObjectKLADRModelDB[]> PopPointsInRegion(string regionCode)
    {
        return await ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}000000_____") && !EF.Functions.Like(x.CODE, $"{regionCode}______000__"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в регионе
                $" code LIKE '{codeRegion}000000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");*/
    }

    /// <summary>
    /// 1.3 районы в регионе
    /// </summary>
    public async Task<ObjectKLADRModelDB[]> AreasInRegion(string regionCode)
    {
        return await ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}___000000__") && !EF.Functions.Like(x.CODE, $"{regionCode}000________"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'area' as typeObj FROM KLADR WHERE " + // районы в регионе
                $" code LIKE '{codeRegion}___000000__' AND code NOT LIKE '{codeRegion}000________' ORDER BY name");*/
    }

    /// <summary>
    /// 1.4 street`s в регионе
    /// </summary>
    public async Task<StreetKLADRModelDB[]> StreetsInRegion(string regionCode)
    {
        return await StreetsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}000000000______"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM STREET WHERE " + // street`s в регионе
                $" (code LIKE '{codeRegion}000000000______') ORDER BY name");*/
    }
    #endregion

    #region Район
    /// <summary>
    /// 2.1 города в районах
    /// </summary>
    public async Task<ObjectKLADRModelDB[]> CitiesInArea(string regionCode, string areaCode)
    {
        return await ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}{areaCode}___000__") && !EF.Functions.Like(x.CODE, $"{regionCode}___000_____"))//6200400000000
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*//queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в районах
                //    $" code LIKE '{codeRegion}{codeRayon}___000__' AND code NOT LIKE '{codeRegion}___000_____'  ORDER BY name");*/
    }

    /// <summary>
    /// 2.2 нас. пункты в районах
    /// </summary>
    public async Task<ObjectKLADRModelDB[]> PopPointsInArea(string regionCode, string areaCode)
    {
        return await ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}{areaCode}000_____") && !EF.Functions.Like(x.CODE, $"{regionCode}______000__"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в районах
                $" code LIKE '{codeRegion}{codeRayon}000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");*/
    }
    #endregion

    #region Город
    /// <summary>
    /// 3.1 нас. пункты в городах
    /// </summary>
    public async Task<ObjectKLADRModelDB[]> PopPointsInCity(string regionCode, string areaCode, string cityCode)
    {
        return await ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}{areaCode}{cityCode}_____") && !EF.Functions.Like(x.CODE, $"{regionCode}______000__"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в городах
                $" code LIKE '{codeRegion}{codeRayon}{codeCity}_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");*/
    }

    /// <summary>
    /// 3.2 улицы в городах
    /// </summary>
    public async Task<StreetKLADRModelDB[]> StreetsInCity(string regionCode, string areaCode, string cityCode)
    {
        return await StreetsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}{areaCode}{cityCode}_________"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
                $" code LIKE '{codeRegion}{codeRayon}{codeCity}_________' ORDER BY name");*/
    }
    #endregion

    #region Нас.пункт
    /// <summary>
    /// 4.1 улицы в городах StreetKLADRModelDB
    /// </summary>
    public async Task<StreetKLADRModelDB[]> StreetsInPopPoint(string regionCode, string areaCode, string cityCode, string popPointCode)
    {
        return await StreetsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}{areaCode}{cityCode}{popPointCode}______"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
                $" code LIKE '{codeRegion}{codeRayon}{codeCity}{codeSmallCity}______' ORDER BY name");*/
    }
    #endregion

    #region Улица
    /// <summary>
    /// 5.1 дома на улицах
    /// </summary>
    public async Task<HouseKLADRModelDB[]> HousesInStrit(string regionCode, string areaCode, string cityCode, string popPointCode, string streetCode)
    {
        return await HousesKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{regionCode}{areaCode}{cityCode}{popPointCode}{streetCode}____"))
            .OrderBy(x => x.NAME)
            .ToArrayAsync();
        /*queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'home' as typeObj FROM DOMA WHERE " + // дома на улицах
                $" code LIKE '{codeRegion}{codeRayon}{codeCity}{codeSmallCity}{codeStreet}____' ORDER BY name");*/
    }
    #endregion


    /// <summary>
    /// Переместить данные из временных таблиц в прод
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "<CustomAttr>")]
    public async Task FlushTempKladr()
    {
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await Database.BeginTransactionAsync();

        await AltnamesKLADR.ExecuteDeleteAsync();
        await NamesMapsKLADR.ExecuteDeleteAsync();
        await ObjectsKLADR.ExecuteDeleteAsync();
        await SocrbasesKLADR.ExecuteDeleteAsync();
        await StreetsKLADR.ExecuteDeleteAsync();
        await HousesKLADR.ExecuteDeleteAsync();

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
    public DbSet<SocrbaseKLADRModelDB> SocrbasesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<ObjectKLADRModelDB> ObjectsKLADR { get; set; } = default!;

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