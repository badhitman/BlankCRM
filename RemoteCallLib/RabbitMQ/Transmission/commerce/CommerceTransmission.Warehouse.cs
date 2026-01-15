////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsTransmission;
using SharedLib;

namespace RemoteCallLib;

public partial class CommerceTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OfferAvailabilityModelDB>>(TransmissionQueues.OffersRegistersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Dictionary<int, DeliveryDocumentMetadataModel>>> RowsDeleteFromWarehouseDocumentAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Dictionary<int, DeliveryDocumentMetadataModel>>>(TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForWarehouseDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<RowOfWarehouseDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.RowForWarehouseDocumentUpdateOrCreateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesDocumentsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WarehouseDocumentModelDB[]>>(TransmissionQueues.WarehousesDocumentsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> WarehouseDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<WarehouseDocumentModelDB> document, CancellationToken token = default)
         => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.WarehouseDocumentUpdateOrCreateCommerceReceive, document, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WarehouseDocumentModelDB>>(TransmissionQueues.WarehouseDocumentsSelectCommerceReceive, req, token: token) ?? new();
}