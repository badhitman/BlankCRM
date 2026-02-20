////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectMessagesForWebChatRoomRequestModel
/// </summary>
public class SelectMessagesForWebChatRoomRequestModel
{
    /// <inheritdoc/>
    public int DialogId { get; set; }

    /// <inheritdoc/>
    public bool? IncludeDeletedMessages { get; set; }
}