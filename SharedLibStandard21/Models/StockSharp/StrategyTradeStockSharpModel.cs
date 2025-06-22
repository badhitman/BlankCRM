////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

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
    public decimal SmallBidVolume { get; set; }

    /// <inheritdoc/>
    public decimal SmallOfferVolume { get; set; }

    /// <inheritdoc/>
    public decimal SmallOffset { get; set; }

    /// <inheritdoc/>
    public decimal WorkingVolume { get; set; }

    /// <inheritdoc/>
    public bool IsSmall { get; set; }

    /// <inheritdoc/>
    public decimal LowLimit { get; set; }

    /// <inheritdoc/>
    public decimal HightLimit { get; set; }

    /// <inheritdoc/>
    public static StrategyTradeStockSharpModel Build(InstrumentTradeStockSharpViewModel instrument, decimal basePrice, decimal valueOperation, decimal shiftPosition, decimal smallBidVolume, decimal smallOfferVolume, decimal smallOffset, decimal workingVolume, bool isSmall, decimal lowLimit, decimal hightLimit)
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
            SmallBidVolume = smallBidVolume,
            SmallOfferVolume = smallOfferVolume,
            SmallOffset = smallOffset,
            WorkingVolume = workingVolume,
            IsSmall = isSmall,
            LowLimit = lowLimit,
            HightLimit = hightLimit,
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
        SmallBidVolume = strategyTrade.SmallBidVolume;
        SmallOfferVolume = strategyTrade.SmallOfferVolume;
        SmallOffset = strategyTrade.SmallOffset;
        WorkingVolume = strategyTrade.WorkingVolume;
        IsSmall = strategyTrade.IsSmall;
        LowLimit = strategyTrade.LowLimit;
        HightLimit = strategyTrade.HightLimit;
    }
}