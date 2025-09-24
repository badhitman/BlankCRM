////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AgentDataModelDB
/// </summary>
public class AgentDataModelDB : AgentDataForReceiptItemTBankModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public ReceiptItemModelDB? ReceiptItem { get; set; }

    /// <inheritdoc/>
    public int ReceiptItemId { get; set; }
}