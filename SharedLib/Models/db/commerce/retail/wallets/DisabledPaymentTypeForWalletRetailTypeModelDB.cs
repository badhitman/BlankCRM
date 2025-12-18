////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// DisabledPaymentTypeForWalletRetailTypeModelDB
/// </summary>
public class DisabledPaymentTypeForWalletRetailTypeModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <inheritdoc/>
    public PaymentsRetailTypesEnum PaymentType { get; set; }
    
    /// <inheritdoc/>
    public WalletRetailTypeModelDB? WalletType { get; set; }
    /// <inheritdoc/>
    public int WalletTypeId { get; set; }
}
