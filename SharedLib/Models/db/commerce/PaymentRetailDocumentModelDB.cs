////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// PaymentRetailDocumentModelDB
/// </summary>
[Index(nameof(PayerIdentityUserId)), Index(nameof(PaymentSource)), Index(nameof(TypePayment)), Index(nameof(StatusPayment))]
public class PaymentRetailDocumentModelDB : EntryUpdatedModel
{
    /// <summary>
    /// Тип/способ оплаты
    /// </summary>
    public PaymentsRetailTypesEnum TypePayment { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public required PaymentsRetailStatusesEnum StatusPayment { get; set; }

    /// <summary>
    /// Плательщик
    /// </summary>
    public required string? PayerIdentityUserId { get; set; }

    /// <summary>
    /// Ссылка для оплаты
    /// </summary>
    public string? PaymentSource { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Кошелёк
    /// </summary>
    public WalletRetailModelDB? Wallet {  get; set; }
    /// <summary>
    /// Кошелёк
    /// </summary>
    public int WalletId { get; set; }

    /// <summary>
    /// PaymentOrderLink
    /// </summary>
    public List<PaymentRetailOrderLinkModelDB>? PaymentOrdersLinks { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}