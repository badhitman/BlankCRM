////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DeliveryStatusDocument
/// </summary>
[Index(nameof(DeliveryStatus))]
public class DeliveryStatusRetailDocumentModelDB : EntryUpdatedModel
{
    /// <summary>
    /// Статус доставки
    /// </summary>
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