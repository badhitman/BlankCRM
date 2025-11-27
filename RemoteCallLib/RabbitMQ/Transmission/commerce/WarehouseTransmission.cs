////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace RemoteCallLib;

public partial class CommerceTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OfferAvailabilityModelDB>>(TransmissionQueues.OffersRegistersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForWarehouseDeleteAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForWarehouseUpdateAsync(RowOfWarehouseDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.RowForWarehouseDocumentUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WarehouseDocumentModelDB[]>>(TransmissionQueues.WarehousesDocumentsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WarehouseDocumentUpdateAsync(WarehouseDocumentModelDB document, CancellationToken token = default)
    {
        document.DeliveryDate = document.DeliveryDate.ToUniversalTime();
        return await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.WarehouseDocumentUpdateCommerceReceive, document, token: token) ?? new();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WarehouseDocumentModelDB>>(TransmissionQueues.WarehousesSelectCommerceReceive, req, token: token) ?? new();
}