////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BankTransfer
/// </summary>
public class BankTransfer
{
    /// <inheritdoc/>
    public required string TransactionId { get; set; }
    
    /// <inheritdoc/>
    public DateTime Timestamp { get; set; }

    /// <inheritdoc/>
    public decimal Amount { get; set; }

    /// <inheritdoc/>
    public required string Currency { get; set; }

    /// <summary>
    /// The sender's Name or identifier
    /// </summary>
    public required string Sender { get; set; }

    /// <summary>
    /// The receiver's Name or identifier
    /// </summary>
    public required string Receiver { get; set; }
}