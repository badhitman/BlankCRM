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
    public async Task<TResponseModel<KladrResponseModel>> ObjectGet(KladrsRequestBaseModel req)
    {
        if (string.IsNullOrWhiteSpace(req.Code))
            throw new NotImplementedException();

        CodeKladrModel codeParse = CodeKladrModel.Build(req.Code);
        ConcurrentDictionary<KladrTypesObjectsEnum, RootKLADRModelDB> dataResponse = [];

        List<(int Level, string Socr)> socrBases = [];
        int lvl = 1;
        List<Task> tasks =
        [
            Task.Run(async () => {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB _db = await context.ObjectsKLADR.FirstAsync(x => x.CODE == $"{codeParse.RegionCode}00000000000");
                dataResponse.TryAdd(KladrTypesObjectsEnum.RootRegion, _db);
                lock(socrBases)
                    {
                        socrBases.Add((lvl++, _db.SOCR));
                    }
            }),
        ];

        if (codeParse.Level > KladrTypesObjectsEnum.RootRegion && codeParse.AreaCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE != codeParse.CodeOrigin && x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}00000000");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Area, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((lvl++, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.Area && codeParse.CityCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE != codeParse.CodeOrigin && x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}00000");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.City, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((lvl++, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.City && codeParse.PopPointCode != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE != codeParse.CodeOrigin && x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}00");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.PopPoint, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((lvl++, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.PopPoint && !string.IsNullOrWhiteSpace(codeParse.StreetCode) && codeParse.StreetCode != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB? _db = await context.StreetsKLADR.FirstOrDefaultAsync(x => x.CODE != codeParse.CodeOrigin && x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}{codeParse.StreetCode}00");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Street, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((lvl++, _db.SOCR));
                    }
                }
            }));

        if (codeParse.Level > KladrTypesObjectsEnum.Street && !string.IsNullOrWhiteSpace(codeParse.HomeCode) && codeParse.HomeCode != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB? _db = await context.HousesKLADR.FirstOrDefaultAsync(x => x.CODE != codeParse.CodeOrigin && x.CODE == $"{codeParse.RegionCode}{codeParse.AreaCode}{codeParse.CityCode}{codeParse.PopPointCode}{codeParse.StreetCode}{codeParse.HomeCode}");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Home, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((lvl++, _db.SOCR));
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
                Chain = codeParse.Chain,
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
                    .Where(x => EF.Functions.Like(x.CODE, "__000000000__"))
                    .OrderBy(x => x.NAME)
                    .ToListAsync();
            }));

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
                .Where(x => EF.Functions.Like(x.CODE,"__000000000__"));

            response.TotalRowsCount = await q.CountAsync();
            dataDb = await q
                .OrderBy(x => x.NAME)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            response.Response.AddRange(dataDb.Select(x => new KladrResponseModel()
            {
                Payload = JObject.FromObject(x),
                Socrbases = [.. socrbases.Where(y => y.SOCRNAME == x.SOCR)],
                Chain = CodeKladrModel.Build(x.CODE).Chain,
            }));
            return response;
        }

        Dictionary<KladrChainTypesEnum, string[]> dataResponse = [];
        CodeKladrModel codeMd = CodeKladrModel.Build(req.CodeLikeFilter);
        string[] dbRows;
        IQueryable<string> query;
        if (codeMd.Level == KladrTypesObjectsEnum.RootRegion) // регионы
        {
            query = context
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
                .Select(x => x.CODE));

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
                .ToArrayAsync();
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Area) //районы
        {//0100700000000
            query = context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Union(context.ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}000_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE));

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
                .ToArrayAsync();
#if DEBUG
            //string[] v1 = await context
            //    .ObjectsKLADR
            //    .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}___000__") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}___000_____"))
            //    .OrderBy(x => x.NAME)
            //    .Select(x => x.CODE)
            //    .Skip(req.PageNum * req.PageSize)
            //    .Take(req.PageSize)
            //    .ToArrayAsync();

            //var v2 = await context
            //    .ObjectsKLADR                
            //    .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}000_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
            //    .OrderBy(x => x.NAME)
            //    .Select(x => x.CODE)
            //    .Skip(req.PageNum * req.PageSize)
            //    .Take(req.PageSize)
            //    .ToArrayAsync();
            //var v3 = v1;
#endif
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.City) // города в районах
        {
            query = context
                .ObjectsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_____") && !EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}______000__"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Union(context.StreetsKLADR.Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}_________"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE));

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
                .ToArrayAsync();
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.PopPoint) // нас пункты
        {
            query = context
                .StreetsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}______"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE);

            dbRows = await context
                .StreetsKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}______"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync();
        }
        else if (codeMd.Level == KladrTypesObjectsEnum.Street) // улицы
        {
            query = context
                .HousesKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}{codeMd.StreetCode}____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE);

            dbRows = await context
                .HousesKLADR
                .Where(x => EF.Functions.Like(x.CODE, $"{codeMd.RegionCode}{codeMd.AreaCode}{codeMd.CityCode}{codeMd.PopPointCode}{codeMd.StreetCode}____"))
                .OrderBy(x => x.NAME)
                .Select(x => x.CODE)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToArrayAsync();
        }
        else
        {
            query = Array.Empty<string>().AsQueryable();
            dbRows = [];
        }

        response.TotalRowsCount = await query.CountAsync();

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
        response.Response.AddRange(fullData.OrderBy(x => x.Response?.Chain).Select(x => x.Response!));

        return response;
    }
}

internal record KladrElementSelectRecord(KladrChainTypesEnum Chain, string CODE);