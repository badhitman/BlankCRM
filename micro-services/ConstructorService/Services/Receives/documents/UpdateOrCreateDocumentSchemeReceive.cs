////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary> 
/// Обновить/создать схему документа
/// </summary>
public class UpdateOrCreateDocumentSchemeReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<EntryConstructedModel>?, TResponseModel<DocumentSchemeConstructorModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateDocumentSchemeReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<EntryConstructedModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<DocumentSchemeConstructorModelDB?> res = await conService.UpdateOrCreateDocumentSchemeAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
