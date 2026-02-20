////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// HelpDesk Config
/// </summary>
public class HelpDeskConfigModel : WebConfigModel
{
    /// <inheritdoc/>
    public static new readonly string Configuration = "HelpDeskConfig";

    /// <summary>
    /// Длительность Cache для сегментов консоли (по статусу)
    /// </summary>
    public int ConsoleSegmentCacheLifetimeSeconds { get; set; } = 60 * 60 * 24 * 7;//неделя
}
