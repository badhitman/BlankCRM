////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using StockSharp.BusinessEntities;
using SharedLib;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// связь моделей StockSharp с локальными моделями
/// </summary>
public static class BindsStockSharpModelsExtensions
{
    public static OrderStockSharpModel Bind(this OrderStockSharpModel main, Order inc)
    {
        main.Id = inc.Id;

        main.Instrument = new InstrumentTradeStockSharpModel().Bind(inc.Security);
        main.Portfolio = new PortfolioStockSharpModel().Bind(inc.Portfolio);

        main.State = (OrderStatesEnum)Enum.Parse(typeof(OrderStatesEnum), Enum.GetName(inc.State));
        main.PositionEffect = inc.PositionEffect is null ? null : (OrderPositionEffectsEnum)Enum.Parse(typeof(OrderPositionEffectsEnum), Enum.GetName(inc.PositionEffect.Value));
        main.TimeInForce = inc.TimeInForce is null ? null : (TimeInForceEnum)Enum.Parse(typeof(TimeInForceEnum), Enum.GetName(inc.TimeInForce.Value));
        main.Type = inc.Type is null ? null : (OrderTypesEnum)Enum.Parse(typeof(OrderTypesEnum), Enum.GetName(inc.Type.Value));
        main.Currency = inc.Currency is null ? null : (CurrenciesTypesEnum)Enum.Parse(typeof(CurrenciesTypesEnum), Enum.GetName(inc.Currency.Value));
        main.Side = (SidesEnum)Enum.Parse(typeof(SidesEnum), Enum.GetName(inc.Side));
        main.MarginMode = inc.MarginMode is null ? null : (MarginModesEnum)Enum.Parse(typeof(MarginModesEnum), Enum.GetName(inc.MarginMode.Value));

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
    public static InstrumentTradeStockSharpModel Bind(this InstrumentTradeStockSharpModel main, Security inc)
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
        main.IdRemote = inc.Id;

        main.Currency = inc.Currency is null ? null : (CurrenciesTypesEnum)Enum.Parse(typeof(CurrenciesTypesEnum), Enum.GetName(inc.Currency.Value));
        main.UnderlyingSecurityType = inc.UnderlyingSecurityType is null ? null : (InstrumentsStockSharpTypesEnum)Enum.Parse(typeof(InstrumentsStockSharpTypesEnum), Enum.GetName(inc.UnderlyingSecurityType.Value));
        main.TypeInstrument = inc.Type is null ? null : (InstrumentsStockSharpTypesEnum)Enum.Parse(typeof(InstrumentsStockSharpTypesEnum), Enum.GetName(inc.Type.Value));
        main.SettlementType = inc.SettlementType is null ? null : (SettlementTypesEnum)Enum.Parse(typeof(SettlementTypesEnum), Enum.GetName(inc.SettlementType.Value));
        main.OptionType = inc.OptionType is null ? null : (OptionInstrumentTradeTypesEnum)Enum.Parse(typeof(OptionInstrumentTradeTypesEnum), Enum.GetName(inc.OptionType.Value));
        main.OptionStyle = inc.OptionStyle is null ? null : (OptionTradeInstrumentStylesEnum)Enum.Parse(typeof(OptionTradeInstrumentStylesEnum), Enum.GetName(inc.OptionStyle.Value));

        main.Board = new BoardStockSharpModel().Bind(inc.Board);

        return main;
    }

    /// <inheritdoc/>
    public static BoardStockSharpModel Bind(this BoardStockSharpModel main, ExchangeBoard inc)
    {
        main.Code = inc.Code;
        main.Exchange = new ExchangeStockSharpModel().Bind(inc.Exchange);
        return main;
    }

    /// <inheritdoc/>
    public static ExchangeStockSharpModel Bind(this ExchangeStockSharpModel main, Exchange inc)
    {
        main.Name = inc.Name;
        main.CountryCode = inc.CountryCode is null ? null : (CountryCodesEnum)Enum.Parse(typeof(CountryCodesEnum), Enum.GetName(inc.CountryCode.Value));
        return main;
    }

    /// <inheritdoc/>
    public static PortfolioStockSharpModel Bind(this PortfolioStockSharpModel main, Portfolio inc)
    {
        main.Board = inc.Board is null ? null : new BoardStockSharpModel().Bind(inc.Board);

        main.Currency = inc.Currency is null ? null : (CurrenciesTypesEnum)Enum.Parse(typeof(CurrenciesTypesEnum), Enum.GetName(inc.Currency.Value));
        main.State = inc.State is null ? null : (PortfolioStatesEnum)Enum.Parse(typeof(PortfolioStatesEnum), Enum.GetName(inc.State.Value));

        main.ClientCode = inc.ClientCode;
        main.Name = inc.Name;
        main.DepoName = inc.DepoName;
        //
        return main;
    }
}