////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MyTradeStockSharpModel
/// </summary>
public class MyTradeStockSharpModel
{
    /// <inheritdoc/>
    public virtual OrderStockSharpModel Order { get; set; }

    /// <inheritdoc/>
    public decimal? Commission { get; set; }

    /// <inheritdoc/>
    public string CommissionCurrency { get; set; }

    /// <inheritdoc/>
    public decimal? Slippage { get; set; }

    /// <inheritdoc/>
    public decimal? PnL { get; set; }

    /// <inheritdoc/>
    public decimal? Position { get; set; }

    /// <inheritdoc/>
    public bool? Initiator { get; set; }

    /// <inheritdoc/>
    public decimal? Yield { get; set; }
}