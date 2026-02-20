////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ReceiptTBankModelDB
/// </summary>
public class ReceiptTBankModelDB : ReceiptTBankModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public new List<ReceiptItemModelDB>? Items { get; set; }

    /// <inheritdoc/>
    public new PaymentsForReceiptTBankModelDB? Payments { get; set; }
}