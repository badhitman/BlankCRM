////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тип кошелька
/// </summary>
public class WalletRetailTypeModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Игнорировать изменение баланса
    /// </summary>
    public bool IgnoreBalanceChanges { get; set; }

    /// <inheritdoc/>
    public int SortIndex { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is WalletRetailTypeViewModel walletOther2)
            return
                walletOther2.IsSystem == IsSystem &&
                walletOther2.IsDisabled == IsDisabled &&
                walletOther2.IgnoreBalanceChanges == IgnoreBalanceChanges &&
                walletOther2.Name == Name &&
                walletOther2.Description == Description;

        if (obj is WalletRetailTypeModelDB walletOther)
            return
                walletOther.IsSystem == IsSystem &&
                walletOther.IsDisabled == IsDisabled &&
                walletOther.IgnoreBalanceChanges == IgnoreBalanceChanges &&
                walletOther.Name == Name && walletOther.Description == Description;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(IsDisabled, Id, Name, Description);
    }

    /// <summary>
    /// Кошельки
    /// </summary>
    public List<WalletRetailModelDB>? Wallets { get; set; }

    /// <summary>
    /// Системный
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// DisabledPaymentsTypes
    /// </summary>
    public List<DisabledPaymentTypeForWalletRetailTypeModelDB>? DisabledPaymentsTypes { get; set; }
}