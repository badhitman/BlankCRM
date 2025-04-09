////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// BrandRealBreezRuModel
/// </summary>
public class BrandRealBreezRuModel : BrandBreezRuModel
{
    /// <inheritdoc/>
    public required string Key { get; set; }

    /// <inheritdoc/>
    public static BrandRealBreezRuModel Build(KeyValuePair<string, BrandBreezRuModel> x)
    {

        return new()
        {
            Key = x.Key,
            Title = x.Value.Title,
            CHPU = x.Value.CHPU,
            Image = x.Value.Image,
            Order = x.Value.Order,
            Url = x.Value.Url,
        };
    }
}