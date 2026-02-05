////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DialogWebChatViewModel
/// </summary>
[Index(nameof(LastOnlineAtUTC)), Index(nameof(DeadlineUTC)), Index(nameof(LastMessageAtUTC))]
[Index(nameof(IsDisabled)), Index(nameof(SessionTicketId), IsUnique = true)]
public class DialogWebChatViewModel : DialogWebChatBaseModel
{
    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime? LastMessageAtUTC { get; set; }

    /// <summary>
    /// Последний раз, когда в диалог кто-то обращался
    /// </summary>
    public DateTime LastOnlineAtUTC { get; set; }

    /// <summary>
    /// Срок окончания действия
    /// </summary>
    public DateTime DeadlineUTC { get; set; }

    /// <summary>
    /// Объект отключён
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <inheritdoc/>
    public required string SessionTicketId { get; set; }

    /// <inheritdoc/>
    public required string? UserAgent { get; set; }

    /// <inheritdoc/>
    public required string? Language { get; set; }
}