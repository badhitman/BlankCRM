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
    public Task<TResponseModel<int>> PaymentDocumentUpdate(TAuthRequestModel<PaymentDocumentBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// PaymentDocumentDelete
    /// </summary>
    public Task<ResponseBaseModel> PaymentDocumentDelete(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion

    #region price-rule
    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffers(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// PriceRuleUpdate
    /// </summary>
    public Task<TResponseModel<int>> PriceRuleUpdate(TAuthRequestModel<PriceRuleForOfferModelDB> req, CancellationToken token = default);

    /// <summary>
    /// PriceRuleDelete
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDelete(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion

    #region offer
    /// <summary>
    /// OfferUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdate(TAuthRequestModel<OfferModelDB> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelect(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersRead(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// OfferDelete
    /// </summary>
    public Task<ResponseBaseModel> OfferDelete(TAuthRequestModel<int> req, CancellationToken token = default);

    #endregion

    #region nomenclatures

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelect(TPaginationRequestModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesRead(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// NomenclatureUpdate
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdate(NomenclatureModelDB nom, CancellationToken token = default);

    #endregion

    #region orders

    /// <summary>
    /// Смена статуса заказу по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> StatusesOrdersChangeByHelpdeskDocumentId(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Rows for order delete
    /// </summary>
    public Task<TResponseModel<bool>> RowsForOrderDelete(int[] req, CancellationToken token = default);

    /// <summary>
    /// Row for order update
    /// </summary>
    public Task<TResponseModel<int>> RowForOrderUpdate(RowOfOrderDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Order update
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdate(OrderDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Orders select
    /// </summary>
    public Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelect(TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Orders read
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersRead(TAuthRequestModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Orders by issues get
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGet(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    #endregion

    /// <summary>
    /// Get full price file Excel (*.xlsx)
    /// </summary>
    public Task<FileAttachModel> GetFullPriceFile(CancellationToken token = default);

    /// <summary>
    /// Get order report file Excel (*.xlsx)
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> GetOrderReportFile(TAuthRequestModel<int> req, CancellationToken token = default);
}