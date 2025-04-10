////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// TechProductRealBreezRuModel
/// </summary>
[Index(nameof(Key))]
public class TechProductRealBreezRuModel : ProductBreezRuLiteModel
{
    /// <inheritdoc/>
    public required int Key { get; set; }

    /// <inheritdoc/>
    public Dictionary<int, PropTechProductBreezRuModel>? Techs { get; set; }

    /// <inheritdoc/>
    public static TechProductRealBreezRuModel Build(KeyValuePair<int, TechProductBreezRuResponseModel> x)
    {
        return new()
        {
            Key = x.Key,
            AccessoryNC = x.Value.AccessoryNC,
            NarujNC = x.Value.NarujNC,
            NC = x.Value.NC,
            VnutrNC = x.Value.VnutrNC,
            Techs = x.Value.Techs
        };
    }
}