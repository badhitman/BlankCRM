////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// InstrumentTradeModelDB
/// </summary>
[Index(nameof(IsFavorite)), Index(nameof(IdRemote)), Index(nameof(Code)), Index(nameof(Class)), Index(nameof(CfiCode)), Index(nameof(UnderlyingSecurityId)), Index(nameof(PrimaryId)), Index(nameof(LastUpdatedAtUTC))]
public class InstrumentStockSharpModelDB : InstrumentTradeStockSharpModel, IBaseStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Добавлен в "Избранное"
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <inheritdoc/>
    public new BoardStockSharpModelDB Board { get; set; }
    /// <inheritdoc/>
    public int BoardId { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public void SetUpdate(InstrumentTradeStockSharpModel req)
    {
        LastUpdatedAtUTC = DateTime.UtcNow;
        ExpiryDate = req.ExpiryDate;
        SettlementDate = req.SettlementDate;
        OptionStyle = req.OptionStyle;
        Currency = req.Currency;
        OptionType = req.OptionType;
        UnderlyingSecurityType = req.UnderlyingSecurityType;
        PrimaryId = req.PrimaryId;
        UnderlyingSecurityId = req.UnderlyingSecurityId;
        TypeInstrument = req.TypeInstrument;
        ShortName = req.ShortName;
        Shortable = req.Shortable;
        SettlementType = req.SettlementType;
        Name = req.Name;
        Multiplier = req.Multiplier;
        IdRemote = req.IdRemote;
        FaceValue = req.FaceValue;
        Decimals = req.Decimals;
        Code = req.Code;
        Class = req.Class;
        CfiCode = req.CfiCode;
    }
}