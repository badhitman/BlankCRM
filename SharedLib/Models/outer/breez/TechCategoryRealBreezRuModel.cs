////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TechCategoryRealBreezRuModel
/// </summary>
public class TechCategoryRealBreezRuModel
{
    /// <inheritdoc/>
    public int CategoryKey { get; set; }

    /// <inheritdoc/>
    public Dictionary<int, TechCategoryBreezRuModel>? Techs { get; set; }

    /// <inheritdoc/>
    public static TechCategoryRealBreezRuModel Build(KeyValuePair<int, TechCategoryBreezRuModel> x, int category)
    {
        TechCategoryRealBreezRuModel res = new()
        {
            CategoryKey = category,
            Techs = []
        };
        res.Techs.Add(x.Key, x.Value);
        return res;
    }
}