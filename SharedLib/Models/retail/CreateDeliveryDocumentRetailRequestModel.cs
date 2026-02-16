////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// CreateDeliveryDocumentRetailRequestModel
/// </summary>
public class CreateDeliveryDocumentRetailRequestModel : DeliveryDocumentRetailModelDB
{
    /// <inheritdoc/>
    public int InjectToOrderId { get; set; }

    /// <inheritdoc/>
    public static CreateDeliveryDocumentRetailRequestModel Build(DeliveryDocumentRetailModelDB other, int injectToOrderId)
    {
        CreateDeliveryDocumentRetailRequestModel res = new()
        {
            AuthorIdentityUserId = other.AuthorIdentityUserId,
            RecipientIdentityUserId = other.RecipientIdentityUserId,
            AddressUserComment = other.AddressUserComment,
            CreatedAtUTC = other.CreatedAtUTC,
            DeliveryCode = other.DeliveryCode,
            DeliveryPaymentUponReceipt = other.DeliveryPaymentUponReceipt,
            DeliveryStatus = other.DeliveryStatus,
            DeliveryStatusesLog = other.DeliveryStatusesLog,
            DeliveryTypeId = other.DeliveryTypeId,
            Description = other.Description,
            Id = other.Id,
            InjectToOrderId = injectToOrderId,
            KladrCode = other.KladrCode,
            KladrTitle = other.KladrTitle,
            LastUpdatedAtUTC = other.LastUpdatedAtUTC,
            Name = other.Name,
            OrdersLinks = other.OrdersLinks,
            Rows = other.Rows,
            ShippingCost = other.ShippingCost,
            WarehouseId = other.WarehouseId,
            WeightShipping = other.WeightShipping,
        };

        if (res.Id <= 0)
            res.Rows?.ForEach(r =>
            {
                r.Document = res;
            });
        else
            res.Rows?.ForEach(r =>
            {
                r.Document = null;
            });

        return res;
    }
}