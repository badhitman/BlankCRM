////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить элемент справочника/списка
/// </summary>
public class DeleteElementFromDirectoryConstructorReceive(IConstructorService conService, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteElementFromDirectoryRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteElementFromDirectoryConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteElementFromDirectoryRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        ResponseBaseModel res = await conService.DeleteElementFromDirectoryAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}