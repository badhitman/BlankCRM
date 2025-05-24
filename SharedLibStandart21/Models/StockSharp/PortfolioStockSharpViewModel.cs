////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// PortfolioStockSharpViewModel
/// </summary>
public partial class PortfolioStockSharpViewModel : PortfolioStockSharpModel
{
    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}