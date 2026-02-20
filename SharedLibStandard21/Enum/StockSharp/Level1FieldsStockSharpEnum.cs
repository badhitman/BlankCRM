////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Level1 fields of market-data.
/// </summary>
public enum Level1FieldsStockSharpEnum
{
    /// <summary>
    /// Opening price.
    /// </summary>
    [Display( Name = "OpenPrice")]
    OpenPrice,
    
    /// <summary>
    /// Highest price.
    /// </summary>
    [Display( Name = "HighPrice")]
    HighPrice,
    
    /// <summary>
    /// Lowest price.
    /// </summary>
    [Display( Name = "LowPrice")]
    LowPrice,
    
    /// <summary>
    /// Closing price.
    /// </summary>
    [Display( Name = "ClosingPrice")]
    ClosePrice,
    
    /// <summary>
    /// Step price.
    /// </summary>
    [Display( Name = "StepPrice")]
    StepPrice,
    
    /// <summary>
    /// Volatility (implied).
    /// </summary>
    [Display( Name = "ImpliedVolatility")]
    ImpliedVolatility,
    
    /// <summary>
    /// Theoretical price.
    /// </summary>
    [Display( Name = "TheorPrice")]
    TheorPrice,
    
    /// <summary>
    /// Open interest.
    /// </summary>
    [Display( Name = "OpenInterest")]
    OpenInterest,
    /// <summary>
    /// Price (min).
    /// </summary>
    [Display( Name = "PriceMin")]
    MinPrice,
    /// <summary>
    /// Price (max).
    /// </summary>
    [Display( Name = "PriceMax")]
    MaxPrice,
    /// <summary>
    /// Bids volume.
    /// </summary>
    [Display( Name = "BidsVolume")]
    BidsVolume,
    /// <summary>
    /// Number of bids.
    /// </summary>
    [Display( Name = "BidsCount")]
    BidsCount,
    /// <summary>
    /// Ask volume.
    /// </summary>
    [Display( Name = "AsksVolume")]
    AsksVolume,
    /// <summary>
    /// Number of asks.
    /// </summary>
    [Display( Name = "AsksCount")]
    AsksCount,
    /// <summary>
    /// Volatility (historical).
    /// </summary>
    [Display( Name = "HistoricalVolatility")]
    HistoricalVolatility,
    /// <summary>
    /// Delta.
    /// </summary>
    [Display( Name = "Delta")]
    Delta,
    /// <summary>
    /// Gamma.
    /// </summary>
    [Display( Name = "Gamma")]
    Gamma,
    /// <summary>
    /// Vega.
    /// </summary>
    [Display( Name = "Vega")]
    Vega,
    /// <summary>
    ///  Theta.
    /// </summary>
    [Display( Name = "Theta")]
    Theta,
    /// <summary>
    /// Initial margin to buy.
    /// </summary>
    [Display( Name = "MarginBuy")]
    MarginBuy,
    /// <summary>
    /// Initial margin to sell.
    /// </summary>
    [Display( Name = "MarginSell")]
    MarginSell,
    /// <summary>
    /// Minimum price step.
    /// </summary>
    [Display( Name = "PriceStep")]
    PriceStep,
    /// <summary>
    /// Minimum volume step.
    /// </summary>
    [Display( Name = "VolumeStep")]
    VolumeStep,

    /// <summary>
    /// State.
    /// </summary>
    [Display( Name = "State")]
    State,
    /// <summary>
    /// Last trade price.
    /// </summary>
    [Display( Name = "LastTradePrice")]
    LastTradePrice,
    /// <summary>
    /// Last trade volume.
    /// </summary>
    [Display( Name = "LastTradeVolume")]
    LastTradeVolume,
    /// <summary>
    /// Volume per session.
    /// </summary>
    [Display( Name = "VolumePerSession")]
    Volume,
    /// <summary>
    /// Average price per session.
    /// </summary>
    [Display( Name = "AveragePricePerSession")]
    AveragePrice,
    /// <summary>
    /// Settlement price.
    /// </summary>
    [Display( Name = "SettlementPrice")]
    SettlementPrice,
    /// <summary>
    /// Change,%.
    /// </summary>
    [Display( Name = "Change")]
    Change,
    /// <summary>
    /// Best bid price.
    /// </summary>
    [Display( Name = "BestBidPrice")]
    BestBidPrice,
    /// <summary>
    /// Best buy volume.
    /// </summary>
    [Display( Name = "BestBidVolume")]
    BestBidVolume,
    /// <summary>
    /// Best ask price.
    /// </summary>
    [Display( Name = "BestAskPrice")]
    BestAskPrice,
    /// <summary>
    /// Best sell volume.
    /// </summary>
    [Display( Name = "BestAskVolume")]
    BestAskVolume,
    /// <summary>
    /// Rho.
    /// </summary>
    [Display( Name = "Rho")]
    Rho,
    /// <summary>
    /// Accrued coupon income (ACI).
    /// </summary>
    [Display( Name = "AccruedCouponIncome")]
    AccruedCouponIncome,
    /// <summary>
    /// Maximum bid during the session.
    /// </summary>
    [Display( Name = "BidMax")]
    HighBidPrice,
    /// <summary>
    /// Minimum ask during the session.
    /// </summary>
    [Display( Name = "AskMin")]
    LowAskPrice,
    /// <summary>
    /// Yield.
    /// </summary>
    [Display( Name = "Yield")]
    Yield,
    /// <summary>
    /// Time of last trade.
    /// </summary>
    [Display( Name = "LastTradeTime")]
    LastTradeTime,
    /// <summary>
    /// Number of trades.
    /// </summary>
    [Display( Name = "NumOfTrades")]
    TradesCount,
    /// <summary>
    /// Average price.
    /// </summary>
    [Display( Name = "AveragePrice")]
    VWAP,
    /// <summary>
    /// Last trade ID.
    /// </summary>
    [Display( Name = "LastTradeId")]
    LastTradeId,
    /// <summary>
    /// Best bid time.
    /// </summary>
    [Display( Name = "BestBidTime")]
    BestBidTime,
    /// <summary>
    /// Best ask time.
    /// </summary>
    [Display( Name = "BestAskTime")]
    BestAskTime,
    /// <summary>
    /// Is tick ascending or descending in price.
    /// </summary>
    [Display( Name = "Trend")]
    LastTradeUpDown,
    /// <summary>
    /// Initiator of the last trade (buyer or seller).
    /// </summary>
    [Display( Name = "Initiator")]
    LastTradeOrigin,
    /// <summary>
    /// Lot multiplier.
    /// </summary>
    [Display( Name = "Lot")]
    Multiplier,
    /// <summary>
    /// Price/profit.
    /// </summary>
    [Display(Name = "P/E")]
    PriceEarnings,
    /// <summary>
    /// Price target/profit.
    /// </summary>
    [Display(Name = "Forward P/E")]
    ForwardPriceEarnings,
    /// <summary>
    /// Price/profit (increase).
    /// </summary>
    [Display(Name = "PEG")]
    PriceEarningsGrowth,
    /// <summary>
    /// Price/buy.
    /// </summary>
    [Display(Name = "P/S")]
    PriceSales,
    /// <summary>
    /// Price/sell.
    /// </summary>
    [Display(Name = "P/B")]
    PriceBook,
    /// <summary>
    /// Price/amount.
    /// </summary>
    [Display(Name = "P/CF")]
    PriceCash,
    /// <summary>
    /// Price/amount (free).
    /// </summary>
    [Display(Name = "P/FCF")]
    PriceFreeCash,
    /// <summary>
    /// Payments.
    /// </summary>
    [Display(Name = "Payout")]
    Payout,
    /// <summary>
    /// Number of shares.
    /// </summary>
    [Display( Name = "NumOfShares")]
    SharesOutstanding,
    /// <summary>
    /// Shares Float.
    /// </summary>
    [Display(Name = "Shares Float")]
    SharesFloat,
    /// <summary>
    /// Float Short.
    /// </summary>
    [Display(Name = "Float Short")]
    FloatShort,
    /// <summary>
    /// Short.
    /// </summary>
    [Display(Name = "Short")]
    ShortRatio,
    /// <summary>
    /// Return on assets.
    /// </summary>
    [Display(Name = "ROA")]
    ReturnOnAssets,
    /// <summary>
    /// Return on equity.
    /// </summary>
    [Display(Name = "ROE")]
    ReturnOnEquity,
    /// <summary>
    /// Return on investment.
    /// </summary>
    [Display(Name = "ROI")]
    ReturnOnInvestment,
    /// <summary>
    /// Liquidity (current).
    /// </summary>
    [Display( Name = "CurrentRatio")]
    CurrentRatio,
    /// <summary>
    /// Liquidity (instantaneous).
    /// </summary>
    [Display( Name = "QuickRatio")]
    QuickRatio,
    /// <summary>
    /// Capital (long-term debt).
    /// </summary>
    [Display( Name = "LongTermDebtEquity")]
    LongTermDebtEquity,
    /// <summary>
    /// Capital (debt).
    /// </summary>
    [Display( Name = "TotalDebtEquity")]
    TotalDebtEquity,
    /// <summary>
    /// Assets margin (gross).
    /// </summary>
    [Display( Name = "GrossMargin")]
    GrossMargin,
    /// <summary>
    /// Assets margin.
    /// </summary>
    [Display( Name = "OperatingMargin")]
    OperatingMargin,
    /// <summary>
    /// Profit margin.
    /// </summary>
    [Display( Name = "ProfitMargin")]
    ProfitMargin,
    /// <summary>
    /// Beta.
    /// </summary>
    [Display( Name = "Beta")]
    Beta,
    /// <summary>
    /// ATR.
    /// </summary>
    [Display(Name = "ATR")]
    AverageTrueRange,
    /// <summary>
    /// Volatility (week).
    /// </summary>
    [Display( Name = "VolatilityWeek")]
    HistoricalVolatilityWeek,
    /// <summary>
    /// Volatility (month).
    /// </summary>
    [Display( Name = "VolatilityMonth")]
    HistoricalVolatilityMonth,
    /// <summary>
    /// System info.
    /// </summary>
    [Display( Name = "System")]
    IsSystem,
    /// <summary>
    /// Number of digits in price after coma.
    /// </summary>
    [Display( Name = "Decimals")]
    Decimals,
    /// <summary>
    /// Duration.
    /// </summary>
    [Display( Name = "Duration")]
    Duration,
    /// <summary>
    /// Number of issued contracts.
    /// </summary>
    [Display( Name = "IssueSize")]
    IssueSize,
    /// <summary>
    /// BuyBack date.
    /// </summary>
    [Display( Name = "BuyBackDate")]
    BuyBackDate,
    /// <summary>
    /// BuyBack price.
    /// </summary>
    [Display( Name = "BuyBackPrice")]
    BuyBackPrice,
    /// <summary>
    /// Turnover.
    /// </summary>
    [Display( Name = "Turnover")]
    Turnover,
    /// <summary>
    /// The middle of spread.
    /// </summary>
    [Display( Name = "Spread")]
    SpreadMiddle,
    /// <summary>
    /// The dividend amount on shares.
    /// </summary>
    [Display( Name = "Dividend")]
    Dividend,
    /// <summary>
    /// Price after split.
    /// </summary>
    [Display( Name = "AfterSplit")]
    AfterSplit,
    /// <summary>
    /// Price before split.
    /// </summary>
    [Display( Name = "BeforeSplit")]
    BeforeSplit,
    /// <summary>
    /// Commission (taker).
    /// </summary>
    [Display( Name = "CommissionTaker")]
    CommissionTaker,
    /// <summary>
    /// Commission (maker).
    /// </summary>
    [Display( Name = "CommissionMaker")]
    CommissionMaker,
    /// <summary>
    /// Minimum volume allowed in order.
    /// </summary>
    [Display( Name = "MinVolume")]
    MinVolume,
    /// <summary>
    /// Minimum volume allowed in order for underlying security.
    /// </summary>
    [Display( Name = "UnderlyingMinVolume")]
    UnderlyingMinVolume,
    /// <summary>
    /// Coupon value.
    /// </summary>
    [Display( Name = "CouponValue")]
    CouponValue,
    /// <summary>
    /// Coupon date.
    /// </summary>
    [Display( Name = "CouponDate")]
    CouponDate,
    /// <summary>
    /// Coupon period.
    /// </summary>
    [Display( Name = "CouponPeriod")]
    CouponPeriod,
    /// <summary>
    /// Market price (yesterday).
    /// </summary>
    [Display( Name = "MarketPriceYesterday")]
    MarketPriceYesterday,
    /// <summary>
    /// Market price (today).
    /// </summary>
    [Display( Name = "MarketPriceToday")]
    MarketPriceToday,
    /// <summary>
    /// VWAP (prev).
    /// </summary>
    [Display( Name = "VWAPPrev")]
    VWAPPrev,
    /// <summary>
    /// Yield by VWAP.
    /// </summary>
    [Display( Name = "YieldVWAP")]
    YieldVWAP,
    /// <summary>
    /// Yield by VWAP (prev).
    /// </summary>
    [Display( Name = "YieldVWAPPrev")]
    YieldVWAPPrev,
    /// <summary>
    /// Index.
    /// </summary>
    [Display( Name = "Index")]
    Index,
    /// <summary>
    /// Imbalance.
    /// </summary>
    [Display( Name = "Imbalance")]
    Imbalance,
    /// <summary>
    /// Underlying price.
    /// </summary>
    [Display( Name = "Underlying")]
    UnderlyingPrice,
    /// <summary>
    /// Maximum volume allowed in order.
    /// </summary>
    [Display( Name = "MaxVolume")]
    MaxVolume,
    /// <summary>
    /// Lowest bid during the session.
    /// </summary>
    [Display( Name = "LowBidPrice", Description = "LowBidPriceDesc")]
    LowBidPrice,
    /// <summary>
    /// Highest ask during the session.
    /// </summary>
    [Display( Name = "HighAskPrice", Description = "HighAskPriceDesc")]
    HighAskPrice,
    /// <summary>
    /// Lowest last trade volume.
    /// </summary>
    [Display( Name = "LastTradeVolumeLow", Description = "LastTradeVolumeLowDesc")]
    LastTradeVolumeLow,
    /// <summary>
    /// Highest last trade volume.
    /// </summary>
    [Display( Name = "LastTradeVolumeHigh", Description = "LastTradeVolumeHighDesc")]
    LastTradeVolumeHigh,
    /// <summary>
    /// Option margin leverage.
    /// </summary>
    [Display( Name = "OptionMargin", Description = "OptionMarginDesc")]
    OptionMargin,
    /// <summary>
    /// Synthetic option position margin leverage.
    /// </summary>
    [Display( Name = "OptionSyntheticMargin", Description = "OptionSyntheticMarginDesc")]
    OptionSyntheticMargin,
    /// <summary>
    /// Volume of the lowest bid.
    /// </summary>
    [Display( Name = "LowBidVolume", Description = "LowBidVolumeDesc")]
    LowBidVolume,
    /// <summary>
    /// Volume of the highest ask.
    /// </summary>
    [Display( Name = "HighAskVolume", Description = "HighAskVolumeDesc")]
    HighAskVolume,
    /// <summary>
    /// Underlying asset best bid price.
    /// </summary>
    [Display( Name = "UnderlyingBestBidPrice", Description = "UnderlyingBestBidPriceDesc")]
    UnderlyingBestBidPrice,
    /// <summary>
    /// Underlying asset best ask price.
    /// </summary>
    [Display( Name = "UnderlyingBestAskPrice", Description = "UnderlyingBestAskPriceDesc")]
    UnderlyingBestAskPrice,
    /// <summary>
    /// Median price.
    /// </summary>
    [Display( Name = "Median", Description = "MedianPrice")]
    MedianPrice,
    /// <summary>
    /// The highest price for 52 weeks.
    /// </summary>
    [Display( Name = "HighPrice52Week", Description = "HighPrice52WeekDesc")]
    HighPrice52Week,
    /// <summary>
    /// The lowest price for 52 weeks.
    /// </summary>
    [Display( Name = "LowPrice52Week", Description = "LowPrice52WeekDesc")]
    LowPrice52Week,
    /// <summary>
    /// 
    /// </summary>
    [Display( Name = "LastTradeStringId", Description = "LastTradeStringIdDesc")]
    LastTradeStringId     
}
