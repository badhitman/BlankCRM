﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using SharedLib;
using DbcLib;

namespace KladrService;

/// <inheritdoc/>
public class KladrNavigationServiceImpl(IDbContextFactory<KladrContext> kladrDbFactory) : IKladrNavigationService
{
    /// <inheritdoc/>
    public async Task<List<ObjectKLADRModelDB>> ObjectsList(KladrsListRequestModel req)
    {
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();

        if (string.IsNullOrWhiteSpace(req.ParentCode))
        {
            return await context.ObjectsKLADR.Where(x => x.CODE.EndsWith("00000000000")).ToListAsync();
        }

        string codeObject = req.ParentCode;
        string codeRegion = codeObject[..2];
        string codeRayon = codeObject.Substring(2, 3);
        string codeCity = codeObject.Substring(5, 3);
        string codeSmallCity = codeObject.Substring(8, 3);
        string codeStreet = codeObject.Length < 17 ? "" : codeObject.Substring(11, 4);
        string codeHome = codeObject.Length < 19 ? "" : codeObject.Substring(15, 4);

        ArrayList queryTextList = [];
        if (Regex.IsMatch(codeObject, @"^..000000000..$")) // регионы
        {
            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // город в регионе
                " code LIKE '__000___000__' AND code NOT LIKE '_____000_____' ORDER BY name");

            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункт в регионе
                " code LIKE '__000000_____' AND code NOT LIKE '________000__' ORDER BY name");

            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'area' as typeObj FROM KLADR WHERE " + // районы в регионах
                " code LIKE '_____000000__' AND code NOT LIKE '__000________' ORDER BY name");

            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM STREET WHERE " + // street
                " (code LIKE '__000000000______') ORDER BY name");
        }
        else if (Regex.IsMatch(codeObject, @"^.{5}000000..$") && !Regex.IsMatch(codeObject, @"^..000000000..$")) //районы
        {
            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'city' as typeObj FROM KLADR WHERE " + // города в районах
                " code LIKE '__" + codeRayon + "___000__' AND code NOT LIKE '_____000_____'  ORDER BY name");

            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в районах
            " code LIKE '__" + codeRayon + "000_____' AND code NOT LIKE '________000__' ORDER BY name");
        }
        else if (Regex.IsMatch(codeObject, @"^.{8}000..$") && !Regex.IsMatch(codeObject, @"^.{5}000.{5}$")) // города
        {
            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'smallcity' as typeObj FROM KLADR WHERE " + // нас. пункты в городах
            " code LIKE '__" + codeRayon + codeCity + "_____' AND code NOT LIKE '________000__' ORDER BY name");

            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
            " code LIKE '__" + codeRayon + codeCity + "_________' ORDER BY name");
        }
        else if (codeObject.Length == 13) // нас пункты
        {
            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'street' as typeObj FROM KLADR WHERE " + // улицы в городах
            " code LIKE '__" + codeRayon + codeCity + codeSmallCity + "______' ORDER BY name");
        }
        else if (codeObject.Length == 17) // улицы
        {
            queryTextList.Add("SELECT name, socr, code, post_index, gninmb, uno, ocatd, 'home' as typeObj FROM DOMA WHERE " + // улицы в городах
            " code LIKE '__" + codeRayon + codeCity + codeSmallCity + codeStreet + "____' ORDER BY name");
        }


        throw new NotImplementedException();
    }
}