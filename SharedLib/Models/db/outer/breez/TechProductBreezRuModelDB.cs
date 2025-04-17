////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechProductBreezRuModelDB
/// </summary>
[Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class TechProductBreezRuModelDB : ProductBreezRuLiteModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public List<TechPropertyProductBreezRuModelDB>? Properties { get; set; }

    /// <summary>
    /// Дата первого появления
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }


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
        res.Properties = x.Techs is null ? null : [.. x.Techs.Select(x => TechPropertyProductBreezRuModelDB.Build(x, res))];
        return res;
    }

    /// <inheritdoc/>
    public void SetLive()
    {
        Properties?.ForEach(pi => { pi.Parent = this; });
    }
}