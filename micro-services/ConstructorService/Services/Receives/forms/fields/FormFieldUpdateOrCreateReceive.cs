////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить/создать поле формы (простой тип)
/// </summary>
public class FormFieldUpdateOrCreateReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<FieldFormConstructorModelDB>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldUpdateOrCreateReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<FieldFormConstructorModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await conService.FormFieldUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
