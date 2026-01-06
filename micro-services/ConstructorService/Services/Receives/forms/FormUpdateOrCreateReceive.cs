////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить/создать форму (имя, описание, `признак таблицы`)
/// </summary>
public class FormUpdateOrCreateReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<FormBaseConstructorModel>?, TResponseModel<FormConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FormUpdateOrCreateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<FormBaseConstructorModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<FormConstructorModelDB> res = await conService.FormUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}