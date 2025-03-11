////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.RegularExpressions;
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
    public async Task<Dictionary<KladrTypesResultsEnum, JObject[]>> ObjectsList(KladrsListRequestModel req)
    {
        Dictionary<KladrTypesResultsEnum, JObject[]> res = [];
        if (string.IsNullOrWhiteSpace(req.ParentCode))
        {
            using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
            List<ObjectKLADRModelDB> dataDb = await context.ObjectsKLADR.Where(x => x.CODE.EndsWith("00000000000")).ToListAsync();
            res.Add(KladrTypesResultsEnum.RootRegions, [.. dataDb.Select(JObject.FromObject)]);
            return res;
        }

        string codeObject = req.ParentCode;
        string codeRegion = codeObject[..2];
        string codeRayon = codeObject.Substring(2, 3);
        string codeCity = codeObject.Substring(5, 3);
        string codeSmallCity = codeObject.Substring(8, 3);
        string codeStreet = codeObject.Length < 17 ? "" : codeObject.Substring(11, 4);
        string codeHome = codeObject.Length < 19 ? "" : codeObject.Substring(15, 4);

        ConcurrentDictionary<KladrTypesResultsEnum, JObject[]> dataResponse = [];
        List<Task> tasks = [];
        if (Regex.IsMatch(codeObject, @"^..000000000..$")) // регионы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInRegion(codeRegion);
                dataResponse.TryAdd(KladrTypesResultsEnum.CitiesInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в регионе
                //    $" code LIKE '{codeRegion}000___000__' AND code NOT LIKE '{codeRegion}___000_____' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInRegion(codeRegion);
                dataResponse.TryAdd(KladrTypesResultsEnum.PopPointsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в регионе
                //    $" code LIKE '{codeRegion}000000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.AreasInRegion(codeRegion);
                dataResponse.TryAdd(KladrTypesResultsEnum.AreasInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'area' as typeObj FROM KLADR WHERE " + // районы в регионе
                //    $" code LIKE '{codeRegion}___000000__' AND code NOT LIKE '{codeRegion}000________' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInRegion(codeRegion);
                dataResponse.TryAdd(KladrTypesResultsEnum.StreetsInRegion, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM STREET WHERE " + // street`s в регионе
                //    $" (code LIKE '{codeRegion}000000000______') ORDER BY name");
            }));
        }
        else if (Regex.IsMatch(codeObject, @"^.{5}000000..$") && !Regex.IsMatch(codeObject, @"^..000000000..$")) //районы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.CitiesInArea(codeRegion, codeRayon);
                dataResponse.TryAdd(KladrTypesResultsEnum.CitiesInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в районах
                //    $" code LIKE '{codeRegion}{codeRayon}___000__' AND code NOT LIKE '{codeRegion}___000_____'  ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInArea(codeRegion, codeRayon);
                dataResponse.TryAdd(KladrTypesResultsEnum.PopPointsInArea, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в районах
                //    $" code LIKE '{codeRegion}{codeRayon}000_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
        }
        else if (Regex.IsMatch(codeObject, @"^.{8}000..$") && !Regex.IsMatch(codeObject, @"^.{5}000.{5}$")) // города в районах
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                ObjectKLADRModelDB[] data = await context.PopPointsInCity(codeRegion, codeRayon, codeCity);
                dataResponse.TryAdd(KladrTypesResultsEnum.PopPointsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + $"_____' AND code NOT LIKE '{codeRegion}______000__' ORDER BY name");
            }));
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInCity(codeRegion, codeRayon, codeCity);
                dataResponse.TryAdd(KladrTypesResultsEnum.StreetsInCity, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + "_________' ORDER BY name");
            }));
        }
        else if (codeObject.Length == 13) // нас пункты
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                StreetKLADRModelDB[] data = await context.StreetsInPopPoint(codeRegion, codeRayon, codeCity, codeSmallCity);
                dataResponse.TryAdd(KladrTypesResultsEnum.StreetsInPopPoint, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в нас. пунктах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + codeSmallCity + "______' ORDER BY name");
            }));
        }
        else if (codeObject.Length == 17) // улицы
        {
            tasks.Add(Task.Run(async () =>
            {
                using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
                HouseKLADRModelDB[] data = await context.HousesInStrin(codeRegion, codeRayon, codeCity, codeSmallCity, codeStreet);
                dataResponse.TryAdd(KladrTypesResultsEnum.HousesInStrin, [.. data.Select(JObject.FromObject)]);
                //queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'home' as typeObj FROM DOMA WHERE " + // дома на улицах
                //    $" code LIKE '{codeRegion}{codeRayon}" + codeCity + codeSmallCity + codeStreet + "____' ORDER BY name");
            }));
        }
        await Task.WhenAll(tasks);

        foreach (KeyValuePair<KladrTypesResultsEnum, JObject[]> v in dataResponse)
            res.Add(v.Key, v.Value);

        return res;
    }
}