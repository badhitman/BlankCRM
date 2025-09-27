////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// QR СБП/НСПК
/// </summary>
[Index(nameof(ErrorCode)), Index(nameof(Success)), Index(nameof(TerminalKey)), Index(nameof(TypeQR))]
public class PaymentInitTBankQRModelDB
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <summary>
    /// Тип данных QR СБП/НСПК
    /// </summary>
    public DataTypeQREnum TypeQR { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DataQR { get; set; }

    /// <summary>
    /// Идентификатор терминала. Выдается продавцу банком при заведении терминала
    /// </summary>
    // [JsonRequired] //not documented in SendClosingReceipt
    public string TerminalKey { get; set; } = string.Empty;
    /// <summary>
    /// Признак успешного выполнения запроса
    /// </summary>
    public bool Success { get; set; } = false;
    /// <summary>
    /// Код ошибки. Если ошибки не произошло, передается значение «0»
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>
    /// Краткое описание ошибки
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// Подробное описание ошибки	
    /// </summary>
    public string? Details { get; set; }
}
