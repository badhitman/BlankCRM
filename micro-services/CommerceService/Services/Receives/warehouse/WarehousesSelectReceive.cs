﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// WarehousesSelectReceive
/// </summary>
public class WarehousesSelectReceive(ICommerceService commRepo) : IResponseReceive<TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel>?, TPaginationResponseModel<WarehouseDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WarehousesSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WarehouseDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.WarehouseDocumentsSelectAsync(req, token);
    }
}