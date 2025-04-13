////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// CategoryBreezRuModelDB
/// </summary>
public class CategoryBreezRuModelDB : CategoryRealBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public static CategoryBreezRuModelDB Build(CategoryRealBreezRuModel x)
    {
        return new()
        {
            Key = x.Key,
            Title = x.Title,
            Order = x.Order,
            CHPU = x.CHPU,
            Level = x.Level, 
        };
    }
}