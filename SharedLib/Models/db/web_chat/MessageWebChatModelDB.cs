////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// MessageWebChatModelDB
/// </summary>
[Index(nameof(IsDisabled)), Index(nameof(CreatedAtUTC)), Index(nameof(SenderUserIdentityId))]
public class MessageWebChatModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Отправитель
    /// </summary>
    /// <remarks>
    /// Если не указано, значит от инициатора диалога
    /// </remarks>
    public string? SenderUserIdentityId { get; set; }

    /// <inheritdoc/>
    public DialogWebChatModelDB? DialogOwner { get; set; }
    /// <inheritdoc/>
    public int DialogOwnerId { get; set; }

    /// <inheritdoc/>
    public required string Text { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <summary>
    /// Отправитель - web клиент
    /// </summary>
    public bool InitiatorMessageSender { get; set; }

    /// <summary>
    /// Сообщение удалено?
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Вложение/файл (опционально)
    /// </summary>
    public List<AttachesMessageWebChatModelDB>? AttachesFiles { get; set; }
}