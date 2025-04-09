////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// TechProductRealBreezRuModel
/// </summary>
[Index(nameof(Key))]
public class TechProductRealBreezRuModel : TechProductBreezRuModel
{
    /// <inheritdoc/>
    public required string Key { get; set; }

    /// <inheritdoc/>
    public static TechProductRealBreezRuModel Build(KeyValuePair<string, TechProductBreezRuModel> x)
    {
        return new()
        {
            Key = x.Key,
            Analog = x.Value.Analog,
            First = x.Value.First,
            Show = x.Value.Show,
            SubCategory = x.Value.SubCategory,
            Title = x.Value.Title,
            TypeParameter = x.Value.TypeParameter,
            Value = x.Value.Value,
            Filter = x.Value.Filter,
            FilterType = x.Value.FilterType,
            Group = x.Value.Group,
            IdChar = x.Value.IdChar,
            Order = x.Value.Order,
            Required = x.Value.Required,
            Unit = x.Value.Unit,

        };
    }
}