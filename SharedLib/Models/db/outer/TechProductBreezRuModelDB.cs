////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechProductBreezRuModelDB
/// </summary>
public class TechProductBreezRuModelDB : ProductBreezRuLiteModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public List<PropertyTechProductBreezRuModelDB>? Properties { get; set; }

    /// <inheritdoc/>
    public static TechProductBreezRuModelDB Build(TechProductRealBreezRuModel x)
    {
        TechProductBreezRuModelDB res = new()
        {
            NarujNC = x.NarujNC,
            AccessoryNC = x.AccessoryNC,
            NC = x.NC,
            VnutrNC = x.VnutrNC,
        };
        //res.Properties = x.Techs is null ? null : [.. x.Techs.Select(x => PropertyTechProductBreezRuModelDB.Build(x, res))];
        return res;
    }
}