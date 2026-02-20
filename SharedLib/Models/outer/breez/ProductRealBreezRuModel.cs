////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////



namespace SharedLib;

/// <summary>
/// ProductRealBreezRuModel
/// </summary>
public class ProductRealBreezRuModel : ProductBreezRuModel
{
    /// <inheritdoc/>
    public required int Key { get; set; }

    /// <inheritdoc/>
    public static ProductRealBreezRuModel Build(KeyValuePair<int, ProductBreezRuModel> x)
    {
        return new()
        {
            Key = x.Key,
            AccessoryNC = x.Value.AccessoryNC,
            Article = x.Value.Article,
            BimModel = x.Value.BimModel,
            Booklet = x.Value.Booklet,
            Brand = x.Value.Brand,
            CategoryId = x.Value.CategoryId,
            Description = x.Value.Description,
            Images = x.Value.Images,
            Price = x.Value.Price,
            Manual = x.Value.Manual,
            NarujNC = x.Value.NarujNC,
            NC = x.Value.NC,
            Series = x.Value.Series,
            Title = x.Value.Title,
            UTP = x.Value.UTP,
            VideoYoutube = x.Value.VideoYoutube,
            VnutrNC = x.Value.VnutrNC,
        };
    }
}