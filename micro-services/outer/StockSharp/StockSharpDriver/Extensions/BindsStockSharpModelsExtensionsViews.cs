////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// связь моделей StockSharp с локальными моделями
/// </summary>
public static class BindsStockSharpModelsExtensionsViews
{
    public static InstrumentTradeStockSharpModel Bind(this InstrumentTradeStockSharpModel main, InstrumentStockSharpModelDB inc)
    {
        main.Board = new BoardStockSharpModel().Bind(inc.Board);

        main.Currency = inc.Currency;
        main.Multiplier = inc.Multiplier;
        main.CfiCode = inc.CfiCode;
        main.Name = inc.Name;
        main.Class = inc.Class;
        main.UnderlyingSecurityType = inc.UnderlyingSecurityType;
        main.UnderlyingSecurityId = inc.UnderlyingSecurityId;
        main.TypeInstrument = inc.TypeInstrument;
        main.ShortName = inc.ShortName;
        main.Shortable = inc.Shortable;
        main.SettlementType = inc.SettlementType;
        main.SettlementDate = inc.SettlementDate;
        main.PrimaryId = inc.PrimaryId;
        main.OptionType = inc.OptionType;
        main.OptionStyle = inc.OptionStyle;
        main.IdRemote = inc.IdRemote;
        main.FaceValue = inc.FaceValue;
        main.ExpiryDate = inc.ExpiryDate;
        main.Decimals = inc.Decimals;
        main.Code = inc.Code;
        return main;
    }

    public static BoardStockSharpModel Bind(this BoardStockSharpModel main, BoardStockSharpModelDB inc)
    {
        main.Code = inc.Code;
        main.Exchange = new ExchangeStockSharpModel().Bind(inc.Exchange);
        return main;
    }

    public static ExchangeStockSharpModel Bind(this ExchangeStockSharpModel main, ExchangeStockSharpModelDB inc)
    {
        main.CountryCode = inc.CountryCode;
        main.Name = inc.Name;
        return main;
    }
}