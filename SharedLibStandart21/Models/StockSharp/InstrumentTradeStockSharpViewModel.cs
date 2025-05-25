////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// InstrumentTradeStockSharpViewModel
/// </summary>
public partial class InstrumentTradeStockSharpViewModel : InstrumentTradeStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// IsFavorite
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public void Reload(InstrumentTradeStockSharpViewModel model)
    {
        Board = model.Board;
        Id = model.Id;
        IsFavorite = model.IsFavorite;
        UnderlyingSecurityType = model.UnderlyingSecurityType;
        UnderlyingSecurityId = model.UnderlyingSecurityId;
        TypeInstrument = model.TypeInstrument;
        ShortName = model.ShortName;
        Shortable = model.Shortable;
        SettlementType = model.SettlementType;
        SettlementDate = model.SettlementDate;
        PrimaryId = model.PrimaryId;
        OptionType = model.OptionType;
        OptionStyle = model.OptionStyle;
        Name = model.Name;
        Multiplier = model.Multiplier;
        LastUpdatedAtUTC = model.LastUpdatedAtUTC;
        IdRemote = model.IdRemote;
        FaceValue = model.FaceValue;
        ExpiryDate = model.ExpiryDate;
        Decimals = model.Decimals;
        Currency = model.Currency;
        CreatedAtUTC = model.CreatedAtUTC;
        Code = model.Code;
        Class = model.Class;
        CfiCode = model.CfiCode;
    }
}