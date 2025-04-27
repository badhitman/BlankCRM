////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

[Index(nameof(LastUpdatedAtUTC)), Index(nameof(Id)), Index(nameof(StringId)), Index(nameof(BoardId)), Index(nameof(TransactionId)), Index(nameof(BrokerCode))]
/// <inheritdoc/>
public class OrderStockSharpModelDB : OrderStockSharpModel, IBaseStockSharpModel
{
    [Key]
    public int IdPK { get; set; }

    ///<inheritdoc/>
    public new InstrumentStockSharpModelDB Instrument { get; set; }
    ///<inheritdoc/>
    public int InstrumentId { get; set; }

    ///<inheritdoc/>
    public new PortfolioTradeModelDB Portfolio { get; set; }
    ///<inheritdoc/>
    public int PortfolioId { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    internal void SetUpdate(OrderStockSharpModel inc)
    {
        Id = inc.Id;
        LastUpdatedAtUTC = DateTime.UtcNow;

        State = inc.State;
        PositionEffect = inc.PositionEffect;
        TimeInForce = inc.TimeInForce;
        Type = inc.Type;
        Currency = inc.Currency;
        Side = inc.Side;
        MarginMode = inc.MarginMode;

        IsMarketMaker = inc.IsMarketMaker;
        Slippage = inc.Slippage;
        Price = inc.Price;
        ExpiryDate = inc.ExpiryDate;
        Yield = inc.Yield;
        Volume = inc.Volume;
        VisibleVolume = inc.VisibleVolume;
        MinVolume = inc.MinVolume;
        MatchedTime = inc.MatchedTime;
        Leverage = inc.Leverage;
        LatencyCancellation = inc.LatencyCancellation;
        IsManual = inc.IsManual;
        SeqNum = inc.SeqNum;
        Comment = inc.Comment;
        Time = inc.Time;
        Commission = inc.Commission;
        AveragePrice = inc.AveragePrice;
        Balance = inc.Balance;
        BoardId = inc.BoardId;
        UserOrderId = inc.UserOrderId;
        TransactionId = inc.TransactionId;
        StringId = inc.StringId;
        Status = inc.Status;
        PostOnly = inc.PostOnly;
        Slippage = inc.Slippage;
        LocalTime = inc.LocalTime;
        LatencyRegistration = inc.LatencyRegistration;
        LatencyEdition = inc.LatencyEdition;
        IsSystem = inc.IsSystem;
        CommissionCurrency = inc.CommissionCurrency;
        ClientCode = inc.ClientCode;
        CancelledTime = inc.CancelledTime;
        BrokerCode = inc.BrokerCode;
    }
}