////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OrderStatusRetailDocumentModelDB
/// </summary>
[Index(nameof(StatusDocument)), Index(nameof(Name)), Index(nameof(DateOperation))]
public class OrderStatusRetailDocumentModelDB : EntryUpdatedLiteModel
{
    /// <inheritdoc/>
    [Required]
    public required DateTime DateOperation { get; set; }

    /// <summary>
    /// Шаг/статус обращения: "Создан", "В работе", "На проверке" и "Готово"
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public StatusesDocumentsEnum StatusDocument { get; set; }

    /// <summary>
    /// OrderDocument
    /// </summary>
    public DocumentRetailModelDB? OrderDocument { get; set; }
    /// <summary>
    /// OrderDocument
    /// </summary>
    public int OrderDocumentId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"#{Id} '{DateOperation:R}'";
    }
}