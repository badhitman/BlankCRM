////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DialogWebChatModelDB
/// </summary>
[Index(nameof(IsDisabled)), Index(nameof(DeadlineUTC)), Index(nameof(LastMessageAtUTC))]
[Index(nameof(LastReadAtUTC)), Index(nameof(InitiatorContacts)), Index(nameof(InitiatorIdentityId))]
public class DialogWebChatModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime LastMessageAtUTC { get; set; }

    /// <summary>
    /// Последний раз, когда в диалог кто-то обращался
    /// </summary>
    public DateTime LastReadAtUTC { get; set; }

    /// <summary>
    /// Срок окончания действия
    /// </summary>
    public DateTime DeadlineUTC { get; set; }

    /// <summary>
    /// Объект отключён
    /// </summary>
    public bool IsDisabled { get; set; }

    #region Initiator/Author
    /// <summary>
    /// Контакты инициатора/автора диалога
    /// </summary>
    public string? InitiatorContacts { get; set; }

    /// <summary>
    /// Имя для обращения
    /// </summary>
    public string? InitiatorHumanName { get; set; }

    /// <summary>
    /// Опция связи с реальным пользователем системы
    /// </summary>
    /// <remarks>
    /// Например, для связи с телегой
    /// </remarks>
    public string? InitiatorIdentityId { get; set; }
    #endregion
}