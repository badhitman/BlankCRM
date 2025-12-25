////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreateDocumentRetailRequestModel
/// </summary>
public class CreateDocumentRetailRequestModel : DocumentRetailModelDB
{
    /// <inheritdoc/>
    public int InjectToDeliveryId { get; set; }

    /// <inheritdoc/>
    public int InjectToConversionId { get; set; }

    /// <inheritdoc/>
    public int InjectToPaymentId { get; set; }

    /// <inheritdoc/>
    public static CreateDocumentRetailRequestModel Build(DocumentRetailModelDB editDocument, int injectToDeliveryId, int injectToConversionId, int injectToPaymentId)
    {
        return new()
        {
            AuthorIdentityUserId = editDocument.AuthorIdentityUserId,
            BuyerIdentityUserId = editDocument.BuyerIdentityUserId,
            DateDocument = editDocument.DateDocument,
            Conversions = editDocument.Conversions,
            CreatedAtUTC = editDocument.CreatedAtUTC,
            Deliveries = editDocument.Deliveries,
            Description = editDocument.Description,
            ExternalDocumentId = editDocument.ExternalDocumentId,
            HelpDeskId = editDocument.HelpDeskId,
            Id = editDocument.Id,
            InjectToConversionId = injectToConversionId,
            InjectToDeliveryId = injectToDeliveryId,
            InjectToPaymentId = injectToPaymentId,
            LastUpdatedAtUTC = editDocument.LastUpdatedAtUTC,
            Name = editDocument.Name,
            NumWeekOfYear = editDocument.NumWeekOfYear,
            Rows = editDocument.Rows,
            StatusDocument = editDocument.StatusDocument,
            Version = editDocument.Version,
            WarehouseId = editDocument.WarehouseId,
        };
    }
}