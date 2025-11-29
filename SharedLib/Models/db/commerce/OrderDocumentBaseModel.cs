////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OrderDocumentBaseModel
/// </summary>
[Index(nameof(ExternalDocumentId), nameof(HelpDeskId), nameof(AuthorIdentityUserId), nameof(StatusDocument))]
public class OrderDocumentBaseModel : EntryUpdatedModel
{
    /// <summary>
    /// Шаг/статус обращения: "Создан", "В работе", "На проверке" и "Готово"
    /// </summary>
    public StatusesDocumentsEnum StatusDocument { get; set; }

    /// <summary>
    /// IdentityUserId
    /// </summary>
    public required string AuthorIdentityUserId { get; set; }

    /// <summary>
    /// Идентификатор документа из внешней системы (например 1С)
    /// </summary>
    public string? ExternalDocumentId { get; set; }

    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public string? Information { get; set; }

    /// <summary>
    /// Заявка, связанная с заказом.
    /// </summary>
    /// <remarks>
    /// До тех пор пока не указана заявка этот заказ всего лишь [Корзина]
    /// </remarks>
    public int? HelpDeskId { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}