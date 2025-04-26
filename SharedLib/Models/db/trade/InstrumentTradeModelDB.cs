////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// InstrumentTradeModelDB
/// </summary>
[Index(nameof(IsFavorite)), Index(nameof(IdRemote)), Index(nameof(Code)), Index(nameof(Class)), Index(nameof(CfiCode)), Index(nameof(UnderlyingSecurityId)), Index(nameof(PrimaryId)), Index(nameof(LastAtUpdatedUTC))]
public class InstrumentTradeModelDB : InstrumentTradeModel, IBaseStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public new InstrumentExternalIdModelDB? ExternalId { get; set; }

    /// <inheritdoc/>
    public new ExchangeBoardModelDB? ExchangeBoard { get; set; }
    /// <inheritdoc/>
    public int ExchangeBoardId { get; set; }

    /// <inheritdoc/>
    public new required string IdRemote { get; set; }

    /// <inheritdoc/>
    public new required string Code { get; set; }

    /// <inheritdoc/>
    public new required string ShortName { get; set; }

    /// <inheritdoc/>
    public new required string Class { get; set; }

    /// <summary>
    /// Type in ISO 10962 standard.
    /// </summary>
    public new required string CfiCode { get; set; }

    /// <summary>
    /// Identifier on primary exchange.
    /// </summary>
    public new required string PrimaryId { get; set; }

    /// <summary>
    /// Underlying asset on which the current security is built.
    /// </summary>
    public new required string UnderlyingSecurityId { get; set; }

    /// <inheritdoc/>
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}