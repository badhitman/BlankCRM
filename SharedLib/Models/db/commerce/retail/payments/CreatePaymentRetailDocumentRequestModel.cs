////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreatePaymentRetailDocumentRequestModel
/// </summary>
public class CreatePaymentRetailDocumentRequestModel : PaymentRetailDocumentModelDB
{
    /// <inheritdoc/>
    public int InjectToOrderId { get; set; }

    /// <inheritdoc/>
    public static CreatePaymentRetailDocumentRequestModel Build(PaymentRetailDocumentModelDB other, int injectToOrderId)
    {
        return new()
        {
            AuthorUserIdentity = other.AuthorUserIdentity,
            InjectToOrderId = injectToOrderId,
            DatePayment = other.DatePayment,
            TypePayment = other.TypePayment,
            Version = other.Version,
            PaymentSource = other.PaymentSource,
            StatusPayment = other.StatusPayment,
            Amount = other.Amount,
            CreatedAtUTC = other.CreatedAtUTC,
            Description = other.Description,
            Id = other.Id,
            LastUpdatedAtUTC = other.LastUpdatedAtUTC,
            Name = other.Name,
            Wallet = other.Wallet,
            WalletId = other.WalletId,
        };
    }
}