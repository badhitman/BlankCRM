////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectMessagesForWebChatResponseModel
/// </summary>
public class SelectMessagesForWebChatResponseModel : SelectMessagesForWebChatRequestModel
{
    /// <summary>
    /// Messages
    /// </summary>
    public required List<MessageWebChatModelDB> Messages { get; set; }

    /// <summary>
    /// TotalRowsCount
    /// </summary>
    public int TotalRowsCount { get; set; }
}