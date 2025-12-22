////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// DisabledPaymentTypeForWalletRetailTypeModelDB
/// </summary>
[Index(nameof(PaymentType), nameof(WalletTypeId), IsUnique = true)]
public class DisabledPaymentTypeForWalletRetailTypeModelDB : ToggleWalletTypeDisabledForPaymentTypeRequestModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public WalletRetailTypeModelDB? WalletType { get; set; }
}