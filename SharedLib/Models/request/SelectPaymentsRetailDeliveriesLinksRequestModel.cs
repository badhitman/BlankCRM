////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectPaymentsRetailDeliveriesLinksRequestModel
/// </summary>
public class SelectPaymentsRetailDeliveriesLinksRequestModel
{
    /// <inheritdoc/>
    public int? DeliveryDocumentId { get; set; }

    /// <inheritdoc/>
    public int? PaymentId { get; set; }
}