////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DialogWebChatModelDB
/// </summary>
[Index(nameof(InitiatorContactsNormalized)), Index(nameof(HelpdeskId))]
public class DialogWebChatModelDB : DialogWebChatViewModel
{
    /// <summary>
    /// InitiatorContactsNormalized
    /// </summary>
    public string? InitiatorContactsNormalized { get; set; }

    /// <inheritdoc/>
    public List<UserJoinDialogWebChatModelDB>? UsersJoins { get; set; }

    /// <inheritdoc/>
    public required string BaseUri { get; set; }

    /// <inheritdoc/>
    public int? HelpdeskId { get; set; }
}