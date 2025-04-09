////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Warehouse
/// </summary>
public partial interface ICommerceService
{
    /// <summary>
    /// Rows for warehouse document delete
    /// </summary>
    public Task<TResponseModel<bool>> RowsForWarehouseDocumentDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Row for warehouse document update
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseDocumentUpdateAsync(RowOfWarehouseDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocument update
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdateAsync(WarehouseDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocuments select
    /// </summary>
    public Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocuments read
    /// </summary>
    public Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehouseDocumentsReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Registers select
    /// </summary>
    public Task<TPaginationResponseModel<OfferAvailabilityModelDB>> RegistersSelectAsync(TPaginationRequestModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);
}