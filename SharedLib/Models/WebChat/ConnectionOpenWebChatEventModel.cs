////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ConnectOpenWebChatEventModel
/// </summary>
public class ConnectionOpenWebChatEventModel
{
    /// <inheritdoc/>
    public KeyValuePair<string, UserInfoModel?>? UserInfoBaseModel { get; set; }

    /// <inheritdoc/>
    public required int DialogId { get; set; }
}
