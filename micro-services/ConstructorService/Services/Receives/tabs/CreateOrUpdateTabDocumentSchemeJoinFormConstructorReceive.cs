////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить/создать связь [таба/вкладки схемы документа] с [формой]
/// </summary>
public class CreateOrUpdateTabDocumentSchemeJoinFormConstructorReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<FormToTabJoinConstructorModelDB>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrUpdateTabDocumentSchemeJoinFormConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<FormToTabJoinConstructorModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.FormId.ToString());
        ResponseBaseModel res = await conService.CreateOrUpdateTabDocumentSchemeJoinFormAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}
