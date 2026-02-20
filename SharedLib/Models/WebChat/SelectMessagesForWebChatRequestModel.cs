////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectMessagesForWebChatRequestModel
/// </summary>
public class SelectMessagesForWebChatRequestModel
{
    /// <summary>
    /// The start index of the data segment requested.
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    /// The requested number of items to be provided. The actual number of provided items does not need to match this value.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// SessionTicket
    /// </summary>
    public string? SessionTicket { get; set; }

    /// <summary>
    /// SessionTicketId
    /// </summary>
    public required string SessionTicketId { get; set; }

    /// <summary>
    /// IncludeDeletedMessages
    /// </summary>
    public bool IncludeDeletedMessages { get; set; }
}