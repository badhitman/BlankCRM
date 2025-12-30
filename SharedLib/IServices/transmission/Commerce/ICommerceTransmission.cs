////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// E-Commerce Remote Transmission Service
/// </summary>
public partial interface ICommerceTransmission : ICommerceServiceBase
{
    /// <summary>
    /// IncomingMerchantPaymentTBankAsync
    /// </summary>
    public Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default);

    /// <summary>
    /// Order report get
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> OrderReportGetAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Status order change
    /// </summary>
    public Task<TResponseModel<bool>> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Удалить ценообразование
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestModel<int> id, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать правило ценообразования
    /// </summary>
    public Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать платёжный документ
    /// </summary>
    public Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestModel<PaymentDocumentBaseModel> payment, CancellationToken token = default);

    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default);

    /// <summary>
    /// Удалить платёжный документ
    /// </summary>
    public Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Удалить строку заказа
    /// </summary>
    public Task<TResponseModel<bool>> RowsForOrderDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку заказа
    /// </summary>
    public Task<TResponseModel<int>> RowForOrderUpdateAsync(RowOfOrderDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// OrdersRead
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestModel<int[]> orders_ids, CancellationToken token = default);

    /// <summary>
    /// OrderUpdate
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB order, CancellationToken token = default);

    /// <summary>
    /// Подбор заказов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Удалить Offer
    /// </summary>
    public Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// OrganizationUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestModel<OfferModelDB> offer, CancellationToken token = default);

    /// <summary>
    /// Обновить/Создать товар
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB req, CancellationToken token = default);
}