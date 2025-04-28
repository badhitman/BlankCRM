////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// связь моделей StockSharp с локальными моделями
/// </summary>
public static class BindsStockSharpModelsExtensionsDB
{
    public static OrderStockSharpModelDB Bind(this OrderStockSharpModelDB main, OrderStockSharpModel inc)
    {
        main.Id = inc.Id;

        //main.Instrument = new InstrumentStockSharpModelDB().Bind(inc.Instrument);
        //main.Portfolio = new PortfolioTradeModelDB().Bind(inc.Portfolio);

        main.State = inc.State;
        main.PositionEffect = inc.PositionEffect;
        main.TimeInForce = inc.TimeInForce;
        main.Type = inc.Type;
        main.Currency = inc.Currency;
        main.Side = inc.Side;
        main.MarginMode = inc.MarginMode;

        main.IsMarketMaker = inc.IsMarketMaker;
        main.Slippage = inc.Slippage;
        main.Price = inc.Price;
        main.ExpiryDate = inc.ExpiryDate;
        main.Yield = inc.Yield;
        main.Volume = inc.Volume;
        main.VisibleVolume = inc.VisibleVolume;
        main.MinVolume = inc.MinVolume;
        main.MatchedTime = inc.MatchedTime;
        main.Leverage = inc.Leverage;
        main.LatencyCancellation = inc.LatencyCancellation;
        main.IsManual = inc.IsManual;
        main.SeqNum = inc.SeqNum;
        main.Comment = inc.Comment;
        main.Time = inc.Time;
        main.Commission = inc.Commission;
        main.AveragePrice = inc.AveragePrice;
        main.Balance = inc.Balance;
        main.BoardId = inc.BoardId;
        main.UserOrderId = inc.UserOrderId;
        main.TransactionId = inc.TransactionId;
        main.StringId = inc.StringId;
        main.Status = inc.Status;
        main.PostOnly = inc.PostOnly;
        main.Slippage = inc.Slippage;
        main.LocalTime = inc.LocalTime;
        main.LatencyRegistration = inc.LatencyRegistration;
        main.LatencyEdition = inc.LatencyEdition;
        main.IsSystem = inc.IsSystem;
        main.CommissionCurrency = inc.CommissionCurrency;
        main.ClientCode = inc.ClientCode;
        main.CancelledTime = inc.CancelledTime;
        main.BrokerCode = inc.BrokerCode;

        return main;
    }

    /// <inheritdoc/>
    public static InstrumentStockSharpModelDB Bind(this InstrumentStockSharpModelDB main, InstrumentTradeStockSharpModel inc)
    {
        main.Multiplier = inc.Multiplier;
        main.FaceValue = inc.FaceValue;
        main.SettlementDate = inc.SettlementDate;
        main.Decimals = inc.Decimals;
        main.CfiCode = inc.CfiCode;
        main.Class = inc.Class;
        main.Code = inc.Code;
        main.ExpiryDate = inc.ExpiryDate;
        main.UnderlyingSecurityId = inc.UnderlyingSecurityId;
        main.ShortName = inc.ShortName;
        main.Shortable = inc.Shortable;
        main.PrimaryId = inc.PrimaryId;
        main.Name = inc.Name;

        main.Currency = inc.Currency;
        main.UnderlyingSecurityType = inc.UnderlyingSecurityType;
        main.TypeInstrument = inc.TypeInstrument;
        main.SettlementType = inc.SettlementType;
        main.OptionType = inc.OptionType;
        main.OptionStyle = inc.OptionStyle;

        //main.Board = new BoardStockSharpModelDB().Bind(inc.Board);

        return main;
    }

    /// <inheritdoc/>
    public static BoardStockSharpModelDB Bind(this BoardStockSharpModelDB main, BoardStockSharpModel inc)
    {
        main.Code = inc.Code;
        //main.Exchange = new ExchangeStockSharpModelDB().Bind(inc.Exchange);
        return main;
    }

    /// <inheritdoc/>
    public static ExchangeStockSharpModelDB Bind(this ExchangeStockSharpModelDB main, ExchangeStockSharpModel inc)
    {
        main.Name = inc.Name;
        main.CountryCode = inc.CountryCode;
        return main;
    }

    /// <inheritdoc/>
    public static PortfolioTradeModelDB Bind(this PortfolioTradeModelDB main, PortfolioStockSharpModel inc)
    {
        //main.Board = new BoardStockSharpModelDB().Bind(inc.Board);

        main.Currency = inc.Currency;
        main.State = inc.State;

        main.ClientCode = inc.ClientCode;
        main.Name = inc.Name;
        main.DepoName = inc.DepoName;
        //
        return main;
    }
}