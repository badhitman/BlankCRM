////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace RemoteCallLib;

public partial class CommerceTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OfferAvailabilityModelDB>>(TransmissionQueues.OffersRegistersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<RowsForWarehouseDocumentDeleteResponseModel> RowsForWarehouseDocumentDeleteAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<RowsForWarehouseDocumentDeleteResponseModel>(TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForWarehouseDocumentUpdateAsync(RowOfWarehouseDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.RowForWarehouseDocumentUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesDocumentsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WarehouseDocumentModelDB[]>>(TransmissionQueues.WarehousesDocumentsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WarehouseDocumentUpdateOrCreateAsync(WarehouseDocumentModelDB document, CancellationToken token = default)
    {
        document.DeliveryDate = document.DeliveryDate.ToUniversalTime();
        return await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.WarehouseDocumentUpdateOrCreateCommerceReceive, document, token: token) ?? new();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WarehouseDocumentModelDB>>(TransmissionQueues.WarehouseDocumentsSelectCommerceReceive, req, token: token) ?? new();
}