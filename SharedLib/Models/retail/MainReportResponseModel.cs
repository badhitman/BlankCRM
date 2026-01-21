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
    public List<PaymentOrderNoSiteRetailModel> PaidNoSitePayments { get; set; } = [];

    decimal? _paidNoSitePaymentsSumAmount;
    /// <summary>
    /// Оплаты [не на сайте]
    /// </summary>
    public decimal PaidNoSitePaymentsSumAmount
    {
        get
        {
            if (!_paidNoSitePaymentsSumAmount.HasValue)
                _paidNoSitePaymentsSumAmount = PaidNoSitePayments.Count == 0
                    ? 0
                    : PaidNoSitePayments.Sum(x => x.AmountPayment);

            return _paidNoSitePaymentsSumAmount.Value;
        }
    }

    decimal? _paidNoSitePaymentsCount;
    /// <summary>
    /// Оплаты [не на сайте]
    /// </summary>
    public decimal PaidNoSitePaymentsCount
    {
        get
        {
            if (!_paidNoSitePaymentsCount.HasValue)
                _paidNoSitePaymentsCount = PaidNoSitePayments.Count == 0
                    ? 0
                    : PaidNoSitePayments.Count;

            return _paidNoSitePaymentsCount.Value;
        }
    }


    /// <summary>
    /// Поступило в переводах/конвертации
    /// </summary>
    public decimal ConversionsSumAmount { get; set; }

    /// <summary>
    /// Поступило в переводах/конвертации
    /// </summary>
    public int ConversionsCount { get; set; }
}