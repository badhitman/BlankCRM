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
    /// Order report get
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> OrderReportGetAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Status order change
    /// </summary>
    public Task<TResponseModel<bool>> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// OrdersRead
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestStandardModel<int[]> orders_ids, CancellationToken token = default);

    /// <summary>
    /// OrderUpdate
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB order, CancellationToken token = default);

    /// <summary>
    /// Подбор заказов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseStandardModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Удалить Offer
    /// </summary>
    public Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// OrganizationUpdate
    /// </summary>
    public Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestStandardModel<OfferModelDB> offer, CancellationToken token = default);

    /// <summary>
    /// Обновить/Создать товар
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB req, CancellationToken token = default);
}