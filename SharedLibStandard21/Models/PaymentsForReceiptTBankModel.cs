////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Данные об оплате чека
/// </summary>
public class PaymentsForReceiptTBankModel
{
    /// <summary>
    /// Вид оплаты "Наличные". Сумма к оплате в копейках не более 14 знаков.
    /// </summary>
    public ulong? Cash { get; set; }

    /// <summary>
    /// Вид оплаты "Безналичный". Сумма к оплате в копейках не более 14 знаков.
    /// </summary>
    public ulong Electronic { get; private set; }

    /// <summary>
    /// Вид оплаты "Предварительная оплата (Аванс)". Сумма к оплате в копейках не более 14 знаков.
    /// </summary>
    public ulong? AdvancePayment { get; set; }

    /// <summary>
    /// Вид оплаты "Пост-оплата (Кредит)". Сумма к оплате в копейках не более 14 знаков.
    /// </summary>
    public ulong? Credit { get; set; }

    /// <summary>
    /// Вид оплаты "Иная форма оплаты". Сумма к оплате в копейках не более 14 знаков.
    /// </summary>
    public ulong? Provision { get; set; }
}