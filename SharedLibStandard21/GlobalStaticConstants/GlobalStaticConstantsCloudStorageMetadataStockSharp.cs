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
    static string STOCKSHARP_QUOTE_FORM = Path.Combine(Routes.STOCKSHARP_CONTROLLER_NAME, GlobalStaticConstantsRoutes.Routes.QUOTE_CONTROLLER_NAME, GlobalStaticConstantsRoutes.Routes.FORM_CONTROLLER_NAME);

    /// <inheritdoc/>
    public static readonly StorageMetadataModel MarkersDashboard = new()
    {
        ApplicationName = Routes.STOCKSHARP_CONTROLLER_NAME,
        PrefixPropertyName = "markers",
        PropertyName = "dashboard",
    };

    /// <inheritdoc/>
    public static StorageMetadataModel QuoteVolume => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.QUOTE_CONTROLLER_NAME, Routes.VOLUME_CONTROLLER_NAME),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };

    /// <inheritdoc/>
    public static StorageMetadataModel QuoteSizeVolume => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.QUOTE_CONTROLLER_NAME, Routes.SIZE_CONTROLLER_NAME, Routes.VOLUME_CONTROLLER_NAME),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };

    /// <inheritdoc/>
    public static StorageMetadataModel SkipSizeVolume => new()
    {
        ApplicationName = STOCKSHARP_QUOTE_FORM,
        PropertyName = Path.Combine(Routes.QUOTE_CONTROLLER_NAME, Routes.SKIP_ACTION_NAME, Routes.SIZE_CONTROLLER_NAME, Routes.VOLUME_CONTROLLER_NAME),
        PrefixPropertyName = Routes.DUMP_ACTION_NAME,
    };
}