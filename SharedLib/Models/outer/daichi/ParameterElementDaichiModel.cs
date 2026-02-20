////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ParameterEntryDaichiModel
/// </summary>
public class ParameterElementDaichiModel : DaichiEntryModel
{
    /// <inheritdoc/>
    public required string ID { get; set; }

    /// <inheritdoc/>
    public string? MAIN_SECTION { get; set; }

    /// <inheritdoc/>
    public string? BRAND { get; set; }
}