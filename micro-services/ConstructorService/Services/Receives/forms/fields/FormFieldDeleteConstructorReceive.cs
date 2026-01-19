////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить поле формы (простой тип)
/// </summary>
public class FormFieldDeleteConstructorReceive(IConstructorService conService, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<FormFieldDeleteRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDeleteConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<FormFieldDeleteRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        ResponseBaseModel res = await conService.FormFieldDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
