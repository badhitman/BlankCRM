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

    /// <summary>
    /// Оплаты [на сайте]
    /// </summary>
    public decimal PaidOnSitePaymentsSumAmount { get; set; }

    /// <summary>
    /// Оплаты [на сайте]
    /// </summary>
    public int PaidOnSitePaymentsCount { get; set; }

    /// <summary>
    /// Оплаты [не на сайте]
    /// </summary>
    public decimal PaidNoSitePaymentsSumAmount { get; set; }

    /// <summary>
    /// Оплаты [не на сайте]
    /// </summary>
    public decimal PaidNoSitePaymentsCount { get; set; }

    /// <summary>
    /// Поступило в переводах/конвертации
    /// </summary>
    public decimal ConversionsSumAmount { get; set; }
    public int ConversionsCount { get; set; }
}