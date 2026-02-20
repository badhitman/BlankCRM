////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Кошельки
/// </summary>
[Index(nameof(UserIdentityId))]
public class WalletRetailModelDB : EntryUpdatedModel
{
    /// <summary>
    /// UserIdentityId
    /// </summary>
    public required string UserIdentityId { get; set; }

    /// <summary>
    /// Баланс
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Тип кошелька
    /// </summary>
    public WalletRetailTypeModelDB? WalletType { get; set; }
    /// <summary>
    /// Тип кошелька
    /// </summary>
    public int WalletTypeId { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }


    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{WalletType?.Name}] {Name}".Trim();
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is WalletRetailModelDB other)
            return other.Id == Id && other.Name == Name && other.WalletTypeId == WalletTypeId && other.UserIdentityId == UserIdentityId;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, WalletTypeId, UserIdentityId);
    }
}