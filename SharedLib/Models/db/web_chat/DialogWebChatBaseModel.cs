////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DialogWebChatBaseModel
/// </summary>
[Index(nameof(InitiatorContacts)), Index(nameof(InitiatorIdentityId))]
public class DialogWebChatBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

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
}