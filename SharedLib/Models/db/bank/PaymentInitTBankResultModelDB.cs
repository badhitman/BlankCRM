////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// PaymentInitTBankResultModelDB
/// </summary>
[Index(nameof(TerminalKey)), Index(nameof(Success)), Index(nameof(Status))]
[Index(nameof(AuthorUserId)), Index(nameof(CreatedDateTimeUTC)), Index(nameof(PaymentId)), Index(nameof(OrderId)), Index(nameof(Amount)), Index(nameof(ErrorCode))]
public class PaymentInitTBankResultModelDB : PaymentInitTBankResultModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public ReceiptTBankModelDB? Receipt { get; set; }

    /// <inheritdoc/>
    public int ReceiptId { get; set; }

    /// <inheritdoc/>
    public PaymentInitTBankQRModelDB? PaymentQR { get; set; }
    /// <inheritdoc/>
    public int? PaymentQRId { get; set; }


    /// <summary>
    /// Идентификатор терминала. Выдается мерчанту в Т‑Бизнес при заведении терминала.
    /// </summary>
    public string? TerminalKey { get; set; }

    /// <summary>
    /// Успешность прохождения запроса — true/false.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Код ошибки.
    /// </summary>
    /// <remarks>
    /// max-characters: 20
    /// </remarks>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Краткое описание ошибки.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Подробное описание ошибки.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Creator (initiator)
    /// </summary>
    public required string AuthorUserId { get; set; }

    /// <inheritdoc/>
    public string? ApiException { get; set; }

    /// <inheritdoc/>
    public required DateTime CreatedDateTimeUTC { get; set; }
}