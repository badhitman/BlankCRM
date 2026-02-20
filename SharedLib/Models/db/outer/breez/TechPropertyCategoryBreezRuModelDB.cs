////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechPropertyCategoryBreezRuModelDB
/// </summary>
public class TechPropertyCategoryBreezRuModelDB : TechCategoryBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Parent
    /// </summary>
    public TechCategoryBreezRuModelDB? Parent { get; set; }
    /// <summary>
    /// Parent
    /// </summary>
    public int ParentId { get; set; }

    /// <inheritdoc/>
    public static TechPropertyCategoryBreezRuModelDB Build(KeyValuePair<int, TechCategoryBreezRuModel> x, TechCategoryBreezRuModelDB res)
    {
        return new()
        {
            TechId = x.Key,
            Title = x.Value.Title,
            DataType = x.Value.DataType,
            Filter = x.Value.Filter,
            FilterType = x.Value.FilterType,
            Group = x.Value.Group,
            Order = x.Value.Order,
            Parent = res,
            Unit = x.Value.Unit,
            Required = x.Value.Required,
        };
    }
}