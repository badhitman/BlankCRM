////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// InstrumentTradeModel
/// </summary>
public partial class InstrumentTradeStockSharpModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <inheritdoc/>
    public virtual BoardStockSharpModel Board { get; set; }

    /// <inheritdoc/>
    public string IdRemote { get; set; }

    /// <inheritdoc/>
    public string Code { get; set; }

    /// <inheritdoc/>
    public string ShortName { get; set; }

    /// <inheritdoc/>
    public int TypeInstrument { get; set; }

    /// <inheritdoc/>
    public int Currency { get; set; }

    /// <inheritdoc/>
    public string Class { get; set; }

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
    public string CfiCode { get; set; }

    /// <summary>
    /// Face value.
    /// </summary>
    public decimal? FaceValue { get; set; }

    /// <inheritdoc/>
    public int SettlementType { get; set; }

    /// <summary>
    /// OptionStyle
    /// </summary>

    public int OptionStyle { get; set; }

    /// <summary>
    /// Identifier on primary exchange.
    /// </summary>
    public string PrimaryId { get; set; }

    /// <summary>
    /// Underlying asset on which the current security is built.
    /// </summary>
    public string UnderlyingSecurityId { get; set; }

    /// <summary>
    /// Option type.
    /// </summary>
    public int OptionType { get; set; }

    /// <summary>
    /// Underlying security type.
    /// </summary>
    public InstrumentsStockSharpTypesEnum UnderlyingSecurityType { get; set; }

    /// <summary>
    /// Can have short positions.
    /// </summary>
    public bool? Shortable { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"#{Code} '{Name}' {(CurrenciesTypesEnum)Currency} /{Class}";
    }
}