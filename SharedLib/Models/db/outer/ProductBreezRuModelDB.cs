////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ProductBreezRuModelDB
/// </summary>
[Index(nameof(PriceRIC))]
public class ProductBreezRuModelDB : ProductRealBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Images
    /// </summary>
    public new List<ImageProductBreezRuModelDB>? Images { get; set; }

    /// <summary>
    /// PriceRIC
    /// </summary>
    public string? PriceRIC {  get; set; }

    /// <summary>
    /// PriceCurrencyRIC
    /// </summary>
    public string? PriceCurrencyRIC { get; set; }
}