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
    /// Price Full - file get
    /// </summary>
    public Task<FileAttachModel> PriceFullFileGet(CancellationToken token = default);

    /// <summary>
    /// Order report get
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> OrderReportGet(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Status order change
    /// </summary>
    public Task<TResponseModel<bool>> StatusOrderChangeByHelpdeskDocumentId(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Удалить ценообразование
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDelete(TAuthRequestModel<int> id, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать правило ценообразования
    /// </summary>
    public Task<TResponseModel<int>> PriceRuleUpdate(TAuthRequestModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать платёжный документ
    /// </summary>
    public Task<TResponseModel<int>> PaymentDocumentUpdate(TAuthRequestModel<PaymentDocumentBaseModel> payment, CancellationToken token = default);

    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffers(TAuthRequestModel<int[]> ids, CancellationToken token = default);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersRead(TAuthRequestModel<int[]> ids, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesRead(TAuthRequestModel<int[]> ids, CancellationToken token = default);

    /// <summary>
    /// Удалить платёжный документ
    /// </summary>
    public Task<ResponseBaseModel> PaymentDocumentDelete(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Удалить строку заказа
    /// </summary>
    public Task<TResponseModel<bool>> RowsForOrderDelete(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку заказа
    /// </summary>
    public Task<TResponseModel<int>> RowForOrderUpdate(RowOfOrderDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// OrdersRead
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersRead(TAuthRequestModel<int[]> orders_ids, CancellationToken token = default);

    /// <summary>
    /// OrderUpdate
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdate(OrderDocumentModelDB order, CancellationToken token = default);

    /// <summary>
    /// Подбор заказов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelect(TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssues(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Удалить Offer
    /// </summary>
    public Task<ResponseBaseModel> OfferDelete(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelect(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelect(TPaginationRequestModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// OrganizationUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdate(TAuthRequestModel<OfferModelDB> offer, CancellationToken token = default);

    /// <summary>
    /// Обновить/Создать товар
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdateReceive(NomenclatureModelDB req, CancellationToken token = default);
}