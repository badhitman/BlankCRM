////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PriceProductDaichiModelDB
/// </summary>
[Index(nameof(PRICE))]
public class PriceProductDaichiModelDB : DaichiEntryModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }


    /// <inheritdoc/>
    public ProductDaichiModelDB? Product { get; set; }

    /// <inheritdoc/>
    public int ProductId { get; set; }


    /// <inheritdoc/>
    public string? PRICE { get; set; }

    /// <inheritdoc/>
    public string? CURRENCY { get; set; }
}