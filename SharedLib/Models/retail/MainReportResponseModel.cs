////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MainReportResponseModel
/// </summary>
public class MainReportResponseModel
{
    /// <summary>
    /// Готовые заказы
    /// </summary>
    /// <remarks>
    /// Количество документов-заказов в статусе Done (Готов/Выполнен)
    /// </remarks>
    public int DoneOrdersCount { get; set; }

    /// <summary>
    /// Сумма (цена * количество) по готовым заказам
    /// </summary>
    public decimal DoneOrdersSumAmount { get; set; }
}