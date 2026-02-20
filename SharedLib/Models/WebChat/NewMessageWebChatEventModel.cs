////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// NewMessageWebChatEventModel
/// </summary>
public class NewMessageWebChatEventModel
{
    /// <inheritdoc/>
    public int DialogId { get; set; }

    /// <inheritdoc/>
    public required string TextMessage { get; set; }
}