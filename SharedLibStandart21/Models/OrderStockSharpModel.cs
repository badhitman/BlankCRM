////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// OrderStockSharpModel
/// </summary>
public class OrderStockSharpModel
{
    /// <summary>
    /// Time taken to register an order.
    /// </summary>
    public TimeSpan? LatencyRegistration { get; set; }

    /// <summary>
    /// Time taken to cancel an order.
    /// </summary>
    public TimeSpan? LatencyCancellation { get; set; }

    /// <summary>
    /// Time taken to edit an order.
    /// </summary>
    public TimeSpan? LatencyEdition { get; set; }

    /// <summary>
    /// Order ID.
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Order ID (as string, if electronic board does not use numeric order ID representation).
    /// </summary>
    public string? StringId { get; set; }

    /// <summary>
    /// Board order id. Uses in case of StockSharp.BusinessEntities.Order.Id and StockSharp.BusinessEntities.Order.StringId is a brokerage system ids.
    /// </summary>
    public string? BoardId { get; set; }

    /// <summary>
    /// Order placing time on exchange.
    /// </summary>
    public DateTimeOffset Time { get; set; }

    /// <summary>
    /// Transaction ID. Automatically set when the StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order) method called.
    /// </summary>
    public virtual long TransactionId { get; set; }

    /// <summary>
    /// Security, for which an order is being placed.
    /// </summary>
    public virtual InstrumentTradeStockSharpModel? Instrument { get; set; }

    /// <summary>
    /// Order state.
    /// </summary>
    public OrderStatesEnum State { get; set; }

    /// <summary>
    /// Portfolio, in which the order is being traded.
    /// </summary>
    public virtual PortfolioStockSharpModel? Portfolio { get; set; }

    /// <summary>
    /// Cancelled time.
    /// </summary>
    public DateTimeOffset? CancelledTime { get; set; }

    /// <summary>
    /// Matched time.
    /// </summary>
    public DateTimeOffset? MatchedTime { get; set; }

    /// <summary>
    /// Last order change local time (Cancellation, Fill).
    /// </summary>
    public DateTimeOffset LocalTime { get; set; }

    /// <summary>
    /// Order price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Number of contracts in the order.
    /// </summary>
    public decimal Volume { get; set; }

    /// <summary>
    /// Visible quantity of contracts in order.
    /// </summary>
    public decimal? VisibleVolume { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    public SidesEnum Side { get; set; }

    /// <summary>
    /// Order contracts balance.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// System order status.
    /// </summary>
    public long? Status { get; set; }

    /// <summary>
    /// Is a system trade.
    /// </summary>
    public bool? IsSystem { get; set; }

    /// <summary>
    /// Placed order comment.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Order type.
    /// </summary>
    public OrderTypesEnum? Type { get; set; }

    /// <summary>
    /// Order expiry time. The default is null, which mean (GTC).
    /// </summary>
    /// <remarks>
    /// If the value is null, then the order is registered until cancel. Otherwise, the period is specified.
    /// </remarks>
    public DateTimeOffset? ExpiryDate { get; set; }

    /// <summary>
    /// Limit order time in force.
    /// </summary>
    public TimeInForceEnum? TimeInForce { get; set; }

    /// <summary>
    /// Commission (broker, exchange etc.).
    /// </summary>
    public decimal? Commission { get; set; }

    /// <summary>
    /// Commission currency. Can be null.
    /// </summary>
    public string? CommissionCurrency { get; set; }

    /// <summary>
    /// User's order ID.
    /// </summary>
    public string? UserOrderId { get; set; }

    /// <summary>
    /// Broker firm code.
    /// </summary>
    public string? BrokerCode { get; set; }

    /// <summary>
    /// Client code assigned by the broker.
    /// </summary>
    public string? ClientCode { get; set; }

    /// <summary>
    /// Trading security currency.
    /// </summary>
    public CurrenciesTypesEnum? Currency { get; set; }

    /// <summary>
    /// Is the order of market-maker.
    /// </summary>
    public bool? IsMarketMaker { get; set; }

    /// <summary>
    /// Margin mode.
    /// </summary>
    public MarginModesEnum? MarginMode { get; set; }

    /// <summary>
    /// Slippage in trade price.
    /// </summary>
    public decimal? Slippage { get; set; }

    /// <summary>
    /// Is order manual.
    /// </summary>
    public bool? IsManual { get; set; }

    /// <summary>
    /// Average execution price.
    /// </summary>
    public decimal? AveragePrice { get; set; }

    /// <summary>
    /// Yield.
    /// </summary>
    public decimal? Yield { get; set; }

    /// <summary>
    /// Minimum quantity of an order to be executed.
    /// </summary>
    public decimal? MinVolume { get; set; }

    /// <summary>
    /// Position effect.
    /// </summary>
    public OrderPositionEffectsEnum? PositionEffect { get; set; }

    /// <summary>
    /// Post-only order.
    /// </summary>
    public bool? PostOnly { get; set; }

    /// <summary>
    /// Sequence number.
    /// </summary>
    /// <remarks>
    /// Zero means no information.
    /// </remarks>
    public long SeqNum { get; set; }

    /// <summary>
    /// Margin leverage.
    /// </summary>
    public int? Leverage { get; set; }


    /// <inheritdoc/>
    public override string ToString()
    {
        string text = "";
        if (!string.IsNullOrWhiteSpace(UserOrderId))
        {
            text = text + " UID=" + UserOrderId;
        }

        if (AveragePrice.HasValue)
        {
            text += $" AvgPrice={AveragePrice}";
        }

        if (MinVolume.HasValue)
        {
            text += $" MinVolume={MinVolume}";
        }

        if (PositionEffect.HasValue)
        {
            text += $" PosEffect={PositionEffect.Value}";
        }

        if (PostOnly.HasValue)
        {
            text += $",PostOnly={PostOnly.Value}";
        }

        if (SeqNum != 0L)
        {
            text += $",SeqNum={SeqNum}";
        }

        if (Leverage.HasValue)
        {
            text += $",Leverage={Leverage.Value}";
        }

        return text;
    }
}