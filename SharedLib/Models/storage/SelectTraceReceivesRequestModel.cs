////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectTraceReceivesRequestModel
/// </summary>
public class SelectTraceReceivesRequestModel : PeriodBaseModel
{
    /// <inheritdoc/>
    public string[]? ReceiversNames { get; set; }

    /// <summary>
    /// IdentityUserId
    /// </summary>
    public string[]? IdentityUsersIds { get; set; }
}