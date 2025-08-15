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

    /// <summary>
    /// TypeInstrument
    /// </summary>
    /// <remarks>
    /// <see cref="InstrumentsStockSharpTypesEnum"/>: Swap, Repo, Volatility, Strategy, Indicator, Receipt,
    /// Gdr, Spread, Loan, MultiLeg, Etf, CryptoCurrency, Adr, Fund, Weather, News, Cfd,
    /// Commodity, Forward, Warrant, Bond, Currency, Index, Option, Future, Stock
    /// </remarks>
    public int TypeInstrument { get; set; }

    /// <summary>
    /// Underlying security type .
    /// </summary>
    /// <remarks>
    /// <see cref="InstrumentsStockSharpTypesEnum"/>: Swap, Repo, Volatility, Strategy, Indicator, Receipt,
    /// Gdr, Spread, Loan, MultiLeg, Etf, CryptoCurrency, Adr, Fund, Weather, News, Cfd,
    /// Commodity, Forward, Warrant, Bond, Currency, Index, Option, Future, Stock
    /// </remarks>
    public int UnderlyingSecurityType { get; set; }

    /// <summary>
    /// <see cref="CurrenciesTypesEnum"/>
    /// </summary>
    public int Currency { get; set; }

    /// <summary>
    /// Security class.
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// ID in ISIN format (International Securities Identification Number).
    /// </summary>
    public string ISIN { get; set; }

    /// <summary>
    /// Minimum price step.
    /// </summary>
    public decimal? PriceStep { get; set; }

    /// <summary>
    /// Minimum volume step.
    /// </summary>
    public decimal? VolumeStep { get; set; }

    /// <summary>
    /// Minimum volume allowed in order.
    /// </summary>
    public decimal? MinVolume { get; set; }

    /// <summary>
    /// Maximum volume allowed in order.
    /// </summary>
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
    public string CfiCode { get; set; }

    /// <summary>
    /// Face value.
    /// </summary>
    public decimal? FaceValue { get; set; }

    /// <summary>
    /// Settlement type.
    /// </summary>
    /// <remarks>
    /// <see cref="SettlementTypesEnum"/>: Delivery, Cash
    /// </remarks>
    public int SettlementType { get; set; }

    /// <summary>
    /// OptionStyle
    /// </summary>
    /// <remarks>
    /// <see cref="OptionTradeInstrumentStylesEnum"/>: European, American, Exotic
    /// </remarks>
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
    /// <remarks>
    /// <see cref="OptionInstrumentTradeTypesEnum"/>: Call, Put
    /// </remarks>
    public int OptionType { get; set; }

    /// <summary>
    /// Can have short positions.
    /// </summary>
    public bool? Shortable { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{IdRemote} '{Name}' {(CurrenciesTypesEnum)Currency}";
    }
}