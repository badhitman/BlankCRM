////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PriceGoodsDaichiModelDB
/// </summary>
[Index(nameof(PRICE)), Index(nameof(CreatedAt))]
public class PriceGoodsDaichiModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public GoodsDaichiModelDB? Goods { get; set; }

    /// <inheritdoc/>
    public int GoodsId { get; set; }

    /// <inheritdoc/>
    public string? PRICE { get; set; }

    /// <inheritdoc/>
    public required DateTime CreatedAt { get; set; }
}