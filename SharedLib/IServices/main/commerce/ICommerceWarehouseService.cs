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
    public Task<TResponseModel<bool>> RowsForWarehouseDocumentDelete(int[] req, CancellationToken token = default);

    /// <summary>
    /// Row for warehouse document update
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseDocumentUpdate(RowOfWarehouseDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocument update
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdate(WarehouseDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocuments select
    /// </summary>
    public Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelect(TPaginationRequestModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocuments read
    /// </summary>
    public Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehouseDocumentsRead(int[] req, CancellationToken token = default);

    /// <summary>
    /// Registers select
    /// </summary>
    public Task<TPaginationResponseModel<OfferAvailabilityModelDB>> RegistersSelect(TPaginationRequestModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);
}