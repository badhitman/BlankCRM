////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PortfolioTradeModel
/// </summary>
public partial class PortfolioStockSharpModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <inheritdoc/>
    public virtual BoardStockSharpModel? Board { get; set; }

    /// <inheritdoc/>
    public PortfolioStatesEnum? State { get; set; }

    /// <inheritdoc/>
    public CurrenciesTypesEnum? Currency { get; set; }

    /// <inheritdoc/>
    public string? ClientCode { get; set; }

    /// <inheritdoc/>
    public string? DepoName { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} /{Currency} [{Board}]";
    }
}