////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Orders
/// </summary>
public partial interface ICommerceService : ICommerceServiceBase
{
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
    /// Order update
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Orders select
    /// </summary>
    public Task<TPaginationResponseStandardModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

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