using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// MarkersInstrumentStockSharpEnum
/// </summary>
public enum MarkersInstrumentStockSharpEnum
{
    /// <inheritdoc/>
    [Description("IsNew")]
    IsNew = 10,

    /// <inheritdoc/>
    [Description("Illiquid")]
    Illiquid = 20,

    /// <inheritdoc/>
    [Description("IsMarketMaker")]
    IsMarketMaker = 30,

    /// <summary>
    /// IsFavorite
    /// </summary>
    [Description("IsFavorite")]
    IsFavorite = 40,

    /// <summary>
    /// IsDisabled
    /// </summary>
    [Description("IsDisabled")]
    IsDisabled = 50,
}