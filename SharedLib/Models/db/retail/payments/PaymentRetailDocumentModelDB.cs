////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PaymentRetailDocumentModelDB
/// </summary>
[Index(nameof(PaymentSource)), Index(nameof(TypePayment)), Index(nameof(TypePaymentId)), Index(nameof(StatusPayment))]
[Index(nameof(Name)), Index(nameof(DatePayment)), Index(nameof(AuthorUserIdentity))]
public class PaymentRetailDocumentModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    [Required]
    public required DateTime DatePayment { get; set; }

    /// <summary>
    /// Тип/способ оплаты
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public PaymentsRetailTypesEnum TypePayment { get; set; }

    /// <summary>
    /// Тип/способ оплаты
    /// </summary>
    public int TypePaymentId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
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
    [ConcurrencyCheck]
    public Guid Version { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{DatePayment} {TypePayment.DescriptionInfo()}] `{StatusPayment.DescriptionInfo()}` - {Amount}";
    }

    /// <inheritdoc/>
    public static PaymentRetailDocumentModelDB Build(CreatePaymentRetailDocumentRequestModel other)
    {
        return new()
        {
            AuthorUserIdentity = other.AuthorUserIdentity,
            DatePayment = other.DatePayment,
            TypePayment = other.TypePayment,
            Version = other.Version,
            PaymentSource = other.PaymentSource,
            StatusPayment = other.StatusPayment,
            Amount = other.Amount,
            CreatedAtUTC = other.CreatedAtUTC,
            Description = other.Description,
            Id = other.Id,
            LastUpdatedAtUTC = other.LastUpdatedAtUTC,
            Name = other.Name,
            Wallet = other.Wallet,
            WalletId = other.WalletId,
        };
    }
}