////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Статусы платежей
/// </summary>
public enum PaymentsRetailStatusesEnum
{
    /// <summary>
    /// Ожидает оплаты
    /// </summary>
    Awaiting =10,

    /// <summary>
    /// Оплачено
    /// </summary>
    Paid  = 20,

    /// <summary>
    /// Отменено
    /// </summary>
    Canceled = 1000
}
