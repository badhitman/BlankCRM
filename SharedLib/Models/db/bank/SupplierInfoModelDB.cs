////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SupplierInfoModelDB
/// </summary>
public class SupplierInfoModelDB : SupplierInfoForReceiptItemTBankModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public ReceiptItemModelDB? ReceiptItem { get; set; }

    /// <inheritdoc/>
    public int ReceiptItemId { get; set; }
}