////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// PaymentOrderNoSiteRetailModel
/// </summary>
public class PaymentOrderNoSiteRetailModel(decimal amountPayment, PaymentsRetailTypesEnum typePayment)
{
    /// <inheritdoc/>
    public decimal AmountPayment { get; } = amountPayment;

    /// <inheritdoc/>
    public PaymentsRetailTypesEnum TypePayment { get; } = typePayment;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is PaymentOrderNoSiteRetailModel other &&
               AmountPayment == other.AmountPayment &&
               TypePayment == other.TypePayment;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(AmountPayment, TypePayment);
    }
}