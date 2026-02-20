////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// BrandRealBreezRuModelDB
/// </summary>
public class BrandBreezRuModelDB : BrandRealBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public static BrandBreezRuModelDB Build(BrandRealBreezRuModel x)
    {
        return new()
        {
            Key = x.Key,
            Title = x.Title,
            CHPU = x.CHPU,
            Image = x.Image,
            Order = x.Order,
            Url = x.Url,
        };
    }
}