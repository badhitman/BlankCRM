////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PropertiesRusklimatResponseModel
/// </summary>
public class PropertiesRusklimatResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public int TotalCount { get; set; }

    /// <inheritdoc/>
    public PropertyRusklimatModelDB[]? Data { get; set; }
}