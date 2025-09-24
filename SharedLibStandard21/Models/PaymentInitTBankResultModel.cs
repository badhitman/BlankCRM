////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Данные о платеже
/// </summary>
public class PaymentInitTBankResultModel
{
    /// <summary>
    /// Сумма в копейках	
    /// </summary>
    public uint Amount { get; set; }

    /// <summary>
    /// Идентификатор заказа в системе продавца
    /// </summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор платежа в системе банка
    /// </summary>
    public string PaymentId { get; set; } = string.Empty;

    /// <summary>
    /// Статус платежа
    /// </summary>
    public EStatusResponsesEnum? Status { get; set; }

    /// <summary>
    /// Ссылка на платежную форму
    /// </summary>
    public string? PaymentURL { get; set; }
}