////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// PositionStockSharpModel
/// </summary>
public class PositionStockSharpModel
{
    /// <summary>
    /// Portfolio, in which position is created.
    /// </summary>
    public PortfolioStockSharpModel? Portfolio { get; set; }

    /// <summary>
    /// Security, for which a position was created.
    /// </summary>
    public virtual InstrumentTradeStockSharpModel? Instrument { get; set; }


    /// <summary>
    /// Portfolio name.
    /// </summary>
    public string PortfolioName { get; set; } = default!;

    /// <summary>
    /// Position size at the beginning of the trading session.
    /// </summary>
    public decimal? BeginValue { get; set; }

    /// <summary>
    /// Current position size.
    /// </summary>
    public decimal? CurrentValue { get; set; }

    /// <summary>
    /// Position size, registered for active orders.
    /// </summary>
    public decimal? BlockedValue { get; set; }

    /// <summary>
    /// Position price.
    /// </summary>
    public decimal? CurrentPrice { get; set; }

    /// <summary>
    /// Average price.
    /// </summary>
    public decimal? AveragePrice { get; set; }

    /// <summary>
    /// Unrealized profit.
    /// </summary>
    public decimal? UnrealizedPnL { get; set; }

    /// <summary>
    /// Realized profit.
    /// </summary>
    public decimal? RealizedPnL { get; set; }

    /// <summary>
    /// Variation margin.
    /// </summary>    
    public decimal? VariationMargin { get; set; }

    /// <summary>
    /// Total commission.
    /// </summary>
    public decimal? Commission { get; set; }

    /// <summary>
    /// Settlement price.
    /// </summary>
    public decimal? SettlementPrice { get; set; }

    /// <summary>
    /// Time of last position change.
    /// </summary>
    public DateTimeOffset LastChangeTime { get; set; }

    /// <summary>
    /// Local time of the last position change.
    /// </summary>
    public DateTimeOffset LocalTime { get; set; }

    /// <summary>
    /// Text position description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Portfolio currency.
    /// </summary>
    public CurrenciesTypesEnum? Currency { get; set; }

    /// <summary>
    /// Expiration date.
    /// </summary>
    public DateTimeOffset? ExpirationDate { get; set; }

    /// <summary>
    /// Client code assigned by the broker.
    /// </summary>
    public string ClientCode { get; set; } = default!;

    /// <summary>
    /// The depositary where the physical security.
    /// </summary>
    public string DepoName { get; set; } = default!;

    /// <summary>
    /// Limit type for Т+ market.
    /// </summary>
    public TPlusLimitsEnum? LimitType { get; set; }

    /// <summary>
    /// Strategy id.
    /// </summary>
    public string StrategyId { get; set; } = default!;

    /// <summary>
    /// Side.
    /// </summary>
    public virtual SidesEnum? Side { get; set; }

    /// <summary>
    /// Margin leverage.
    /// </summary>
    public decimal? Leverage { get; set; }

    /// <summary>
    /// Commission (taker).
    /// </summary>
    public decimal? CommissionTaker { get; set; }

    /// <summary>
    /// Commission (maker).
    /// </summary>
    public decimal? CommissionMaker { get; set; }

    /// <summary>
    /// Orders (bids).
    /// </summary>
    public int? BuyOrdersCount { get; set; }

    /// <summary>
    /// Orders (asks).
    /// </summary>
    public int? SellOrdersCount { get; set; }

    /// <summary>
    /// Margin (buy).
    /// </summary>
    public decimal? BuyOrdersMargin { get; set; }

    /// <summary>
    /// Margin (sell).
    /// </summary>
    public decimal? SellOrdersMargin { get; set; }

    /// <summary>
    /// Orders (margin).
    /// </summary>
    public decimal? OrdersMargin { get; set; }

    /// <summary>
    /// Orders.
    /// </summary>
    public int? OrdersCount { get; set; }

    /// <summary>
    /// Trades.
    /// </summary>
    public int? TradesCount { get; set; }

    /// <summary>
    /// Liquidation price.
    /// </summary>
    public decimal? LiquidationPrice { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string text = $"{Portfolio}-{Instrument}";
        if (!StrategyId.IsEmpty())
        {
            text = text + "-" + StrategyId;
        }

        if (Side.HasValue)
        {
            text += $"-{Side.Value}";
        }

        return text;
    }
}