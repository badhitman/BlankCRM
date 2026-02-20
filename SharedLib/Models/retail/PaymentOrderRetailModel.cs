////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PaymentOrderRetailModel
/// </summary>
public class PaymentOrderRetailModel(decimal amountPayment, int typePayment)
{
    /// <inheritdoc/>
    public decimal AmountPayment { get; } = amountPayment;

    /// <inheritdoc/>
    public int TypePayment { get; } = typePayment;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is PaymentOrderRetailModel other &&
               AmountPayment == other.AmountPayment &&
               TypePayment == other.TypePayment;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(AmountPayment, TypePayment);
    }
}