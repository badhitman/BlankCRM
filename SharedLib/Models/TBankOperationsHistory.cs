////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TBankOperationsHistory
/// </summary>
public class TBankOperationsHistory
{
    /// <inheritdoc/>
    public List<BankTransferModelDB>? Data { get; set; }
    
    /// <inheritdoc/>
    public bool Success { get; set; }
   
    /// <inheritdoc/>
    public PaginationMeta? Meta { get; set; }
}
