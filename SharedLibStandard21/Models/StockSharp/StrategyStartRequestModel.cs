////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// StrategyStartRequestModel
/// </summary>
public class StrategyStartRequestModel
{
    /// <inheritdoc/>
    public List<StrategyTradeStockSharpModel> Instruments { get; set; }
}