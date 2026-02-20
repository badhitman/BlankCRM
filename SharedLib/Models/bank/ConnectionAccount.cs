////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ConnectionAccount
/// </summary>
public class ConnectionAccount : TBankAccountModel
{
    /// <inheritdoc/>
    public List<BankTransferModelDB>? Operations { get; set; }

    /// <inheritdoc/>
    public static ConnectionAccount Bind(TBankAccountModelDB bankAccount, BankConnectionCheckResponseModel res)
    {
        return new()
        {
            AccountNumber = bankAccount.AccountNumber,
            AccountType = bankAccount.AccountType,
            Balance = bankAccount.Balance,
            ActivationDate = bankAccount.ActivationDate,
            CreatedOn = bankAccount.CreatedOn,
            Currency = bankAccount.Currency,
            MainFlag = bankAccount.MainFlag,
            Name = bankAccount.Name,
            Status = bankAccount.Status,
            TariffCode = bankAccount.TariffCode,
            TariffName = bankAccount.TariffName,
            BankBik = bankAccount.BankBik,
            Operations = res.Operations,
        };
    }
}