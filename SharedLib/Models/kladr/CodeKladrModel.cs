////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CodeKladrModel
/// </summary>
public class CodeKladrModel
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
}