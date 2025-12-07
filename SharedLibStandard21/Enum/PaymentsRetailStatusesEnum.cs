////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Статусы платежей
/// </summary>
public enum PaymentsRetailStatusesEnum
{
    /// <summary>
    /// Ожидает оплаты
    /// </summary>
    [Description("Ожидает оплаты")]
    Awaiting = 0,

    /// <summary>
    /// Оплачено
    /// </summary>
    [Description("Оплачено")]
    Paid = 20,

    /// <summary>
    /// Отменено
    /// </summary>
    [Description("Отменено")]
    Canceled = 1000
}
