////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить значения (введённые в сессиях) по имени поля
/// </summary>
public class ClearValuesForFieldNameConstructorReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<FormFieldOfSessionModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ClearValuesForFieldNameConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(FormFieldOfSessionModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.SessionId.ToString());
        ResponseBaseModel res = await conService.ClearValuesForFieldNameAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}