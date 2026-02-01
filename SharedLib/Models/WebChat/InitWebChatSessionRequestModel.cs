////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InitWebChatSessionRequestModel
/// </summary>
public class InitWebChatSessionRequestModel
{
    /// <summary>
    /// SessionTicket
    /// </summary>
    public string? SessionTicket { get; set; }

    /// <summary>
    /// UserIdentityId
    /// </summary>
    public string? UserIdentityId { get; set; }
}