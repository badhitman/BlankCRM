////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BankTransferStorage
/// </summary>
public class BankTransferModelDB : BankTransfer
{
    /// <inheritdoc/>
    public int Id { get; set; }


    /// <inheritdoc/>
    public  int? CustomerBankId { get; set; }

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