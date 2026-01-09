////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Orders
/// </summary>
public partial interface ICommerceService : ICommerceServiceBase
{
    #region price-rule
    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// PriceRuleDelete
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion

    #region offer
    /// <summary>
    /// OfferUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestStandardModel<OfferModelDB> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// OfferDelete
    /// </summary>
    public Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion

    #region nomenclatures
    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

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
    public Task<TResponseModel<bool>> StatusesOrdersChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);

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
    public Task<TPaginationResponseStandardModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Orders read
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Orders by issues get
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);
    #endregion


    /// <summary>
    /// Get order report file Excel (*.xlsx)
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> GetOrderReportFileAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
}