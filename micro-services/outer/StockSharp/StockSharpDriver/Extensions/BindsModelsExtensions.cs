////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using StockSharp.BusinessEntities;
using SharedLib;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// связь моделей StockSharp с локальными моделями
/// </summary>
public static class BindsModelsExtensions
{
    /// <inheritdoc/>
    public static InstrumentTradeModel Bind(this InstrumentTradeModel main, Security inc)
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

        main.Currency = (CurrenciesTypesEnum?)Enum.Parse(typeof(CurrenciesTypesEnum), Enum.GetName(inc.Currency!.Value)!);
        main.UnderlyingSecurityType = (InstrumentsStockSharpTypesEnum)Enum.Parse(typeof(InstrumentsStockSharpTypesEnum), Enum.GetName(inc.UnderlyingSecurityType!.Value)!);
        main.TypeInstrument = (InstrumentsStockSharpTypesEnum)Enum.Parse(typeof(InstrumentsStockSharpTypesEnum), Enum.GetName(inc.Type!.Value)!);
        main.SettlementType = (SettlementTypesEnum)Enum.Parse(typeof(SettlementTypesEnum), Enum.GetName(inc.SettlementType!.Value)!);
        main.OptionType = (OptionInstrumentTradeTypesEnum)Enum.Parse(typeof(OptionInstrumentTradeTypesEnum), Enum.GetName(inc.OptionType!.Value)!);
        main.OptionStyle = (OptionTradeInstrumentStylesEnum)Enum.Parse(typeof(OptionTradeInstrumentStylesEnum), Enum.GetName(inc.OptionStyle!.Value)!);

        main.Board = new BoardStockSharpModel().Bind(inc.Board);
        main.ExternalId = new InstrumentExternalIdModel().Bind(inc.ExternalId);

        return main;
    }

    /// <inheritdoc/>
    public static InstrumentExternalIdModel Bind(this InstrumentExternalIdModel main, SecurityExternalId inc)
    {
        main.Cusip = inc.Cusip;
        main.Sedol = inc.Sedol;
        main.Bloomberg = inc.Bloomberg;
        main.Ric = inc.Ric;
        main.InteractiveBrokers = inc.InteractiveBrokers;
        main.IQFeed = inc.IQFeed;
        main.Isin = inc.Isin;
        main.Plaza = inc.Plaza;

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
        main.CountryCode = (CountryCodesEnum)Enum.Parse(typeof(CountryCodesEnum), Enum.GetName(inc.CountryCode!.Value)!);
        return main;
    }

    /// <inheritdoc/>
    public static PortfolioTradeModel Bind(this PortfolioTradeModel main, Portfolio inc)
    {
        main.Board = new BoardStockSharpModel().Bind(inc.Board);
        main.Currency = (CurrenciesTypesEnum?)Enum.Parse(typeof(CurrenciesTypesEnum), Enum.GetName(inc.Currency!.Value)!);
        main.ClientCode = inc.ClientCode;
        main.State = (PortfolioStatesEnum?)Enum.Parse(typeof(PortfolioStatesEnum), Enum.GetName(inc.State!.Value)!);
        main.Name = inc.Name;
        main.DepoName = inc.DepoName;
        //
        return main;
    }
}