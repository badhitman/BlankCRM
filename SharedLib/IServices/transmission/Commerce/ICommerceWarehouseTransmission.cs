////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

public partial interface ICommerceTransmission
{
    /// <summary>
    /// Получить остатки
    /// </summary>
    public Task<TPaginationResponseModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Удалить строку складского документа
    /// </summary>
    public Task<TResponseModel<bool>> RowsForWarehouseDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку складского документа
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseUpdateAsync(RowOfWarehouseDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// WarehousesRead
    /// </summary>
    public Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesReadAsync(int[] ids, CancellationToken token = default);

    /// <summary>
    /// WarehouseUpdate
    /// </summary>
    public Task<TResponseModel<int>> WarehouseUpdateAsync(WarehouseDocumentModelDB document, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehousesSelectAsync(TPaginationRequestModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);
}