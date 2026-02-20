////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// TotalWeightOrdersLinksDocuments
/// </summary>
public class TotalWeightOrdersLinksDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TotalWeightDeliveriesOrdersLinksDocumentsRequestModel?, TResponseModel<decimal>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TotalWeightOrdersLinksDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>?> ResponseHandleActionAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.TotalWeightOrdersDocumentsLinksAsync(req, token);
    }
}