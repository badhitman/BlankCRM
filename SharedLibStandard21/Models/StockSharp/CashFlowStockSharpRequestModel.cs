////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CashFlowStockSharpRequestModel
/// </summary>
public class CashFlowStockSharpRequestModel
{
    /// <inheritdoc/>
    public int FromDays { get; set; }
    
    /// <inheritdoc/>
    public decimal NotionalFirst { get; set; }
}