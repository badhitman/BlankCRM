////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

public partial interface ICommerceTransmission : ICommerceServiceBase
{
    /// <summary>
    /// WarehouseUpdate
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdateOrCreateAsync(WarehouseDocumentModelDB document, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);
}