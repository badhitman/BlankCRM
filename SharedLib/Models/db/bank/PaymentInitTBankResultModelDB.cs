////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// PaymentInitTBankResultModelDB
/// </summary>
[Index(nameof(TerminalKey)), Index(nameof(Success)), Index(nameof(StatusName)), Index(nameof(Status))]
[Index(nameof(PaymentId)), Index(nameof(OrderId)), Index(nameof(Amount)), Index(nameof(ErrorCode))]
public class PaymentInitTBankResultModelDB : PaymentInitTBankResultModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public ReceiptTBankModelDB? Receipt { get; set; }

    /// <inheritdoc/>
    public int ReceiptId { get; set; }


    /// <summary>
    /// Идентификатор терминала. Выдается мерчанту в Т‑Бизнес при заведении терминала.
    /// </summary>
    public string? TerminalKey { get; set; }

    /// <summary>
    /// Успешность прохождения запроса — true/false.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Статус транзакции.
    /// </summary>
    /// <remarks>
    /// max-characters: 20
    /// </remarks>
    public string? StatusName { get; set; }

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


    /// <inheritdoc/>
    public string? ApiException { get; set; }
}