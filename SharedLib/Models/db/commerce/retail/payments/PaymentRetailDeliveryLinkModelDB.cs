////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PaymentOrderLinkModelDB
/// </summary>
public class PaymentRetailDeliveryLinkModelDB : PaymentRetailLinkBaseModel
{
    /// <summary>
    /// Доставка (документ)
    /// </summary>
    public DeliveryDocumentRetailModelDB? DeliveryDocument { get; set; }

    /// <summary>
    /// Доставка (документ)
    /// </summary>
    public int DeliveryDocumentId { get; set; }
}