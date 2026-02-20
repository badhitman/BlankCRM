////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ConnectionCloseWebChatEventModel
/// </summary>
public class ConnectionCloseWebChatEventModel
{
    /// <inheritdoc/>
    public UserInfoModel? UserInfoBaseModel { get; set; }

    /// <inheritdoc/>
    public required int DialogId { get; set; }
}
