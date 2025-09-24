////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ReceiptItemModelDB
/// </summary>
public class  ReceiptItemModelDB : ReceiptItemTBankModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public new SupplierInfoModelDB? SupplierInfo { get; set; }

    /// <inheritdoc/>
    public new AgentDataModelDB? AgentData { get; set; }


    /// <inheritdoc/>
    public ReceiptTBankModelDB? Receipt { get; set; }

    /// <inheritdoc/>
    public int ReceiptId { get; set; }
}