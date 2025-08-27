////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OrderStockSharpViewModel
/// </summary>
public class OrderStockSharpViewModel : OrderStockSharpModel
{
    /// <inheritdoc/>
    [Key]
    public int IdPK { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}
