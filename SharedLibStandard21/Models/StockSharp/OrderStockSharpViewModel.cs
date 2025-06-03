////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// OrderStockSharpViewModel
/// </summary>
public class OrderStockSharpViewModel : OrderStockSharpModel
{
    /// <inheritdoc/>
    public int IdPK { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}
