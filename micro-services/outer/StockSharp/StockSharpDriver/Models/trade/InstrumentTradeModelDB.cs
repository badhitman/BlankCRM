////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// InstrumentTradeModelDB
/// </summary>
[Index(nameof(IsFavorite)), Index(nameof(IdRemote)), Index(nameof(Code)), Index(nameof(Class)), Index(nameof(CfiCode)), Index(nameof(UnderlyingSecurityId)), Index(nameof(PrimaryId)), Index(nameof(LastAtUpdatedUTC))]
public class InstrumentTradeModelDB : InstrumentTradeStockSharpModel, IBaseStockSharpModel
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
    public new ExternalIdInstrumentModelDB ExternalId { get; set; }

    /// <inheritdoc/>
    public new BoardStockSharpModelDB Board { get; set; }
    /// <inheritdoc/>
    public int BoardId { get; set; }

    /// <inheritdoc/>
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}