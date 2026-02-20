////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// WalletRetailReportRowModel
/// </summary>
public class WalletRetailReportRowModel
{
    /// <summary>
    /// Кошелёк
    /// </summary>
    public required WalletRetailModelDB Wallet { get; set; }

    /// <summary>
    /// User
    /// </summary>
    public required UserInfoModel User {  get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public required decimal Amount { get; set; }
}