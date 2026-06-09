////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// LogsClearRequestModel
/// </summary>
public class LogsClearRequestModel : LogsSelectRequestModel
{
    /// <inheritdoc/>
    public string? ConfirmQuery { get; set; }
}