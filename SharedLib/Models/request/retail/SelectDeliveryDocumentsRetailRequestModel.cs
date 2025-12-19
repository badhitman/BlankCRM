////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////



namespace SharedLib;

/// <summary>
/// SelectDeliveryDocumentsRetailRequestModel
/// </summary>
public class SelectDeliveryDocumentsRetailRequestModel
{
    /// <summary>
    /// UserIdentityId
    /// </summary>
    public string[]? RecipientsFilterIdentityId { get; set; }

    /// <inheritdoc/>
    public int? FilterOrderId { get; set; }

    /// <inheritdoc/>
    public DeliveryTypesEnum[]? TypesFilter { get; set; }

    /// <inheritdoc/>
    public List<DeliveryStatusesEnum?>? StatusesFilter { get; set; }

    /// <summary>
    /// Исключить по документу доставки
    /// </summary>
    public int? ExcludeOrderId { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }

    /// <inheritdoc/>
    public DateTime? End { get; set; }
}