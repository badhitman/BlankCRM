////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// TBankAccount
/// </summary>
[Index(nameof(BankConnectionId)), Index(nameof(IsActive))]
[Index(nameof(TariffCode)), Index(nameof(Status)), Index(nameof(Name)), Index(nameof(MainFlag)), Index(nameof(Currency))]
[Index(nameof(CreatedOn)), Index(nameof(ActivationDate))]
[Index(nameof(BankBik)), Index(nameof(AccountType)), Index(nameof(AccountNumber))]
public class TBankAccountModelDB : TBankAccountModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public int BankConnectionId { get; set; }

    /// <inheritdoc/>
    public BankConnectionModelDB? BankConnection { get; set; }

    /// <inheritdoc/>
    public bool IsActive { get; set; }

    /// <inheritdoc/>
    public static TBankAccountModelDB Build(TBankAccountModel x, int bankConnection)
    {
        return new()
        {
            AccountNumber = x.AccountNumber,
            AccountType = x.AccountType,
            Balance = x.Balance,
            BankBik = x.BankBik,
            Currency = x.Currency,
            MainFlag = x.MainFlag,
            Name = x.Name,
            Status = x.Status,
            TariffCode = x.TariffCode,
            TariffName = x.TariffName,
            ActivationDate = x.ActivationDate,
            BankConnectionId = bankConnection,
            CreatedOn = x.CreatedOn,
        };
    }
}