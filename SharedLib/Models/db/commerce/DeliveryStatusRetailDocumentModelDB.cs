////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DeliveryStatusDocument
/// </summary>
[Index(nameof(DeliveryStatus)),Index(nameof(Name))]
public class DeliveryStatusRetailDocumentModelDB : EntryUpdatedLiteModel
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