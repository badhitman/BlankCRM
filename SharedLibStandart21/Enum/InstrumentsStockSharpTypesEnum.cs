////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// InstrumentsStockSharpTypesEnum
/// </summary>
public enum InstrumentsStockSharpTypesEnum
{
    /// <inheritdoc/>
    [Display(Name = "None")]
    None,

    /// <summary>
    /// Shares
    /// </summary>
    [Display(Name = "Stock")]
    Stock,

    /// <summary>
    /// Future contract.
    /// </summary>
    [Display(Name = "FutureContract")]
    Future,

    /// <summary>
    /// Options contract.
    /// </summary>
    [Display(Name = "OptionsContract")]
    Option,

    /// <summary>
    /// Index.
    /// </summary>
    [Display(Name = "Index")]
    Index,

    /// <summary>
    /// Currency.
    /// </summary>
    [Display(Name = "Currency")]
    Currency,

    /// <summary>
    /// Bond.
    /// </summary>
    [Display(Name = "Bond")]
    Bond,

    /// <summary>
    /// Warrant.
    /// </summary>
    [Display(Name = "Warrant")]
    Warrant,

    /// <summary>
    /// Forward.
    /// </summary>
    [Display(Name = "Forward")]
    Forward,

    /// <summary>
    /// Swap.
    /// </summary>
    [Display(Name = "Swap")]
    Swap,

    /// <summary>
    /// Commodity.
    /// </summary>
    [Display(Name = "Commodity")]
    Commodity,

    /// <summary>
    /// CFD.
    /// </summary>
    [Display(Name = "Cfd")]
    Cfd,

    /// <summary>
    /// News.
    /// </summary>
    [Display(Name = "News")]
    News,

    /// <summary>
    /// Weather.
    /// </summary>
    [Display(Name = "Weather")]
    Weather,

    /// <summary>
    /// Mutual funds.
    /// </summary>
    [Display(Name = "ShareFund")]
    Fund,

    /// <summary>
    /// American Depositary Receipts.
    /// </summary>
    [Display(Name = "Adr")]
    Adr,

    /// <summary>
    /// Cryptocurrency.
    /// </summary>
    [Display(Name = "Cryptocurrency")]
    CryptoCurrency,

    /// <summary>
    /// ETF.
    /// </summary>
    [Display(Name = "Etf")]
    Etf,

    /// <summary>
    /// Multi leg.
    /// </summary>
    [Display(Name = "Legs")]
    MultiLeg,

    /// <summary>
    /// Loan.
    /// </summary>
    [Display(Name = "Loan")]
    Loan,

    /// <summary>
    /// Spread.
    /// </summary>
    [Display(Name = "Spread")]
    Spread,

    /// <summary>
    /// Global Depositary Receipts.
    /// </summary>
    [Display(Name = "Gdr")]
    Gdr,

    /// <summary>
    /// Receipt.
    /// </summary>
    [Display(Name = "Receipt")]
    Receipt,

    /// <summary>
    /// Indicator.
    /// </summary>
    [Display(Name = "Indicator")]
    Indicator,

    /// <summary>
    /// Strategy.
    /// </summary>
    [Display(Name = "Strategy")]
    Strategy,

    /// <summary>
    /// Volatility.
    /// </summary>
    [Display(Name = "Volatility")]
    Volatility,

    /// <summary>
    /// REPO.
    /// </summary>
    [Display(Name = "Repo")]
    Repo
}