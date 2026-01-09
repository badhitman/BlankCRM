////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

public partial interface ICommerceTransmission : ICommerceServiceBase
{
    /// <summary>
    /// Удалить строку складского документа
    /// </summary>
    public Task<RowsForWarehouseDocumentDeleteResponseModel> RowsForWarehouseDocumentDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку складского документа
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseUpdateAsync(RowOfWarehouseDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// WarehouseUpdate
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdateOrCreateAsync(WarehouseDocumentModelDB document, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);
}