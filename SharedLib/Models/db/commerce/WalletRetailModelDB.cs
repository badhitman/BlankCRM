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
}