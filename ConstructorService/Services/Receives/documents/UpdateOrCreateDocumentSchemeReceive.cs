////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary> 
/// Обновить/создать схему документа
/// </summary>
public class UpdateOrCreateDocumentSchemeReceive(IConstructorService conService) : IResponseReceive<TAuthRequestModel<EntryConstructedModel>?, TResponseModel<DocumentSchemeConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.UpdateOrCreateDocumentSchemeReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestModel<EntryConstructedModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.UpdateOrCreateDocumentScheme(payload, token);
    }
}
