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