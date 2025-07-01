////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// PortfolioStockSharpModel
/// </summary>
public partial class PortfolioStockSharpModel : IEquatable<PortfolioStockSharpModel>
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <inheritdoc/>
    public virtual BoardStockSharpModel Board { get; set; }

    /// <inheritdoc/>
    public PortfolioStatesEnum? State { get; set; }

    /// <inheritdoc/>
    public CurrenciesTypesEnum? Currency { get; set; }

    /// <inheritdoc/>
    public string ClientCode { get; set; }

    /// <inheritdoc/>
    public string DepoName { get; set; }

    /// <inheritdoc/>
    public decimal? BeginValue { get; set; }

    /// <inheritdoc/>
    public decimal? CurrentValue { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} /{Currency}{(Board is null ? null : $"[{Board}]")}";
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Board, ClientCode, State, Currency, DepoName);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is PortfolioStockSharpModel other)
            return Equals(other);

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(PortfolioStockSharpModel other)
    {
        return
                other.State == State &&
                other.ClientCode == ClientCode &&
                other.Name == Name &&
                other.DepoName == DepoName &&
                ((other.Board is null && Board is null) || other.Board?.Equals(Board) == true);
    }
}