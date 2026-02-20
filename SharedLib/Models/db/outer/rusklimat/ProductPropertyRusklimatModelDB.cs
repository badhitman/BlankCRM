////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Связь свойства с товаром
/// </summary>
[Index(nameof(PropertyKey))]
public class ProductPropertyRusklimatModelDB : ProductSimplePropertyModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Product
    /// </summary>
    public ProductRusklimatModelDB? Product { get; set; }
    /// <summary>
    /// Product
    /// </summary>
    public string ProductId { get; set; } = default!;

    /// <summary>
    /// PropertyKey
    /// </summary>
    public string? PropertyKey { get; set; }


    /// <inheritdoc/>
    public static ProductPropertyRusklimatModelDB Build(KeyValuePair<string, ProductSimplePropertyModel> x, ProductRusklimatModelDB prod, PropertyRusklimatModelDB[] data)
    {
        return new()
        {
            Product = prod,
            Value = x.Value.Value,
            PropertyKey = x.Key,
            Unit = x.Value.Unit,
        };
    }
}