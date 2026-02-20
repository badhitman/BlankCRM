////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechPropertyProductBreezRuModelDB
/// </summary>
public class TechPropertyProductBreezRuModelDB : PropTechProductBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public int Key { get; set; }

    /// <inheritdoc/>
    public TechProductBreezRuModelDB? Parent { get; set; }
    /// <inheritdoc/>
    public int ParentId { get; set; }

    /// <inheritdoc/>
    public static TechPropertyProductBreezRuModelDB Build(KeyValuePair<int, PropTechProductBreezRuModel> x, TechProductBreezRuModelDB res)
    {
        return new()
        {
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
            Parent = res,
            Unit = x.Value.Unit,
            Required = x.Value.Required,
            Key = x.Key,
        };
    }
}