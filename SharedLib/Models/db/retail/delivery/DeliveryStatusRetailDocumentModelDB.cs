////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// DeliveryStatusRetailDocumentModelDB
/// </summary>
[Index(nameof(DeliveryStatus)), Index(nameof(Name)), Index(nameof(DateOperation))]
public class DeliveryStatusRetailDocumentModelDB : EntryUpdatedLiteModel
{
    /// <inheritdoc/>
    [Required]
    public required DateTime DateOperation { get; set; }

    /// <summary>
    /// Статус доставки
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public DeliveryStatusesEnum DeliveryStatus { get; set; }

    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public DeliveryDocumentRetailModelDB? DeliveryDocument { get; set; }
    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public int DeliveryDocumentId { get; set; }
}