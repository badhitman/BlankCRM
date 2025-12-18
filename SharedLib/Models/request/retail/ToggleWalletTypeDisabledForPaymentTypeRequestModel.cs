////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ToggleWalletTypeDisabledForPaymentTypeRequestModel
/// </summary>
public class ToggleWalletTypeDisabledForPaymentTypeRequestModel
{
    /// <inheritdoc/>
    public PaymentsRetailTypesEnum PaymentType { get; set; }

    /// <inheritdoc/>
    public int WalletTypeId { get; set; }
}