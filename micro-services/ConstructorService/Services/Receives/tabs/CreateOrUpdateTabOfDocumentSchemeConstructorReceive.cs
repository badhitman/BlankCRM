////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить/создать таб/вкладку схемы документа
/// </summary>
public class CreateOrUpdateTabOfDocumentSchemeConstructorReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<EntryDescriptionOwnedModel>?, TResponseModel<TabOfDocumentSchemeConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrUpdateTabOfDocumentSchemeConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<EntryDescriptionOwnedModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.OwnerId.ToString());
        TResponseModel<TabOfDocumentSchemeConstructorModelDB> res = await conService.CreateOrUpdateTabOfDocumentSchemeAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<TabOfDocumentSchemeConstructorModelDB>)), token);
        return res;
    }
}