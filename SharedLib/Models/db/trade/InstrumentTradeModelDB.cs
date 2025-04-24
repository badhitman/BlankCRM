////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// InstrumentTradeModelDB
/// </summary>
[Index(nameof(IsFavorite))]
public class InstrumentTradeModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    public required string IdRemote { get; set; }

    /// <inheritdoc/>
    public required string Code { get; set; }

    /// <inheritdoc/>
    public required string ShortName { get; set; }

    /// <summary>
    /// Добавлен в "Избранное"
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <inheritdoc/>
    public ExchangeBoardModelDB? ExchangeBoard { get; set; }
    /// <inheritdoc/>
    public int ExchangeBoardId { get; set; }

    /// <inheritdoc/>
    public virtual InstrumentsStockSharpTypesEnum? TypeInstrument { get; set; }

    /// <inheritdoc/>
    public CurrenciesTypesEnum? Currency { get; set; }

    /// <inheritdoc/>
    public required InstrumentExternalIdModelDB? ExternalId { get; set; }

    /// <inheritdoc/>
    public required string Class { get; set; }

    /// <summary>
    /// Lot multiplier.
    /// </summary>
    public decimal? Multiplier { get; set; }

    /// <summary>
    /// Number of digits in price after coma.
    /// </summary>
    public int? Decimals { get; set; }

    /// <summary>
    /// Security expiration date (for derivatives - expiration, for bonds — redemption).
    /// </summary>
    public DateTimeOffset? ExpiryDate { get; set; }

    /// <summary>
    /// Settlement date for security (for derivatives and bonds).
    /// </summary>
    public DateTimeOffset? SettlementDate { get; set; }

    /// <summary>
    /// Type in ISO 10962 standard.
    /// </summary>
    public required string CfiCode { get; set; }

    /// <summary>
    /// Face value.
    /// </summary>
    public decimal? FaceValue { get; set; }

    /// <inheritdoc/>
    public SettlementTypesEnum? SettlementType { get; set; }

    /// <summary>
    /// OptionStyle
    /// </summary>

    public OptionTradeInstrumentStylesEnum? OptionStyle { get; set; }

    /// <summary>
    /// Identifier on primary exchange.
    /// </summary>
    public required string PrimaryId { get; set; }

    /// <summary>
    /// Underlying asset on which the current security is built.
    /// </summary>
    public required string UnderlyingSecurityId { get; set; }

    /// <summary>
    /// Option type.
    /// </summary>
    public OptionInstrumentTradeTypesEnum? OptionType { get; set; }

    /// <summary>
    /// Can have short positions.
    /// </summary>
    public bool? Shortable { get; set; }

    /// <summary>
    /// Underlying security type.
    /// </summary>
    public InstrumentsStockSharpTypesEnum? UnderlyingSecurityType { get; set; }
}