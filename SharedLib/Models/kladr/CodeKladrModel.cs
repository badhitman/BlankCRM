////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.RegularExpressions;

namespace SharedLib;

/// <summary>
/// CodeKladrModel
/// </summary>
public partial class CodeKladrModel
{
    /// <inheritdoc/>
    public required KladrTypesObjectsEnum Level { get; set; }

    /// <inheritdoc/>
    public required KladrChainTypesEnum Chain { get; set; }


    /// <inheritdoc/>
    public required string CodeOrigin { get; set; }


    /// <inheritdoc/>
    public required string RegionCode { get; set; } // code[..2];

    /// <inheritdoc/>
    public required string AreaCode { get; set; } // code.Substring(2, 3);

    /// <inheritdoc/>
    public required string CityCode { get; set; } // code.Substring(5, 3);

    /// <inheritdoc/>
    public required string PopPointCode { get; set; } // code.Substring(8, 3);

    /// <inheritdoc/>
    public required string StreetCode { get; set; } // code.Length < 17 ? "" : code.Substring(11, 4);

    /// <inheritdoc/>
    public required string HomeCode { get; set; } // code.Length < 19 ? "" : code.Substring(15, 4);


    /// <inheritdoc/>
    public required string? SignOfRelevanceCode { get; set; }

    /// <inheritdoc/>
    public required SignOfRelevanciesEnum? SignOfRelevance { get; set; }

    /// <inheritdoc/>
    public string? ChildsCodesTemplate => Level switch 
    {
         KladrTypesObjectsEnum.RootRegion => $"{RegionCode}%00",
         KladrTypesObjectsEnum.Area => $"{RegionCode}{AreaCode}%00",
         KladrTypesObjectsEnum.City => $"{RegionCode}{AreaCode}{CityCode}%00",
         KladrTypesObjectsEnum.PopPoint => $"{RegionCode}{AreaCode}{CityCode}{PopPointCode}____00",
         KladrTypesObjectsEnum.Street => $"{RegionCode}{AreaCode}{CityCode}{PopPointCode}{StreetCode}____",
         _ => default
    };

    /// <inheritdoc/>
    public static CodeKladrModel Build(string code)
    {
        string codeRegion = code[..2];
        string codeArea = code.Substring(2, 3);
        string codeCity = code.Substring(5, 3);
        string codePopPoint = code.Substring(8, 3);
        string codeStreet = code.Length < 17 ? "" : code.Substring(11, 4);
        string codeHome = code.Length < 19 ? "" : code.Substring(15, 4);

        string? signOfRelevanceCode = code.Length > 17
            ? null
            : code[^2..];

        SignOfRelevanciesEnum? signOfRel = null;
        if (!string.IsNullOrWhiteSpace(signOfRelevanceCode))
        {
            int parseRel = int.Parse(signOfRelevanceCode);
            if (parseRel == 0)
                signOfRel = SignOfRelevanciesEnum.Actual;

            else if (parseRel >= 1 && parseRel <= 50)
                signOfRel = SignOfRelevanciesEnum.Renamed;

            else if (parseRel == 51)
                signOfRel = SignOfRelevanciesEnum.WasReassigned;

            else if (parseRel >= 52 && parseRel <= 98)
                signOfRel = SignOfRelevanciesEnum.Reserve;

            else if (parseRel == 99)
                signOfRel = SignOfRelevanciesEnum.NotExist;
        }

        if (RegionPatternRegex().IsMatch(code)) // регионы
            return new()
            {
                CodeOrigin = code,
                Level = KladrTypesObjectsEnum.RootRegion,
                Chain = KladrChainTypesEnum.RootRegions,
                RegionCode = codeRegion,
                AreaCode = codeArea,
                CityCode = codeCity,
                HomeCode = codeHome,
                PopPointCode = codePopPoint,
                StreetCode = codeStreet,
                SignOfRelevance = signOfRel,
                SignOfRelevanceCode = signOfRelevanceCode,
            };

        if (AreaPatternRegex().IsMatch(code)) // районы
            return new()
            {
                CodeOrigin = code,
                Level = KladrTypesObjectsEnum.Area,
                Chain = KladrChainTypesEnum.AreasInRegion,
                RegionCode = codeRegion,
                AreaCode = codeArea,
                CityCode = codeCity,
                HomeCode = codeHome,
                PopPointCode = codePopPoint,
                StreetCode = codeStreet,
                SignOfRelevance = signOfRel,
                SignOfRelevanceCode = signOfRelevanceCode,
            };

        if (CityPatternRegex().IsMatch(code) && !CityValidatePatternRegex().IsMatch(code)) // города
            return new()
            {
                CodeOrigin = code,
                Level = KladrTypesObjectsEnum.City,
                Chain = codeArea.Equals("000") ? KladrChainTypesEnum.CitiesInRegion : KladrChainTypesEnum.CitiesInArea,
                RegionCode = codeRegion,
                AreaCode = codeArea,
                CityCode = codeCity,
                HomeCode = codeHome,
                PopPointCode = codePopPoint,
                StreetCode = codeStreet,
                SignOfRelevance = signOfRel,
                SignOfRelevanceCode = signOfRelevanceCode,
            };

        if (code.Length == 13) // нас пункты
        {
            if (codeArea.Equals("000") && codeCity.Equals("000"))
                return new()
                {
                    CodeOrigin = code,
                    Level = KladrTypesObjectsEnum.PopPoint,
                    Chain = KladrChainTypesEnum.PopPointsInRegion,
                    RegionCode = codeRegion,
                    AreaCode = codeArea,
                    CityCode = codeCity,
                    HomeCode = codeHome,
                    PopPointCode = codePopPoint,
                    StreetCode = codeStreet,
                    SignOfRelevance = signOfRel,
                    SignOfRelevanceCode = signOfRelevanceCode,
                };

            if (!codeCity.Equals("000"))
                return new()
                {
                    CodeOrigin = code,
                    Level = KladrTypesObjectsEnum.PopPoint,
                    Chain = KladrChainTypesEnum.PopPointsInCity,
                    RegionCode = codeRegion,
                    AreaCode = codeArea,
                    CityCode = codeCity,
                    HomeCode = codeHome,
                    PopPointCode = codePopPoint,
                    StreetCode = codeStreet,
                    SignOfRelevance = signOfRel,
                    SignOfRelevanceCode = signOfRelevanceCode,
                };

            return new()
            {
                CodeOrigin = code,
                Level = KladrTypesObjectsEnum.PopPoint,
                Chain = KladrChainTypesEnum.PopPointsInArea,
                RegionCode = codeRegion,
                AreaCode = codeArea,
                CityCode = codeCity,
                HomeCode = codeHome,
                PopPointCode = codePopPoint,
                StreetCode = codeStreet,
                SignOfRelevance = signOfRel,
                SignOfRelevanceCode = signOfRelevanceCode,
            };
        }

        if (code.Length == 17) // улицы
        {
            if (codePopPoint.Equals("000") && codeCity.Equals("000"))
                return new()
                {
                    CodeOrigin = code,
                    Level = KladrTypesObjectsEnum.Street,
                    Chain = KladrChainTypesEnum.StreetsInRegion,
                    RegionCode = codeRegion,
                    AreaCode = codeArea,
                    CityCode = codeCity,
                    HomeCode = codeHome,
                    PopPointCode = codePopPoint,
                    StreetCode = codeStreet,
                    SignOfRelevance = signOfRel,
                    SignOfRelevanceCode = signOfRelevanceCode,
                };

            if (!codeCity.Equals("000"))
                return new()
                {
                    CodeOrigin = code,
                    Level = KladrTypesObjectsEnum.Street,
                    Chain = KladrChainTypesEnum.StreetsInCity,
                    RegionCode = codeRegion,
                    AreaCode = codeArea,
                    CityCode = codeCity,
                    HomeCode = codeHome,
                    PopPointCode = codePopPoint,
                    StreetCode = codeStreet,
                    SignOfRelevance = signOfRel,
                    SignOfRelevanceCode = signOfRelevanceCode,
                };

            return new()
            {
                CodeOrigin = code,
                Level = KladrTypesObjectsEnum.Street,
                Chain = KladrChainTypesEnum.StreetsInPopPoint,
                RegionCode = codeRegion,
                AreaCode = codeArea,
                CityCode = codeCity,
                HomeCode = codeHome,
                PopPointCode = codePopPoint,
                StreetCode = codeStreet,
                SignOfRelevance = signOfRel,
                SignOfRelevanceCode = signOfRelevanceCode,
            };
        }

        return new()
        {
            CodeOrigin = code,
            Level = KladrTypesObjectsEnum.House,
            Chain = KladrChainTypesEnum.HousesInStreet,
            RegionCode = codeRegion,
            AreaCode = codeArea,
            CityCode = codeCity,
            HomeCode = codeHome,
            PopPointCode = codePopPoint,
            StreetCode = codeStreet,
            SignOfRelevance = signOfRel,
            SignOfRelevanceCode = signOfRelevanceCode,
        };

    }

    [GeneratedRegex(@"^..000000000..$")]
    private static partial Regex RegionPatternRegex();
    [GeneratedRegex(@"^.{5}000000..$")]
    private static partial Regex AreaPatternRegex();
    [GeneratedRegex(@"^.{8}000..$")]
    private static partial Regex CityPatternRegex();
    [GeneratedRegex(@"^.{5}000.{5}$")]
    private static partial Regex CityValidatePatternRegex();
}