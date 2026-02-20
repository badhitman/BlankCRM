////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectHistoryReceivesRequestModel
/// </summary>
public class SelectHistoryReceivesRequestModel : PeriodBaseModel
{
    /// <inheritdoc/>
    public string[]? ReceiversNames { get; set; }

    /// <summary>
    /// IdentityUserId
    /// </summary>
    public string[]? IdentityUsersIds { get; set; }
}