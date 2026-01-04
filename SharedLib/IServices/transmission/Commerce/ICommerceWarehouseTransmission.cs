////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

public partial interface ICommerceTransmission : ICommerceServiceBase
{
    /// <summary>
    /// Получить остатки
    /// </summary>
    public Task<TPaginationResponseModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Удалить строку складского документа
    /// </summary>
    public Task<TResponseModel<bool>> RowsForWarehouseDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку складского документа
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseUpdateAsync(RowOfWarehouseDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// WarehouseUpdate
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdateAsync(WarehouseDocumentModelDB document, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);
}