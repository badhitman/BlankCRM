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
        modelBuilder.Entity<KladrEntry>().HasNoKey();
    }

    /// <inheritdoc/>
    public record KladrEntry(string CODE);

    /// <inheritdoc/>
    static string GetFindQuery(string findText, bool housesInclude, string[]? codeLikeFilters = null)
    {
        return $@"(SELECT o.""CODE""
                    FROM public.""{nameof(ObjectsKLADR)}"" AS o
                    WHERE {(codeLikeFilters is null || codeLikeFilters.Length == 0 ? "" : $"({string.Join(" OR ", codeLikeFilters.Select(x => $"o.\"CODE\" LIKE '{x}'"))}) AND")} ({string.IsNullOrWhiteSpace(findText)} OR o.""NAME"" LIKE '{findText}')
                    ORDER BY o.""NAME"", o.""CODE"")
                    UNION
                    (SELECT s.""CODE""
                    FROM public.""{nameof(StreetsKLADR)}"" AS s
                    WHERE {(codeLikeFilters is null || codeLikeFilters.Length == 0 || codeLikeFilters.Length == 0 ? "" : $"({string.Join(" OR ", codeLikeFilters.Select(x => $"s.\"CODE\" LIKE '{x}'"))}) AND")} ({string.IsNullOrWhiteSpace(findText)} OR s.""NAME"" LIKE '{findText}')
                    ORDER BY s.""NAME"", s.""CODE"")
                    {(!housesInclude ? "" : $@"
                    UNION
                    (SELECT h.""CODE""
                    FROM public.""{nameof(HousesKLADR)}"" AS h
                    {(codeLikeFilters is null || codeLikeFilters.Length == 0 ? "" : $@"WHERE {string.Join(" OR ", codeLikeFilters.Select(x => $"h.\"CODE\" LIKE '{x}'"))}")}                    
                    ORDER BY h.""NAME"", h.""CODE"")")}
        ";
    }

    /// <summary>
    /// Поиск объектов КЛАДР
    /// </summary>
    public async Task<int> CountByNameAsync(string? findText, bool housesInclude, string[]? codeLikeFilters = null, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(findText) || findText.Contains('\''))
            return 0;

        string query = $@"SELECT COUNT(*) as ""Value"" FROM ({GetFindQuery(findText, housesInclude, codeLikeFilters)})";
        return await Database.SqlQueryRaw<int>(query).FirstOrDefaultAsync(cancellationToken: token);
    }

    /// <summary>
    /// Поиск объектов КЛАДР
    /// </summary>
    public async Task<KladrEntry[]> FindByNameAsync(string findText, bool housesInclude, int offset, int limit = 10, string[]? codeLikeFilters = null, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(findText) || findText.Contains('\''))
            return [];

        string query = $@"SELECT r.""CODE"" FROM ({GetFindQuery(findText, housesInclude, codeLikeFilters)}) AS r LIMIT {limit} OFFSET {offset}";
        return await Set<KladrEntry>()
                        .FromSqlRaw(query).ToArrayAsync(cancellationToken: token);
    }


    /// <summary>
    /// Очистка временных таблиц
    /// </summary>
    public abstract Task EmptyTemplateTablesAsync(bool forTemplate = true);

    #region Регион
    /// <summary>
    /// 1.1 города в регионе
    /// </summary>
    public async Task<TPaginationResponseModel<ObjectKLADRModelDB>> CitiesInRegion(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<ObjectKLADRModelDB> q = ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }

    /// <summary>
    /// 1.2 нас. пункты в регионе
    /// </summary>
    public async Task<TPaginationResponseModel<ObjectKLADRModelDB>> PopPointsInRegion(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<ObjectKLADRModelDB> q = ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000000%") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }

    /// <summary>
    /// 1.3 районы в регионе
    /// </summary>
    public async Task<TPaginationResponseModel<ObjectKLADRModelDB>> AreasInRegion(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<ObjectKLADRModelDB> q = ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000________"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }

    /// <summary>
    /// 1.4 street`s в регионе
    /// </summary>
    public async Task<TPaginationResponseModel<StreetKLADRModelDB>> StreetsInRegion(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<StreetKLADRModelDB> q = StreetsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000000000______"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Район
    /// <summary>
    /// 2.1 города в районах
    /// </summary>
    public async Task<TPaginationResponseModel<ObjectKLADRModelDB>> CitiesInArea(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<ObjectKLADRModelDB> q = ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token),
        };
    }

    /// <summary>
    /// 2.2 нас. пункты в районах
    /// </summary>
    public async Task<TPaginationResponseModel<ObjectKLADRModelDB>> PopPointsInArea(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<ObjectKLADRModelDB> q = ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}000_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Город
    /// <summary>
    /// 3.1 нас. пункты в городах
    /// </summary>
    public async Task<TPaginationResponseModel<ObjectKLADRModelDB>> PopPointsInCity(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<ObjectKLADRModelDB> q = ObjectsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }

    /// <summary>
    /// 3.2 улицы в городах
    /// </summary>
    public async Task<TPaginationResponseModel<StreetKLADRModelDB>> StreetsInCity(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<StreetKLADRModelDB> q = StreetsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_________"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Нас.пункт
    /// <summary>
    /// 4.1 улицы в городах StreetKLADRModelDB
    /// </summary>
    public async Task<TPaginationResponseModel<StreetKLADRModelDB>> StreetsInPopPoint(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<StreetKLADRModelDB> q = StreetsKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}______"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Улица
    /// <summary>
    /// 5.1 дома на улицах
    /// </summary>
    public async Task<TPaginationResponseModel<HouseKLADRModelDB>> HousesInStrit(CodeKladrModel codeMd, PaginationRequestModel req, CancellationToken token = default)
    {
        IQueryable<HouseKLADRModelDB> q = HousesKLADR
            .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}{codeMd.StreetCode}____"));

        return new(req)
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.OrderBy(x => x.NAME).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
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