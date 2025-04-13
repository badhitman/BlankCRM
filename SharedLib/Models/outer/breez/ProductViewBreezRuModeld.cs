////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ProductViewBreezRuModeld
/// </summary>
public class ProductViewBreezRuModeld : ProductBreezRuModelDB
{
    /// <summary>
    /// остатки на складах
    /// </summary>
    public BreezRuLeftoverModelDB? Leftover { get; set; }

    /// <inheritdoc/>
    public static ProductViewBreezRuModeld Build(ProductBreezRuModelDB product, BreezRuLeftoverModelDB leftover)
    {
        ProductViewBreezRuModeld res = new()
        {
            NC = product.NC,
            UTP = product.UTP,
            AccessoryNC = product.AccessoryNC,
            Article = product.Article,
            BimModel = product.BimModel,
            Booklet = product.Booklet,
            Brand = product.Brand,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            Description = product.Description,
            Id = product.Id,
            Images = product.Images,
            Manual = product.Manual,
            NarujNC = product.NarujNC,
            PriceCurrencyRIC = product.PriceCurrencyRIC,
            PriceRIC = product.PriceRIC,
            Series = product.Series,
            Title = product.Title,
            UpdatedAt = product.UpdatedAt,
            VideoYoutube = product.VideoYoutube,
            VnutrNC = product.VnutrNC,
            Leftover = leftover,
        };

        return res;
    }
}