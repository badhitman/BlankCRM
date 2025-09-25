////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Данные по чеку
/// </summary>
public class ReceiptTBankModel
{
    /// <summary>
    /// Электронная почта покупателя. Обязательна если не задан <see cref="Phone"/>
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Телефон покупателя. Обязателен если не задан <see cref="Email"/>
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// Система налогообложения.
    /// </summary>
    [JsonProperty(PropertyName = "Taxation", Required = Required.Always)]
    public TaxationsTBankEnum Taxation { get; set; }

    /// <summary>
    /// Электронная почта продавца
    /// </summary>
    public string? EmailCompany { get; set; }

    /// <summary>
    /// Позиции чека с информацией о товарах.
    /// </summary>
    public virtual List<ReceiptItemTBankModel> Items { get; set; } = [];

    /// <summary>
    /// Объект c информацией о видами суммы платежа.
    /// <para>
    /// Если объект не передан, будет автоматически указана итоговая сумма чека с видом оплаты "Безналичный".
    /// </para>
    /// <para>
    /// Если передан то значение в <see cref="PaymentsForReceiptTBankModel.Electronic"/> должно быть равно итоговому значению <c>Commands.Init.Amount</c>.
    /// При этом сумма введенных значений по всем видам оплат, включая <see cref="PaymentsForReceiptTBankModel.Electronic"/>,
    /// должна быть равна сумме (<see cref="ReceiptItemTBankModel.Amount"/>) всех товаров, переданных в <see cref="Items"/>
    /// </para>
    /// </summary>
    public virtual PaymentsForReceiptTBankModel? Payments { get; set; }
}