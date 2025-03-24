////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// WarehousesDocumentsReadReceive
/// </summary>
public class WarehousesDocumentsReadReceive(ICommerceService commRepo) : IResponseReceive<int[]?, TResponseModel<WarehouseDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.WarehousesDocumentsReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<WarehouseDocumentModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.WarehouseDocumentsRead(req, token);
    }
}