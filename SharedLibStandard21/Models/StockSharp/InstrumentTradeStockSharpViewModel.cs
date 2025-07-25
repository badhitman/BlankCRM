////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
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
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// StateInstrument
    /// </summary>
    public int StateInstrument { get; set; } = (int)ObjectStatesEnum.Default;

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public virtual List<MarkerInstrumentStockSharpViewModel> Markers { get; set; }

    /// <inheritdoc/>
    public new BoardStockSharpViewModel Board { get; set; }


    #region manual properties

    /// <summary>
    /// <see cref="BondsTypesInstrumentsManualEnum"/>
    /// </summary>
    public int BondTypeInstrumentManual { get; set; }

    /// <summary>
    /// <see cref="TypesInstrumentsManualEnum"/>
    /// </summary>
    public int TypeInstrumentManual { get; set; }

    /// <inheritdoc/>
    public string ISIN { get; set; }

    /// <inheritdoc/>
    public DateTime IssueDate { get; set; }

    /// <inheritdoc/>
    public DateTime MaturityDate { get; set; }

    /// <inheritdoc/>
    public decimal CouponRate { get; set; }

    /// <inheritdoc/>
    public decimal LastFairPrice { get; set; }

    /// <inheritdoc/>
    public string Comment { get; set; }

    #endregion


    /// <inheritdoc/>
    public virtual void Reload(InstrumentTradeStockSharpViewModel model)
    {
        Markers = model.Markers;

        Board = model.Board;
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
        IdRemote = model.IdRemote;
        FaceValue = model.FaceValue;
        ExpiryDate = model.ExpiryDate;
        Decimals = model.Decimals;
        Currency = model.Currency;
        Code = model.Code;
        Class = model.Class;
        CfiCode = model.CfiCode;

        LastUpdatedAtUTC = model.LastUpdatedAtUTC;
        Id = model.Id;
        CreatedAtUTC = model.CreatedAtUTC;
    }
}