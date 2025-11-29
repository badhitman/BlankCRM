////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// WalletBalanceCommitRequestModel
/// </summary>
public class WalletBalanceCommitRequestModel
{
    /// <inheritdoc/>
    public int WalletId { get; set; }

    /// <inheritdoc/>
    public decimal ValueCommit { get; set; }
}