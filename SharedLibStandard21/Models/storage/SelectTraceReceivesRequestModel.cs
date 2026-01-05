////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectTraceReceivesRequestModel
/// </summary>
public class SelectTraceReceivesRequestModel : SelectRequestAuthBaseModel
{
    /// <inheritdoc/>
    public string[]? ReceiversNames { get; set; }
}