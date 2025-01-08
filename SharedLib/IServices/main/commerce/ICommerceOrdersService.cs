﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Orders
/// </summary>
public partial interface ICommerceService
{
    /// <summary>
    /// OffersSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelect(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>> req);

    /// <summary>
    /// OffersRead
    /// </summary>
    public Task<TResponseModel<OfferModelDB[]>> OffersRead(TAuthRequestModel<int[]> req);

    /// <summary>
    /// OfferDelete
    /// </summary>
    public Task<ResponseBaseModel> OfferDelete(TAuthRequestModel<int> req);

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelect(TPaginationRequestModel<NomenclaturesSelectRequestModel> req);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesRead(TAuthRequestModel<int[]> req);

    /// <summary>
    /// NomenclatureUpdate
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdate(NomenclatureModelDB nom);

    /// <summary>
    /// Get full price file Excel (*.xlsx)
    /// </summary>
    public Task<FileAttachModel> GetFullPriceFile();

    /// <summary>
    /// Get order report file
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> GetOrderReportFile(TAuthRequestModel<int> req);

    /// <summary>
    /// Смена статуса заказу по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> StatusesOrdersChangeByHelpdeskDocumentId(TAuthRequestModel<StatusChangeRequestModel> req);

    /// <summary>
    /// Rows for order delete
    /// </summary>
    public Task<TResponseModel<bool>> RowsForOrderDelete(int[] req);

    /// <summary>
    /// Row for order update
    /// </summary>
    public Task<TResponseModel<int>> RowForOrderUpdate(RowOfOrderDocumentModelDB req);

    /// <summary>
    /// Order update
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdate(OrderDocumentModelDB req);

    /// <summary>
    /// Orders select
    /// </summary>
    public Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelect(TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>> req);

    /// <summary>
    /// Orders read
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersRead(int[] req);

    /// <summary>
    /// Orders by issues get
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGet(OrdersByIssuesSelectRequestModel req);
}