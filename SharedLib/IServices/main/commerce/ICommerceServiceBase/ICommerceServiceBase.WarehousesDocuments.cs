////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{
    /// <summary>
    /// WarehouseDocuments read
    /// </summary>
    public Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesDocumentsReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocument update
    /// </summary>
    public Task<DocumentNewVersionResponseModel> WarehouseDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<WarehouseDocumentModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Обновить (или создать) строку складского документа
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<RowOfWarehouseDocumentModelDB> row, CancellationToken token = default);

    /// <summary>
    /// Удалить строку складского документа
    /// </summary>
    public Task<TResponseModel<Dictionary<int, DeliveryDocumentMetadataModel>>> RowsDeleteFromWarehouseDocumentAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);
}