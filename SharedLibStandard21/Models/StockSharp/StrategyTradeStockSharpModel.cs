////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// StrategyTradeStockSharpModel
/// </summary>
public partial class StrategyTradeStockSharpModel : InstrumentTradeStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    public int Id { get; set; }

    /// <inheritdoc/>
    public decimal BasePrice { get; set; }

    /// <inheritdoc/>
    public decimal ValueOperation { get; set; }

    /// <inheritdoc/>
    public decimal ShiftPosition { get; set; }

    /// <inheritdoc/>
    public bool IsMM { get; set; }

    /// <inheritdoc/>
    public decimal L1 { get; set; }

    /// <inheritdoc/>
    public decimal L2 { get; set; }

    /// <inheritdoc/>
    public static StrategyTradeStockSharpModel Build(InstrumentTradeStockSharpViewModel instrument, decimal basePrice, decimal valueOperation, decimal shiftPosition, bool isMom, decimal l1, decimal l2)
    {
        return new()
        {
            Id = instrument.Id,
            Board = instrument.Board,
            CfiCode = instrument.CfiCode,
            Class = instrument.Class,
            Code = instrument.Code,
            Currency = instrument.Currency,
            Decimals = instrument.Decimals,
            ExpiryDate = instrument.ExpiryDate,
            FaceValue = instrument.FaceValue,
            IdRemote = instrument.IdRemote,
            Multiplier = instrument.Multiplier,
            Name = instrument.Name,
            OptionStyle = instrument.OptionStyle,
            OptionType = instrument.OptionType,
            PrimaryId = instrument.PrimaryId,
            SettlementDate = instrument.SettlementDate,
            SettlementType = instrument.SettlementType,
            Shortable = instrument.Shortable,
            ShortName = instrument.ShortName,
            TypeInstrument = instrument.TypeInstrument,
            UnderlyingSecurityId = instrument.UnderlyingSecurityId,
            UnderlyingSecurityType = instrument.UnderlyingSecurityType,
            //
            BasePrice = basePrice,
            ValueOperation = valueOperation,
            ShiftPosition = shiftPosition,
            IsMM = isMom,
            L1 = l1,
            L2 = l2,
        };
    }

    /// <inheritdoc/>
    public void Reload(StrategyTradeStockSharpModel strategyTrade, InstrumentTradeStockSharpViewModel instrument)
    {
        Id = instrument.Id;
        Board = instrument.Board;
        CfiCode = instrument.CfiCode;
        Class = instrument.Class;
        Code = instrument.Code;
        Currency = instrument.Currency;
        Decimals = instrument.Decimals;
        ExpiryDate = instrument.ExpiryDate;
        FaceValue = instrument.FaceValue;
        IdRemote = instrument.IdRemote;
        Multiplier = instrument.Multiplier;
        Name = instrument.Name;
        OptionStyle = instrument.OptionStyle;
        OptionType = instrument.OptionType;
        PrimaryId = instrument.PrimaryId;
        SettlementDate = instrument.SettlementDate;
        SettlementType = instrument.SettlementType;
        Shortable = instrument.Shortable;
        ShortName = instrument.ShortName;
        TypeInstrument = instrument.TypeInstrument;
        UnderlyingSecurityId = instrument.UnderlyingSecurityId;
        UnderlyingSecurityType = instrument.UnderlyingSecurityType;
        //
        BasePrice = strategyTrade.BasePrice;
        ValueOperation = strategyTrade.ValueOperation;
        ShiftPosition = strategyTrade.ShiftPosition;
        IsMM = strategyTrade.IsMM;
        L1 = strategyTrade.L1;
        L2 = strategyTrade.L2;
    }
}