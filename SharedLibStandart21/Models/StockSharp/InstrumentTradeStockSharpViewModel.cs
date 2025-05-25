////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// InstrumentTradeStockSharpViewModel
/// </summary>
public partial class InstrumentTradeStockSharpViewModel : InstrumentTradeStockSharpModel
{
    /// <summary>
    /// IsFavorite
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}