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
    public DeliveryTypesEnum[]? TypesFilter {  get; set; }

    /// <summary>
    /// Исключить по документу доставки
    /// </summary>
    public int? ExcludeDeliveryId { get; set; }
}