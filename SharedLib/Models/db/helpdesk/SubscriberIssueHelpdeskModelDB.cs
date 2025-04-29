////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Подписчик на собыия в обращении (ModelDB)
/// </summary>
[Index(nameof(UserId), nameof(IssueId), IsUnique = true)]
public class SubscriberIssueHelpDeskModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Обращение клиента с вопросом
    /// </summary>
    public int IssueId { get; set; }
    /// <summary>
    /// Обращение клиента с вопросом
    /// </summary>
    public IssueHelpDeskModelDB? Issue { get; set; }

    /// <summary>
    /// Пользователь, который подписан (of Identity)
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// отключение отправки уведомлений
    /// </summary>
    public bool IsSilent { get; set; }
}