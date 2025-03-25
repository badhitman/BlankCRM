////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using SharedLib;
using DbcLib;

namespace KladrService;

/// <inheritdoc/>
public class KladrNavigationServiceImpl(IDbContextFactory<KladrContext> kladrDbFactory) : IKladrNavigationService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<KladrResponseModel>> ObjectGetAsync(KladrsRequestBaseModel req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req.Code))
            throw new NotImplementedException();

        CodeKladrModel codeParse = CodeKladrModel.Build(req.Code);

        ConcurrentDictionary<KladrTypesObjectsEnum, RootKLADRModelDB> dataResponse = [];
        List<(int Level, string Socr)> socrBases = [];

        SocrbaseKLADRModelDB[] socrBasesDb;
        List<RootKLADRModelDB> dataList = [];
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync(token);

        if (codeParse.Level == KladrTypesObjectsEnum.RootRegion) // если это регион, тогда одним запросом получаем данные и переходим к ответу
        {
            var dbRaw = await (from x in context.ObjectsKLADR.Where(x => x.CODE == codeParse.CodeOrigin)
                               join y in context.SocrbasesKLADR on
                                  new
                                  {
                                      Key1 = "1",
                                      Key2 = x.SOCR,
                                  }
                                  equals
                                  new
                                  {
                                      Key1 = y.LEVEL,
                                      Key2 = y.SOCRNAME,
                                  }
                             into result
                               from r in result.DefaultIfEmpty()
                               select new { Region = x, Socr = r })
                        .FirstAsync();

            dataList.Add(dbRaw.Region);
            socrBasesDb = [dbRaw.Socr];
            goto fin;
        }

        List<Task> tasks =
        [
            Task.Run(async () => {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB _db = await context.ObjectsKLADR.FirstAsync(x => x.CODE == $"{codeParse.RegionCode}00000000000");
                dataResponse.TryAdd(KladrTypesObjectsEnum.RootRegion, _db);
                lock(socrBases)
                    {
                        socrBases.Add((1, _db.SOCR));
                    }
            }, token),
        ];

        if (codeParse.AreaCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}000000{(codeParse.Level == KladrTypesObjectsEnum.Area ? codeParse.SignOfRelevanceCode : "00")}");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Area, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((2, _db.SOCR));
                    }
                }
            }, token));

        if (codeParse.CityCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}000{(codeParse.Level == KladrTypesObjectsEnum.City ? codeParse.SignOfRelevanceCode : "00")}");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.City, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((3, _db.SOCR));
                    }
                }
            }, token));

        if (codeParse.PopPointCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}{(codeParse.Level == KladrTypesObjectsEnum.PopPoint ? codeParse.SignOfRelevanceCode : "00")}");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.PopPoint, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((4, _db.SOCR));
                    }
                }
            }, token));

        if (!string.IsNullOrWhiteSpace(codeParse.StreetCode) && codeParse.StreetCode != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB? _db = await context.StreetsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}{codeParse.StreetCode}{(codeParse.Level == KladrTypesObjectsEnum.Street ? codeParse.SignOfRelevanceCode : "00")}");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Street, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((5, _db.SOCR));
                    }
                }
            }, token));

        if (!string.IsNullOrWhiteSpace(codeParse.HomeCode) && codeParse.HomeCode != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB? _db = await context.HousesKLADR.FirstOrDefaultAsync(x => x.CODE == codeParse.CodeOrigin);
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Home, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((6, _db.SOCR));
                    }
                }
            }, token));

        await Task.WhenAll(tasks);

        string[] socrBasesFilter = [.. socrBases.Select(x => x.Socr).Distinct()];
        socrBasesDb = await context.SocrbasesKLADR.Where(x => socrBasesFilter.Contains(x.SOCRNAME)).ToArrayAsync(cancellationToken: token);

        dataList.AddRange(dataResponse.OrderBy(x => x.Key).Select(x => x.Value));

    fin:
        RootKLADRModelDB mainElement = dataList.Last();
        return new()
        {
            Response = new()
            {
                Name = mainElement.NAME,
                Code = mainElement.CODE,
                Socr = mainElement.SOCR,
                Payload = JObject.FromObject(mainElement),
                Parents = dataList.Count == 1 ? null : [.. dataList.Take(dataList.Count - 1)],
                Socrbases = socrBasesDb,
                GNINMB = mainElement.GNINMB,
                OCATD = mainElement.OCATD,
                UNO = mainElement.UNO,
            }
        };
    }

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParentAsync(KladrsRequestBaseModel req, CancellationToken token = default)
    {
        List<Task> tasks = [];
        Dictionary<KladrChainTypesEnum, JObject[]> res = [];
        if (string.IsNullOrWhiteSpace(req.Code))
        {
            List<ObjectKLADRModelDB> dataDb = default!;
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                dataDb = await context
                    .ObjectsKLADR
                    .Where(x => EF.Functions.Like(x.CODE, "__000000000__"))
                    .OrderBy(x => x.NAME)
                    .ToListAsync();
            }, token));

            await Task.WhenAll(tasks);
            res.Add(KladrChainTypesEnum.RootRegions, [.. dataDb.Select(JObject.FromObject)]);

            return res;
        }

        CodeKladrModel codeMd = CodeKladrModel.Build(req.Code);
        ConcurrentDictionary<KladrChainTypesEnum, JObject[]> dataResponse = [];
        PaginationRequestModel _sq = new() { PageNum = 0, PageSize = int.MaxValue };

        if (codeMd.Level == KladrTypesObjectsEnum.RootRegion) // регионы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.CitiesInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в регионе
                //    $" code LIKE '{codeRegion}000___000__' AND code NOT LIKE '{codeRegion}___000_____' ORDER BY name");
            }, token));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в регионе
                //    $" code LIKE '{codeRegion}000000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }, token));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.AreasInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.AreasInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'area' as typeObj FROM KLADR WHERE " + // районы в регионе
                //    $" code LIKE '{codeRegion}___000000__' AND code NOT LIKE '{codeRegion}000________' ORDER BY name");
            }, token));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM STREET WHERE " + // street`s в регионе
                //    $" (code LIKE '{codeRegion}000000000______') ORDER BY name");
            }, token));
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Area) //районы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInArea(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.CitiesInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в районах
                //    $" code LIKE '{codeRegion}{codeRayon}___000__' AND code NOT LIKE '{codeRegion}___000_____'  ORDER BY name");
            }, token));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInArea(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в районах
                //    $" code LIKE '{codeRegion}{codeRayon}000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }, token));
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.City) // города в районах
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInCity(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + $"_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }, token));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInCity(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + "_________' ORDER BY name");
            }, token));
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.PopPoint) // нас пункты
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInPopPoint(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInPopPoint, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в нас. пунктах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + codeSmallCity + "______' ORDER BY name");
            }, token));
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Street) // улицы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB[] data = await context.HousesInStrit(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.HousesInStreet, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'home' as typeObj FROM DOMA WHERE " + // дома на улицах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + codeSmallCity + codeStreet + "____' ORDER BY name");
            }, token));
        }
        await Task.WhenAll(tasks);

        foreach (KeyValuePair<KladrChainTypesEnum, JObject[]> v in dataResponse.OrderBy(x => x.Key))
            res.Add(v.Key, v.Value);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelectAsync(KladrSelectRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseModel<KladrResponseModel> response = new(req) { Response = [] };

        SocrbaseKLADRModelDB[] socrbases = default!;
        List<ObjectKLADRModelDB> dataDb = default!;

        using KladrContext context = await kladrDbFactory.CreateDbContextAsync(token);

        if (string.IsNullOrWhiteSpace(req.CodeLikeFilter))
        {
            socrbases = await context
                    .SocrbasesKLADR
                    .Where(x => x.LEVEL == "1")
                    .OrderBy(x => x.SCNAME)
                    .ToArrayAsync(cancellationToken: token);

            IQueryable<ObjectKLADRModelDB> q = context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, "__000000000__"));

            response.TotalRowsCount = await q.CountAsync(cancellationToken: token);
            dataDb = await q
                .OrderBy(x => x.NAME)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync(cancellationToken: token);

            response.Response.AddRange(dataDb.Select(x => new KladrResponseModel()
            {
                Payload = JObject.FromObject(x),
                Socrbases = [.. socrbases.Where(y => y.SOCRNAME == x.SOCR)],
                Name = x.NAME,
                Code = x.CODE,
                Socr = x.SOCR,
                GNINMB = x.GNINMB,
                OCATD = x.OCATD,
                UNO = x.UNO,
            }));
            return response;
        }

        Dictionary<KladrChainTypesEnum, string[]> dataResponse = [];
        CodeKladrModel codeMd = CodeKladrModel.Build(req.CodeLikeFilter);
        string[] dbRows;

        if (codeMd.Level == KladrTypesObjectsEnum.RootRegion) // регионы
        {
            response.TotalRowsCount = await context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000000%") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000________"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Union(context.StreetsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000000000______"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .CountAsync(cancellationToken: token);

            dbRows = await context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000000%") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000________"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Union(context.StreetsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}000000000______"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync(cancellationToken: token);
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Area) //районы
        {
            response.TotalRowsCount = await context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"))
                .Select(x => x.CODE)
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}000_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .Select(x => x.CODE))
                .CountAsync(cancellationToken: token);

            dbRows = await context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}000_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync(cancellationToken: token);
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.City) // города в районах
        {
            response.TotalRowsCount = await context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .Select(x => x.CODE)
                .Union(context.StreetsKLADR.Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_________"))
                .Select(x => x.CODE))
                .CountAsync(cancellationToken: token);

            dbRows = await context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Union(context.StreetsKLADR.Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_________"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE))
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync(cancellationToken: token);
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.PopPoint) // нас пункты
        {
            response.TotalRowsCount = await context
                .StreetsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}______"))
                .CountAsync(cancellationToken: token);

            dbRows = await context
                .StreetsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}______"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync(cancellationToken: token);
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Street) // улицы
        {
            response.TotalRowsCount = await context
                .HousesKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}{codeMd.StreetCode}____"))
                .CountAsync(cancellationToken: token);

            dbRows = await context
                .HousesKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}{codeMd.StreetCode}____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync(cancellationToken: token);
        }
        else
            dbRows = [];

        List<KladrResponseModel> fullData = [];
        await Task.WhenAll(dbRows.Select(x => Task.Run(async () =>
        {
            TResponseModel<KladrResponseModel> objDb = await ObjectGetAsync(new() { Code = x });
            if (objDb.Response is not null)
                lock (fullData)
                {
                    fullData.Add(objDb.Response);
                }
        })));

        response.Response.AddRange(fullData.OrderBy(x => x.Chain).ThenBy(x => x.Name));
        return response;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsFindAsync(KladrFindRequestModel req, CancellationToken token = default)
    {
        TPaginationResponseModel<KladrResponseModel> response = new(req) { Response = [] };

        if (string.IsNullOrWhiteSpace(req.FindText))
            return response;

        using KladrContext context = await kladrDbFactory.CreateDbContextAsync(token);
        IQueryable<string> q = context
            .ObjectsKLADR
            .Where(x => EF.Functions.Like(x.NAME, req.FindText) && (req.CodeLikeFilter == null || req.CodeLikeFilter.Length == 0 || req.CodeLikeFilter.Any(y => EF.Functions.Like(x.CODE, y))))

            .Select(x => x.CODE)
            .Union(context
            .StreetsKLADR
            .Where(x => EF.Functions.Like(x.NAME, req.FindText) && (req.CodeLikeFilter == null || req.CodeLikeFilter.Length == 0 || req.CodeLikeFilter.Any(y => EF.Functions.Like(x.CODE, y))))

            .Select(x => x.CODE))
            ;

        response.TotalRowsCount = await q.CountAsync(cancellationToken: token);
        string[] dbRows;

        dbRows = [.. (await context.FindByName(req.FindText, req.PageNum * req.PageSize, req.PageSize, req.CodeLikeFilter, token)).Select(x => x.CODE)];
        //.ObjectsKLADR
        //.Where(x => EF.Functions.Like(x.NAME, req.FindText) && (req.CodeLikeFilter == null || req.CodeLikeFilter.Length == 0 || req.CodeLikeFilter.Any(y => EF.Functions.Like(x.CODE, y))))
        //.OrderBy(x => x.NAME)
        //.ThenBy(x => x.CODE)
        //.Select(x => x.CODE)
        //.Union(context
        //.StreetsKLADR
        //.Where(x => EF.Functions.Like(x.NAME, req.FindText) && (req.CodeLikeFilter == null || req.CodeLikeFilter.Length == 0 || req.CodeLikeFilter.Any(y => EF.Functions.Like(x.CODE, y))))
        //.OrderBy(x => x.NAME)
        //.ThenBy(x => x.CODE)
        //.Select(x => x.CODE))
        //.Skip(req.PageNum * req.PageSize)
        //.Take(req.PageSize)
        //.ToArrayAsync();


        List<KladrResponseModel> fullData = [];
        await Task.WhenAll(dbRows.Select(x => Task.Run(async () =>
        {
            TResponseModel<KladrResponseModel> objDb = await ObjectGetAsync(new() { Code = x });
            if (objDb.Response is not null)
                lock (fullData)
                {
                    fullData.Add(objDb.Response);
                }
        })));

        response.Response.AddRange(fullData.OrderBy(x => x.Chain).ThenBy(x => x.Name));
        return response;
    }
}

internal record KladrElementSelectRecord(KladrChainTypesEnum Chain, string CODE);