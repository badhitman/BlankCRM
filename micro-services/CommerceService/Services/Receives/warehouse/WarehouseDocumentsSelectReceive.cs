////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// <see cref="WarehouseDocumentsSelectReceive"/>
/// </summary>
public class WarehouseDocumentsSelectReceive(ICommerceService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel>?, TPaginationResponseStandardModel<WarehouseDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WarehouseDocumentsSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.WarehouseDocumentsSelectAsync(req, token);
    }
}