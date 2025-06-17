////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// StrategyStartRequestModel
/// </summary>
public class StrategyStartRequestModel
{
    /// <inheritdoc/>
    public BoardStockSharpModel Board { get; set; }

    /// <inheritdoc/>
    public PortfolioStockSharpModel SelectedPortfolio { get; set; }
}