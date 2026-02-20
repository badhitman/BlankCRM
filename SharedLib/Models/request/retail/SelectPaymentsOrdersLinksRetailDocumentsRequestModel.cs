////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectPaymentsOrdersLinksRetailDocumentsRequestModel
/// </summary>
public class SelectPaymentsOrdersLinksRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int[]? OrdersIds { get; set; }

    /// <inheritdoc/>
    public int[]? PaymentsIds { get; set; }
}