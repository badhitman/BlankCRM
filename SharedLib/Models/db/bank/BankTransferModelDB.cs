////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// BankTransferModelDB
/// </summary>
[Index(nameof(TransactionId)), Index(nameof(Timestamp)), Index(nameof(Amount)), Index(nameof(Currency)), Index(nameof(Sender)), Index(nameof(Receiver))]
public class BankTransferModelDB : BankTransfer
{
    /// <inheritdoc/>
    public int Id { get; set; }


    /// <inheritdoc/>
    public int? CustomerBankId { get; set; }

    /// <inheritdoc/>
    public CustomerBankIdModelDB? CustomerBank { get; set; }


    /// <inheritdoc/>
    public int BankConnectionId { get; set; }

    /// <inheritdoc/>
    public BankConnectionModelDB? BankConnection { get; set; }


    /// <inheritdoc/>
    public static BankTransferModelDB Build(BankTransfer x, int connectionId, int customerBankId)
    {
        return new()
        {
            Amount = x.Amount,
            Currency = x.Currency,
            TransactionId = x.TransactionId,
            Receiver = x.Receiver,
            Sender = x.Sender,
            Timestamp = x.Timestamp,

            BankConnectionId = connectionId,
            CustomerBankId = customerBankId
        };
    }

    /// <inheritdoc/>
    public override string ToString()
        => $"{Timestamp} - {Amount} {TransactionId}";
}