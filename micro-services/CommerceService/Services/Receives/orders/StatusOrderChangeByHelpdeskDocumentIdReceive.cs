////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// StatusOrderChangeByHelpDeskDocumentIdReceive
/// </summary>
public class StatusOrderChangeByHelpDeskDocumentIdReceive(ICommerceService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<StatusChangeRequestModel>?, TResponseModel<OrderDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.StatusOrderChangeByHelpDeskDocumentIdReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>?> ResponseHandleActionAsync(TAuthRequestStandardModel<StatusChangeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<OrderDocumentModelDB[]> res = await commRepo.StatusOrderChangeByHelpDeskDocumentIdAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}