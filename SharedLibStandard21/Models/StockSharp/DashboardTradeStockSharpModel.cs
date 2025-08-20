////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// StrategyTradeStockSharpModel
/// </summary>
public partial class DashboardTradeStockSharpModel : InstrumentTradeStockSharpModel, IEquatable<DashboardTradeStockSharpModel>
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
    public decimal Offset { get; set; }

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
    public bool IsAlter { get; set; }

    /// <inheritdoc/>
    public decimal LowLimit { get; set; }

    /// <inheritdoc/>
    public decimal HightLimit { get; set; }

    /// <inheritdoc/>
    public static DashboardTradeStockSharpModel Build(InstrumentTradeStockSharpViewModel instrument, decimal basePrice, decimal valueOperation, decimal shiftPosition, decimal smallBidVolume, decimal smallOfferVolume, decimal smallOffset, decimal workingVolume, bool isSmall, bool isAlter, decimal lowLimit, decimal hightLimit)
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
            Offset = shiftPosition,
            SmallBidVolume = smallBidVolume,
            SmallOfferVolume = smallOfferVolume,
            SmallOffset = smallOffset,
            WorkingVolume = workingVolume,
            IsSmall = isSmall,
            IsAlter = isAlter,
            LowLimit = lowLimit,
            HightLimit = hightLimit,
        };
    }

    /// <inheritdoc/>
    public void Reload(DashboardTradeStockSharpModel strategyTrade, InstrumentTradeStockSharpViewModel instrument)
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
        Offset = strategyTrade.Offset;
        SmallBidVolume = strategyTrade.SmallBidVolume;
        SmallOfferVolume = strategyTrade.SmallOfferVolume;
        SmallOffset = strategyTrade.SmallOffset;
        WorkingVolume = strategyTrade.WorkingVolume;
        IsAlter = strategyTrade.IsAlter;
        IsSmall = strategyTrade.IsSmall;
        LowLimit = strategyTrade.LowLimit;
        HightLimit = strategyTrade.HightLimit;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is InstrumentTradeStockSharpModel _other)
        {
            return Equals(_other);
        }

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public bool Equals(DashboardTradeStockSharpModel other)
    {
        return other is not null &&
               Name == other.Name &&
               Board.Equals(other.Board) &&
               IdRemote == other.IdRemote &&
               Code == other.Code &&
               ShortName == other.ShortName &&
               TypeInstrument == other.TypeInstrument &&
               UnderlyingSecurityType == other.UnderlyingSecurityType &&
               Currency == other.Currency &&
               Class == other.Class &&
               PriceStep == other.PriceStep &&
               VolumeStep == other.VolumeStep &&
               MinVolume == other.MinVolume &&
               MaxVolume == other.MaxVolume &&
               Multiplier == other.Multiplier &&
               Decimals == other.Decimals &&
               EqualityComparer<DateTimeOffset?>.Default.Equals(ExpiryDate, other.ExpiryDate) &&
               EqualityComparer<DateTimeOffset?>.Default.Equals(SettlementDate, other.SettlementDate) &&
               CfiCode == other.CfiCode &&
               FaceValue == other.FaceValue &&
               SettlementType == other.SettlementType &&
               OptionStyle == other.OptionStyle &&
               PrimaryId == other.PrimaryId &&
               UnderlyingSecurityId == other.UnderlyingSecurityId &&
               OptionType == other.OptionType &&
               Shortable == other.Shortable &&
               Id == other.Id &&
               BasePrice == other.BasePrice &&
               ValueOperation == other.ValueOperation &&
               Offset == other.Offset &&
               SmallBidVolume == other.SmallBidVolume &&
               SmallOfferVolume == other.SmallOfferVolume &&
               SmallOffset == other.SmallOffset &&
               WorkingVolume == other.WorkingVolume &&
               IsSmall == other.IsSmall &&
               IsAlter == other.IsAlter &&
               LowLimit == other.LowLimit &&
               HightLimit == other.HightLimit;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Name);
        hash.Add(Board);
        hash.Add(IdRemote);
        hash.Add(Code);
        hash.Add(ShortName);
        hash.Add(TypeInstrument);
        hash.Add(UnderlyingSecurityType);
        hash.Add(Currency);
        hash.Add(Class);
        hash.Add(PriceStep);
        hash.Add(VolumeStep);
        hash.Add(MinVolume);
        hash.Add(MaxVolume);
        hash.Add(Multiplier);
        hash.Add(Decimals);
        hash.Add(ExpiryDate);
        hash.Add(SettlementDate);
        hash.Add(CfiCode);
        hash.Add(FaceValue);
        hash.Add(SettlementType);
        hash.Add(OptionStyle);
        hash.Add(PrimaryId);
        hash.Add(UnderlyingSecurityId);
        hash.Add(OptionType);
        hash.Add(Shortable);
        hash.Add(Id);
        hash.Add(BasePrice);
        hash.Add(ValueOperation);
        hash.Add(Offset);
        hash.Add(SmallBidVolume);
        hash.Add(SmallOfferVolume);
        hash.Add(SmallOffset);
        hash.Add(WorkingVolume);
        hash.Add(IsSmall);
        hash.Add(IsAlter);
        hash.Add(LowLimit);
        hash.Add(HightLimit);
        return hash.ToHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(DashboardTradeStockSharpModel left, DashboardTradeStockSharpModel right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator !=(DashboardTradeStockSharpModel left, DashboardTradeStockSharpModel right)
    {
        return !left.Equals(right);
    }
}