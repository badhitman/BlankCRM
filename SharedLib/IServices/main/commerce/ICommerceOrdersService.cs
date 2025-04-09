////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Orders
/// </summary>
public partial interface ICommerceService
{
    #region payment-document
    /// <summary>
    /// PaymentDocumentUpdate
    /// </summary>
    public Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestModel<PaymentDocumentBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// PaymentDocumentDelete
    /// </summary>
    public Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion

    #region price-rule
    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// PriceRuleUpdate
    /// </summary>
    public Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestModel<PriceRuleForOfferModelDB> req, CancellationToken token = default);

    /// <summary>
    /// PriceRuleDelete
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion

    #region offer
    /// <summary>
    /// OfferUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestModel<OfferModelDB> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// OfferDelete
    /// </summary>
    public Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    #endregion

    #region nomenclatures

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// NomenclatureUpdate
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB nom, CancellationToken token = default);

    #endregion

    #region orders

    /// <summary>
    /// Смена статуса заказу по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> StatusesOrdersChangeByHelpdeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Rows for order delete
    /// </summary>
    public Task<TResponseModel<bool>> RowsForOrderDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Row for order update
    /// </summary>
    public Task<TResponseModel<int>> RowForOrderUpdateAsync(RowOfOrderDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Order update
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Orders select
    /// </summary>
    public Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Orders read
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Orders by issues get
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    #endregion

    /// <summary>
    /// Get full price file Excel (*.xlsx)
    /// </summary>
    public Task<FileAttachModel> GetFullPriceFileAsync(CancellationToken token = default);

    /// <summary>
    /// Get order report file Excel (*.xlsx)
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> GetOrderReportFileAsync(TAuthRequestModel<int> req, CancellationToken token = default);
}