////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

public partial class CommerceTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OfferAvailabilityModelDB>> OffersRegistersSelect(TPaginationRequestModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TPaginationResponseModel<OfferAvailabilityModelDB>>(GlobalStaticConstants.TransmissionQueues.OffersRegistersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForWarehouseDelete(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForWarehouseUpdate(RowOfWarehouseDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.RowForWarehouseDocumentUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesRead(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<WarehouseDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.WarehousesDocumentsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WarehouseUpdate(WarehouseDocumentModelDB document, CancellationToken token = default)
    {
        document.DeliveryDate = document.DeliveryDate.ToUniversalTime();
        return await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.WarehouseDocumentUpdateCommerceReceive, document, token: token) ?? new();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehousesSelect(TPaginationRequestModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TPaginationResponseModel<WarehouseDocumentModelDB>>(GlobalStaticConstants.TransmissionQueues.WarehousesSelectCommerceReceive, req, token: token) ?? new();
}