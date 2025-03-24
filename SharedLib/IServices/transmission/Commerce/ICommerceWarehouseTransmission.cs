////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

public partial interface ICommerceTransmission
{
    /// <summary>
    /// Получить остатки
    /// </summary>
    public Task<TPaginationResponseModel<OfferAvailabilityModelDB>> OffersRegistersSelect(TPaginationRequestModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Удалить строку складского документа
    /// </summary>
    public Task<TResponseModel<bool>> RowsForWarehouseDelete(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку складского документа
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseUpdate(RowOfWarehouseDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// WarehousesRead
    /// </summary>
    public Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesRead(int[] ids, CancellationToken token = default);

    /// <summary>
    /// WarehouseUpdate
    /// </summary>
    public Task<TResponseModel<int>> WarehouseUpdate(WarehouseDocumentModelDB document, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehousesSelect(TPaginationRequestModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);
}