////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InstrumentTradeModelDB
/// </summary>
public class InstrumentTradeModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    public required string IdRemote { get; set; }

    /// <inheritdoc/>
    public required string Code { get; set; }

    /// <inheritdoc/>
    public required string ShortName { get; set; }

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

    /// <inheritdoc/>
    public decimal? PriceStep { get; set; }

    /// <inheritdoc/>
    public decimal? VolumeStep { get; set; }

    /// <inheritdoc/>
    public decimal? MinVolume { get; set; }

    /// <inheritdoc/>
    public decimal? MaxVolume { get; set; }

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

    /// <summary>
    /// To copy fields of the current instrument to destination.
    /// </summary>
    /// <param name="destination">The instrument in which you should to copy fields.</param>
    public void CopyTo(InstrumentTradeModelDB destination)
    {
        ArgumentNullException.ThrowIfNull(destination);

        destination.Id = Id;
        destination.Name = Name;
        destination.Code = Code;
        destination.Class = Class;
        destination.ShortName = ShortName;
        destination.VolumeStep = VolumeStep;
        destination.MinVolume = MinVolume;
        destination.MaxVolume = MaxVolume;
        destination.Multiplier = Multiplier;
        destination.PriceStep = PriceStep;
        destination.Decimals = Decimals;
        destination.SettlementDate = SettlementDate;
        destination.ExpiryDate = ExpiryDate;
        destination.OptionType = OptionType;
        destination.UnderlyingSecurityId = UnderlyingSecurityId;
        //destination.ExternalId = ExternalId.Clone();
        destination.Currency = Currency;
        destination.CfiCode = CfiCode;
        destination.UnderlyingSecurityType = UnderlyingSecurityType;
        destination.Shortable = Shortable;
        destination.FaceValue = FaceValue;
        destination.SettlementType = SettlementType;
        destination.OptionStyle = OptionStyle;
        destination.PrimaryId = PrimaryId;
    }
}