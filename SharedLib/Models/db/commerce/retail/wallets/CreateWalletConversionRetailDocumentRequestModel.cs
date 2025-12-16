////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreateWalletConversionRetailDocumentRequestModel
/// </summary>
public class CreateWalletConversionRetailDocumentRequestModel : WalletConversionRetailDocumentModelDB
{
    /// <inheritdoc/>
    public int InjectToOrderId { get; set; }

    /// <inheritdoc/>
    public static CreateWalletConversionRetailDocumentRequestModel Build(WalletConversionRetailDocumentModelDB other, int injectToOrderId)
    {
        return new()
        {
            DateDocument = other.DateDocument,
            InjectToOrderId = injectToOrderId,
            CreatedAtUTC = other.CreatedAtUTC,
            Description = other.Description,
            FromWallet = other.FromWallet,
            FromWalletId = other.FromWalletId,
            FromWalletSum = other.FromWalletSum,
            ToWallet = other.ToWallet,
            ToWalletId = other.ToWalletId,
            Id = other.Id,
            IsDisabled = other.IsDisabled,
            LastUpdatedAtUTC = other.LastUpdatedAtUTC,
            Name = other.Name,
            Orders = other.Orders,
            ToWalletSum = other.ToWalletSum,
            Version = other.Version,
        };
    }
}