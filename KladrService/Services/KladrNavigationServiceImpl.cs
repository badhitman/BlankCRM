////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using SharedLib;
using DbcLib;
using System.Linq;

namespace KladrService;

/// <inheritdoc/>
public class KladrNavigationServiceImpl(IDbContextFactory<KladrContext> kladrDbFactory) : IKladrNavigationService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<KladrResponseModel>> ObjectGet(KladrsRequestBaseModel req)
    {
        if (string.IsNullOrWhiteSpace(req.Code))
            throw new NotImplementedException();

        CodeKladrModel codeParse = GlobalTools.ParseKladrTypeObject(req.Code);

        ConcurrentDictionary<KladrTypesObjectsEnum, RootKLADRModelDB> dataResponse = [];

        List<(int Level, string Socr)> socrBases = [];

        List<Task> tasks =
        [
            Task.Run(async () => {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB _db = await context.ObjectsKLADR.FirstAsync(x=>x.CODE == $"{codeParse.RegionCode}00000000000");
                dataResponse.TryAdd(KladrTypesObjectsEnum.RootRegion, _db);
                lock(socrBases)
                    {
                        socrBases.Add((1, _db.SOCR));
                    }
            }),
        ];

        if (codeParse.Level > KladrTypesObjectsEnum.RootRegion && codeParse.AreaCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}00000000");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Area, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((2, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.Area && codeParse.CityCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}00000");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.City, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((3, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.City && codeParse.PopPointCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}00");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.PopPoint, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((4, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.PopPoint && !string.IsNullOrWhiteSpace(codeParse.StreetCode) && codeParse.StreetCode != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB? _db = await context.StreetsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}{codeParse.StreetCode}00");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Street, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((5, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.Street && !string.IsNullOrWhiteSpace(codeParse.HomeCode) && codeParse.HomeCode != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB? _db = await context.HousesKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}{codeParse.StreetCode}{codeParse.HomeCode}");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Home, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((6, _db.SOCR));
                    }
                }
            }));

        await Task.WhenAll(tasks);
        List<RootKLADRModelDB> dataList = [.. dataResponse.OrderBy(x => x.Key).Select(x => x.Value)];
        string[] socrBasesFilter = [.. socrBases.Select(x => x.Socr).Distinct()];
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
        SocrbaseKLADRModelDB[] resDb = await context.SocrbasesKLADR.Where(x => socrBasesFilter.Contains(x.SOCRNAME)).ToArrayAsync();

        return new()
        {
            Response = new()
            {
                Payload = JObject.FromObject(dataList.Last()),
                Parents = dataList.Count == 1 ? null : [.. dataList.Take(dataList.Count - 1)],
                Socrbases = resDb,
            }
        };
    }

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParent(KladrsRequestBaseModel req)
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
                    .Where(x => x.CODE.EndsWith("00000000000"))
                    .OrderBy(x => x.NAME)
                    .ToListAsync();
            }));

            await Task.WhenAll(tasks);
            res.Add(KladrChainTypesEnum.RootRegions, [.. dataDb.Select(JObject.FromObject)]);

            return res;
        }

        ConcurrentDictionary<KladrChainTypesEnum, JObject[]> dataResponse = [];
        PaginationRequestModel _sq = new() { PageNum = 0, PageSize = int.MaxValue };
        CodeKladrModel codeMd = GlobalTools.ParseKladrTypeObject(req.Code);
        if (codeMd.Level == KladrTypesObjectsEnum.RootRegion) // регионы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.CitiesInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в регионе
                //    $" code LIKE '{codeRegion}000___000__' AND code NOT LIKE '{codeRegion}___000_____' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в регионе
                //    $" code LIKE '{codeRegion}000000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.AreasInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.AreasInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'area' as typeObj FROM KLADR WHERE " + // районы в регионе
                //    $" code LIKE '{codeRegion}___000000__' AND code NOT LIKE '{codeRegion}000________' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInRegion(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM STREET WHERE " + // street`s в регионе
                //    $" (code LIKE '{codeRegion}000000000______') ORDER BY name");
            }));
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
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInArea(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в районах
                //    $" code LIKE '{codeRegion}{codeRayon}000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
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
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInCity(codeMd, _sq);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + "_________' ORDER BY name");
            }));
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
            }));
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
            }));
        }
        await Task.WhenAll(tasks);

        foreach (KeyValuePair<KladrChainTypesEnum, JObject[]> v in dataResponse.OrderBy(x => x.Key))
            res.Add(v.Key, v.Value);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelect(KladrSelectRequestModel req)
    {
        TPaginationResponseModel<KladrResponseModel> response = new(req) { Response = [] };
        SocrbaseKLADRModelDB[] socrbases = default!;
        List<ObjectKLADRModelDB> dataDb = default!;
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();

        if (string.IsNullOrWhiteSpace(req.CodeLikeFilter))
        {
            socrbases = await context
                    .SocrbasesKLADR
                    .Where(x => x.LEVEL == "1")
                    .OrderBy(x => x.SCNAME)
                    .ToArrayAsync();

            IQueryable<ObjectKLADRModelDB> q = context
                .ObjectsKLADR
                .Where(x => x.CODE.EndsWith("00000000000"));

            response.TotalRowsCount = await q.CountAsync();
            dataDb = await q
                .OrderBy(x => x.NAME)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            response.Response.AddRange(dataDb.Select(x => new KladrResponseModel()
            {
                Payload = JObject.FromObject(x),
                Socrbases = [.. socrbases.Where(y => y.SOCRNAME == x.SOCR)]
            }));
            return response;
        }

        Dictionary<KladrChainTypesEnum, string[]> dataResponse = [];
        CodeKladrModel codeMd = GlobalTools.ParseKladrTypeObject(req.CodeLikeFilter);
        string[] dbRows;
        IQueryable<string> query;
        if (codeMd.Level == KladrTypesObjectsEnum.RootRegion) // регионы
        {
            query = context
                .CitiesInRegionQuery(codeMd).Select(x => x.CODE)
                .Union(context.PopPointsInRegionQuery(codeMd).Select(x => x.CODE))
                .Union(context.AreasInRegionQuery(codeMd).Select(x => x.CODE))
                .Union(context.StreetsInRegionQuery(codeMd).Select(x => x.CODE))
                ;
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Area) //районы
        {
            query = context
                .CitiesInAreaQuery(codeMd).Select(x => x.CODE)
                .Union(context.PopPointsInAreaQuery(codeMd).Select(x => x.CODE));
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.City) // города в районах
        {
            query = context
                .PopPointsInCityQuery(codeMd).Select(x => x.CODE)
                .Union(context.StreetsInCityQuery(codeMd).Select(x => x.CODE));
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.PopPoint) // нас пункты
        {
            query = context
                .StreetsInPopPointQuery(codeMd).Select(x => x.CODE);
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Street) // улицы
        {
            query = context
                .HousesInStritQuery(codeMd).Select(x => x.CODE);
        }
        else
            query = Array.Empty<string>().AsQueryable();

        response.TotalRowsCount = await query.CountAsync();

        dbRows = await query
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize)
            .ToArrayAsync();

        List<TResponseModel<KladrResponseModel>> fullData = [];
        await Task.WhenAll(dbRows.Select(x => Task.Run(async () =>
        {
            TResponseModel<KladrResponseModel> objDb = await ObjectGet(new() { Code = x });
            lock (fullData)
            {
                fullData.Add(objDb);
            }
        })));

        fullData.RemoveAll(x => x.Response is null);
        response.Response.AddRange(fullData.Select(x => x.Response!));

        /*
         response.Response.AddRange(dataDb.Select(x => new KladrResponseModel()
            {
                ChainType = KladrChainTypesEnum.RootRegions,
                TypeObject = KladrTypesObjectsEnum.RootRegion,
                Payload = JObject.FromObject(x),
                Socrbase = [.. socrbases.Where(y => y.SOCRNAME == x.SOCR)]
            }));
         */

        return response;
    }
}

internal record KladrElementSelectRecord(KladrChainTypesEnum Chain, string CODE);