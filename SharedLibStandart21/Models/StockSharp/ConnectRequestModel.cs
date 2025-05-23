////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// ConnectRequestModel
/// </summary>
public class ConnectRequestModel
{
    /// <inheritdoc/>
    public List<InstrumentTradeStockSharpViewModel>? Instruments { get; set; }

    /// <inheritdoc/>
    public PortfolioStockSharpModel? Portfolio { get; set; }

    /// <inheritdoc/>
    public List<BoardStockSharpModel>? BoardsFilter { get; set; }
}