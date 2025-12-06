////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// PaymentRetailDocumentModelDB
/// </summary>
[Index(nameof(PaymentSource)), Index(nameof(TypePayment)), Index(nameof(StatusPayment))]
[Index(nameof(Name)), Index(nameof(DatePayment)), Index(nameof(AuthorUserIdentity))]
public class PaymentRetailDocumentModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    [Required]
    public required DateTime DatePayment { get; set; }

    /// <summary>
    /// Тип/способ оплаты
    /// </summary>
    public PaymentsRetailTypesEnum TypePayment { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public required PaymentsRetailStatusesEnum StatusPayment { get; set; }

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
    public WalletRetailModelDB? Wallet { get; set; }
    /// <summary>
    /// Кошелёк
    /// </summary>
    public int WalletId { get; set; }

    /// <inheritdoc/>
    public required string AuthorUserIdentity { get; set; }

    /// <inheritdoc/>
    public List<PaymentRetailOrderLinkModelDB>? OrdersLinks { get; set; }

    /// <inheritdoc/>
    public List<PaymentRetailDeliveryLinkModelDB>? DeliveriesLinks { get; set; }

    /// <inheritdoc/>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}