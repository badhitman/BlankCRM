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
    : IResponseReceive<TAuthRequestModel<FormBaseConstructorModel>?, TResponseModel<FormConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FormUpdateOrCreateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestModel<FormBaseConstructorModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        TResponseModel<FormConstructorModelDB> res = await conService.FormUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}