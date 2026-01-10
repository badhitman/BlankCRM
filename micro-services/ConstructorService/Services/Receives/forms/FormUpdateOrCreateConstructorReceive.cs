////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить/создать форму (имя, описание, `признак таблицы`)
/// </summary>
public class FormUpdateOrCreateConstructorReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<FormBaseConstructorModel>?, TResponseModel<FormConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FormUpdateOrCreateConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<FormBaseConstructorModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.Id.ToString());
        TResponseModel<FormConstructorModelDB> res = await conService.FormUpdateOrCreateAsync(req, token);
        if (req.Payload.Id == 0 && res.Response is not null)
            trace.TraceReceiverRecordId = res.Response.Id.ToString();
        
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<FormConstructorModelDB>)), token);
        return res;
    }
}