////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// ProductBreezRuModelDB
/// </summary>
[Index(nameof(PriceRIC))]
public class ProductBreezRuModelDB : ProductBreezRuBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Images
    /// </summary>
    public List<ImageProductBreezRuModelDB>? Images { get; set; }

    /// <summary>
    /// PriceRIC
    /// </summary>
    public string? PriceRIC { get; set; }

    /// <summary>
    /// PriceCurrencyRIC
    /// </summary>
    public string? PriceCurrencyRIC { get; set; }

    /// <inheritdoc/>
    public static ProductBreezRuModelDB Build(ProductRealBreezRuModel x)
    {
        ProductBreezRuModelDB res = new()
        {
            Title = x.Title,
            AccessoryNC = x.AccessoryNC,
            Article = x.Article,
            BimModel = x.BimModel,
            Booklet = x.Booklet,
            Brand = x.Brand,
            CategoryId = x.CategoryId,
            Description = x.Description,
            Manual = x.Manual,
            NarujNC = x.NarujNC,
            NC = x.NC,
            VnutrNC = x.VnutrNC,
            UTP = x.UTP,
            Series = x.Series,
            VideoYoutube = x.VideoYoutube,
            PriceRIC = x.Price?.GetValueOrDefault("ric"),
            PriceCurrencyRIC = x.Price?.GetValueOrDefault("ric_currency"),
        };
        res.Images = x.Images is null || x.Images.Length == 0 ? null : [.. x.Images.Select(x => new ImageProductBreezRuModelDB() { Name = x, Product = res })];
        return res;
    }
}