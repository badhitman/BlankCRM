////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InitMerchantTBankRequestModel
/// </summary>
public class InitMerchantTBankRequestModel
{
    /// <inheritdoc/>
    public required uint Amount { get; set; }

    /// <inheritdoc/>
    public required string OrderId { get; set; }

    /// <inheritdoc/>
    public string? Description { get; set; }

    /// <inheritdoc/>
    public required ReceiptTBankModel Receipt { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, object>? Data { get; set; }
}