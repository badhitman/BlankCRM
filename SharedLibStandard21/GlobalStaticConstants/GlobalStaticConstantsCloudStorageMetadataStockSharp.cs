////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.IO;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace SharedLib;

/// <summary>
/// Cloud storage metadata
/// </summary>
public static partial class GlobalStaticCloudStorageMetadata
{
    static string STOCKSHARP_QUOTE_FORM = Path.Combine(Routes.STOCKSHARP_CONTROLLER_NAME, Routes.QUOTE_CONTROLLER_NAME, Routes.FORM_CONTROLLER_NAME);

    /// <inheritdoc/>
    public static readonly StorageMetadataModel MarkersDashboard = new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PrefixPropertyName = Routes.MARKERS_CONTROLLER_NAME,
        PropertyName = Routes.DASHBOARD_CONTROLLER_NAME,
    };

    /// <inheritdoc/>
    public static readonly StorageMetadataModel BoardsDashboard = new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PrefixPropertyName = Routes.BOARDS_CONTROLLER_NAME,
        PropertyName = Routes.DASHBOARD_CONTROLLER_NAME,
    };

    /// <inheritdoc/>
    public static StorageMetadataModel QuoteStrategyVolume => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.QUOTE_CONTROLLER_NAME, Routes.VOLUME_CONTROLLER_NAME),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };

    /// <inheritdoc/>
    public static StorageMetadataModel QuoteSizeStrategyVolume => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.QUOTE_CONTROLLER_NAME, Routes.SIZE_CONTROLLER_NAME, Routes.VOLUME_CONTROLLER_NAME),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };

    // bondOutOfRangePositionTraded

    /// <inheritdoc/>
    public static StorageMetadataModel BondPositionLimitTraded => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.BOND_CONTROLLER_NAME, Routes.POSITION_CONTROLLER_NAME, $"{Routes.LIMIT_CONTROLLER_NAME}-{Routes.TRADED_CONTROLLER_NAME}"),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };

    /// <inheritdoc/>
    public static StorageMetadataModel BondOutOfRangePositionLimitTraded => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.BOND_CONTROLLER_NAME, $"{Routes.OUT_CONTROLLER_NAME}-of-{Routes.RANGE_CONTROLLER_NAME}", Routes.POSITION_CONTROLLER_NAME, $"{Routes.LIMIT_CONTROLLER_NAME}-{Routes.TRADED_CONTROLLER_NAME}"),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };

    /// <inheritdoc/>
    public static readonly StorageMetadataModel BonusAmountStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "BonusAmount",
    };

    /// <summary>
    /// QuoteSmallStrategyBidVolume
    /// </summary>
    public static StorageMetadataModel QuoteSmallStrategyBidVolume => new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PropertyName = $"{Routes.QUOTE_CONTROLLER_NAME}-{Routes.SMALL_CONTROLLER_NAME}-{Routes.STRATEGY_CONTROLLER_NAME}-{Routes.BID_CONTROLLER_NAME}-{Routes.VOLUME_CONTROLLER_NAME}",
        PrefixPropertyName = Routes.DEFAULT_CONTROLLER_NAME
    };

    /// <summary>
    /// QuoteSmallStrategyOfferVolume
    /// </summary>
    public static StorageMetadataModel QuoteSmallStrategyOfferVolume => new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PropertyName = $"{Routes.QUOTE_CONTROLLER_NAME}-{Routes.SMALL_CONTROLLER_NAME}-{Routes.STRATEGY_CONTROLLER_NAME}-{Routes.OFFER_CONTROLLER_NAME}-{Routes.VOLUME_CONTROLLER_NAME}",
        PrefixPropertyName = Routes.DEFAULT_CONTROLLER_NAME
    };

    /// <summary>
    /// ProgramDataPathStockSharp
    /// </summary>
    public static StorageMetadataModel ProgramDataPathStockSharp => new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PropertyName = $"{Routes.PROGRAM_CONTROLLER_NAME}-{Routes.DATA_CONTROLLER_NAME}-{Routes.PATH_CONTROLLER_NAME}",
        PrefixPropertyName = Routes.DEFAULT_CONTROLLER_NAME
    };

    /// <summary>
    /// Client code assigned by the broker.
    /// </summary>
    public static StorageMetadataModel ClientCodeBrokerStockSharp => new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PropertyName = $"{Routes.CLIENT_CONTROLLER_NAME}-{Routes.CODE_CONTROLLER_NAME}",
        PrefixPropertyName = Routes.BROKER_CONTROLLER_NAME
    };

    /// <summary>
    /// TradeInstrumentStrategyStockSharp
    /// </summary>
    public static StorageMetadataModel TradeInstrumentStrategyStockSharp(int ownerId) => new()
    {
        ApplicationName = GlobalStaticConstantsTransmission.TransmissionQueues.TradeInstrumentStrategyStockSharpReceive,
        OwnerPrimaryKey = ownerId,
        PropertyName = $"{Routes.TRADE_CONTROLLER_NAME}-{Routes.STRATEGY_CONTROLLER_NAME}",
        PrefixPropertyName = Routes.BROKER_CONTROLLER_NAME
    };

    /// <summary>
    /// SecuritiesCriteriaCodeFilterStockSharp
    /// </summary>
    public static StorageMetadataModel SecuritiesCriteriaCodeFilterStockSharp => new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PropertyName = $"{Routes.SECURITIES_CONTROLLER_NAME}-{Routes.CRITERIA_CONTROLLER_NAME}-{Routes.LOOKUP_ACTION_NAME}",
        PrefixPropertyName = $"{Routes.DEFAULT_CONTROLLER_NAME}-{Routes.CODE_CONTROLLER_NAME}-{Routes.FILTER_CONTROLLER_NAME}"
    };

    /// <summary>
    /// BoardCriteriaCodeFilterStockSharp
    /// </summary>
    public static StorageMetadataModel BoardCriteriaCodeFilterStockSharp => new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PropertyName = $"{Routes.BOARDS_CONTROLLER_NAME}-{Routes.CRITERIA_CONTROLLER_NAME}-{Routes.LOOKUP_ACTION_NAME}",
        PrefixPropertyName = $"{Routes.DEFAULT_CONTROLLER_NAME}-{Routes.CODE_CONTROLLER_NAME}-{Routes.FILTER_CONTROLLER_NAME}"
    };
}