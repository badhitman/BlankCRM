////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Warehouse
/// </summary>
public partial interface ICommerceService : ICommerceServiceBase
{
    /// <summary>
    /// WarehouseDocument update
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdateOrCreateAsync(WarehouseDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocuments select
    /// </summary>
    public Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);
}