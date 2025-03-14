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

        string codeObject = req.Code;
        string codeRegion = codeObject[..2];
        string codeRayon = codeObject.Substring(2, 3);
        string codeCity = codeObject.Substring(5, 3);
        string codeSmallCity = codeObject.Substring(8, 3);

        string codeStreet = codeObject.Length < 17 ? "" : codeObject.Substring(11, 4);
        string codeHome = codeObject.Length < 19 ? "" : codeObject.Substring(15, 4);

        ConcurrentDictionary<KladrTypesObjectsEnum, RootKLADRModelDB> dataResponse = [];

        List<(int, string)> socrBases = [];

        List<Task> tasks =
        [
            Task.Run(async () => {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB _db = await context.ObjectsKLADR.FirstAsync(x=>x.CODE == $"{codeRegion}00000000000");
                dataResponse.TryAdd(KladrTypesObjectsEnum.RootRegion, _db);
                lock(socrBases)
                    {
                        socrBases.Add((1, _db.SOCR));
                    }
            }),
        ];

        (KladrTypesObjectsEnum Level, KladrChainTypesEnum Chain) objectLevel = GlobalTools.ParseKladrTypeObject(codeObject);

        if (objectLevel.Level > KladrTypesObjectsEnum.RootRegion && codeRayon != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeRegion}{codeRayon}00000000");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Area, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((2, _db.SOCR));
                    }
                }
            }));

        if (objectLevel.Level > KladrTypesObjectsEnum.Area && codeCity != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeRegion}{codeRayon}{codeCity}00000");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.City, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((3, _db.SOCR));
                    }
                }
            }));

        if (objectLevel.Level > KladrTypesObjectsEnum.City && codeSmallCity != "000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB? _db = await context.ObjectsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeRegion}{codeRayon}{codeCity}{codeSmallCity}00");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.PopPoint, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((4, _db.SOCR));
                    }
                }
            }));

        if (objectLevel.Level > KladrTypesObjectsEnum.PopPoint && !string.IsNullOrWhiteSpace(codeStreet) && codeStreet != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB? _db = await context.StreetsKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeRegion}{codeRayon}{codeCity}{codeSmallCity}{codeStreet}00");
                if (_db is not null)
                {
                    dataResponse.TryAdd(KladrTypesObjectsEnum.Street, _db);
                    lock (socrBases)
                    {
                        socrBases.Add((5, _db.SOCR));
                    }
                }
            }));

        if (objectLevel.Level > KladrTypesObjectsEnum.Street && !string.IsNullOrWhiteSpace(codeHome) && codeHome != "0000")
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB? _db = await context.HousesKLADR.FirstOrDefaultAsync(x => x.CODE == $"{codeRegion}{codeRayon}{codeCity}{codeSmallCity}{codeStreet}{codeHome}");
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
        KeyValuePair<KladrTypesObjectsEnum, RootKLADRModelDB>[] dataList = [.. dataResponse.OrderBy(x => x.Key)];

        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
        SocrbaseKLADRModelDB[] resDb = await context.SocrbasesKLADR.Where(x => socrBases.Any(y => y.Item2 == x.SOCRNAME)).ToArrayAsync();

        return new()
        {
            Response = new()
            {
                Payload = JObject.FromObject(dataList.First()),
                Parents = [.. dataList.Skip(1)],
                Socrbase = resDb,
                TypeObject = objectLevel.Level,
                ChainType = objectLevel.Chain,
            }
        };
    }

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParent(KladrFindRequestModel req)
    {
        List<Task> tasks = [];
        Dictionary<KladrChainTypesEnum, JObject[]> res = [];
        if (string.IsNullOrWhiteSpace(req.Code))
        {
            List<ObjectKLADRModelDB> dataDb = default!;
            List<string> filterRegions = [];
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                dataDb = await context
                    .ObjectsKLADR
                    .Where(x => x.CODE.EndsWith("00000000000"))
                    .OrderBy(x => x.NAME)
                    .ToListAsync();
            }));

            if (!string.IsNullOrWhiteSpace(req.SearchQuery))
            {
                tasks.Add(
                        Task.Run(async () =>
                        {
                            using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                            filterRegions = await context
                                .ObjectsKLADR
                                .Where(x => EF.Functions.Like(x.NAME, $"%{req.SearchQuery}%"))
                                .Select(x => x.CODE.Substring(0, 2))
                                .Union(context.StreetsKLADR.Where(x => EF.Functions.Like(x.NAME, $"%{req.SearchQuery}%")).Select(x => x.CODE.Substring(0, 2)).Distinct())
                                .Distinct()
                                .ToListAsync();
                        })
                    );
            }

            await Task.WhenAll(tasks);
            res.Add(KladrChainTypesEnum.RootRegions, [.. dataDb.Where(x => filterRegions is null || filterRegions.Count == 0 || filterRegions.Contains(x.CODE[..2])).Select(JObject.FromObject)]);

            return res;
        }

        string codeObject = req.Code;
        string codeRegion = codeObject[..2];
        string codeRayon = codeObject.Substring(2, 3);
        string codeCity = codeObject.Substring(5, 3);
        string codeSmallCity = codeObject.Substring(8, 3);
        string codeStreet = codeObject.Length < 17 ? "" : codeObject.Substring(11, 4);
        string codeHome = codeObject.Length < 19 ? "" : codeObject.Substring(15, 4);

        ConcurrentDictionary<KladrChainTypesEnum, JObject[]> dataResponse = [];

        (KladrTypesObjectsEnum Level, KladrChainTypesEnum Chain) objectLevel = GlobalTools.ParseKladrTypeObject(codeObject);
        if (objectLevel.Level == KladrTypesObjectsEnum.RootRegion) // регионы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInRegion(codeRegion);
                dataResponse.TryAdd(KladrChainTypesEnum.CitiesInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в регионе
                //    $" code LIKE '{codeRegion}000___000__' AND code NOT LIKE '{codeRegion}___000_____' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInRegion(codeRegion);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в регионе
                //    $" code LIKE '{codeRegion}000000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.AreasInRegion(codeRegion);
                dataResponse.TryAdd(KladrChainTypesEnum.AreasInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'area' as typeObj FROM KLADR WHERE " + // районы в регионе
                //    $" code LIKE '{codeRegion}___000000__' AND code NOT LIKE '{codeRegion}000________' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInRegion(codeRegion);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM STREET WHERE " + // street`s в регионе
                //    $" (code LIKE '{codeRegion}000000000______') ORDER BY name");
            }));
        }
        else if (objectLevel.Level == KladrTypesObjectsEnum.Area) //районы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInArea(codeRegion, codeRayon);
                dataResponse.TryAdd(KladrChainTypesEnum.CitiesInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в районах
                //    $" code LIKE '{codeRegion}{codeRayon}___000__' AND code NOT LIKE '{codeRegion}___000_____'  ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInArea(codeRegion, codeRayon);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в районах
                //    $" code LIKE '{codeRegion}{codeRayon}000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
        }
        else if (objectLevel.Level == KladrTypesObjectsEnum.City) // города в районах
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInCity(codeRegion, codeRayon, codeCity);
                dataResponse.TryAdd(KladrChainTypesEnum.PopPointsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + $"_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInCity(codeRegion, codeRayon, codeCity);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + "_________' ORDER BY name");
            }));
        }
        else if (objectLevel.Level == KladrTypesObjectsEnum.PopPoint) // нас пункты
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInPopPoint(codeRegion, codeRayon, codeCity, codeSmallCity);
                dataResponse.TryAdd(KladrChainTypesEnum.StreetsInPopPoint, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в нас. пунктах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + codeSmallCity + "______' ORDER BY name");
            }));
        }
        else if (objectLevel.Level == KladrTypesObjectsEnum.Street) // улицы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB[] data = await context.HousesInStrit(codeRegion, codeRayon, codeCity, codeSmallCity, codeStreet);
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
        List<Task> tasks = [];
        TPaginationResponseModel<KladrResponseModel> response = new(req) { Response = [] };
        SocrbaseKLADRModelDB[] socrbases = default!;
        List<ObjectKLADRModelDB> dataDb = default!;

        if (string.IsNullOrWhiteSpace(req.CodeLikeFilter))
        {
            List<string> filterRegions = [];
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                socrbases = await context
                    .SocrbasesKLADR
                    .Where(x => x.LEVEL == "1")
                    .OrderBy(x => x.SCNAME)
                    .ToArrayAsync();
            }));

            if (!string.IsNullOrWhiteSpace(req.FindQuery))
            {
                tasks.Add(
                        Task.Run(async () =>
                        {
                            using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                            filterRegions = await context
                                .ObjectsKLADR
                                .Where(x => EF.Functions.Like(x.NAME, $"%{req.FindQuery}%"))
                                .Select(x => x.CODE.Substring(0, 2))
                                .Union(context.StreetsKLADR.Where(x => EF.Functions.Like(x.NAME, $"%{req.FindQuery}%")).Select(x => x.CODE.Substring(0, 2)).Distinct())
                                .Distinct()
                                .ToListAsync();
                        })
                    );
            }

            await Task.WhenAll(tasks);

            using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
            IQueryable<ObjectKLADRModelDB> q = context
                .ObjectsKLADR
                .Where(x => x.CODE.EndsWith("00000000000"));

            if (filterRegions is not null && filterRegions.Count != 0)
                q = q.Where(x => filterRegions.Contains(x.CODE.Substring(0, 2)));

            response.TotalRowsCount = await q.CountAsync();
            dataDb = await q
                .OrderBy(x => x.NAME)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            response.Response.AddRange(dataDb.Select(x => new KladrResponseModel()
            {
                ChainType = KladrChainTypesEnum.RootRegions,
                TypeObject = KladrTypesObjectsEnum.RootRegion,
                Payload = JObject.FromObject(x),
                Socrbase = [.. socrbases.Where(y => y.SOCRNAME == x.SOCR)]
            }));
            return response;
        }


        throw new NotImplementedException();
    }
}