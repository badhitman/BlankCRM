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
    public UserInfoModel? UserInfo { get; set; }

    /// <inheritdoc/>
    public required int DialogId { get; set; }
}
