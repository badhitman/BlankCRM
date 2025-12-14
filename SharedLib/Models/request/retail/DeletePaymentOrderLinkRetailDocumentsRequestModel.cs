////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeletePaymentOrderLinkRetailDocumentsRequestModel
/// </summary>
public class DeletePaymentOrderLinkRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int OrderPaymentLinkId { get; set; }

    /// <inheritdoc/>
    public int OrderId { get; set; }

    /// <inheritdoc/>
    public int PaymentId { get; set; }
}