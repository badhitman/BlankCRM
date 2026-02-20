////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Перемещение страницы опроса/анкеты (сортировка страниц внутри опроса/анкеты)
/// </summary>
public class MoveTabOfDocumentSchemeConstructorReceive(IConstructorService conService) 
    : IResponseReceive<TAuthRequestStandardModel<MoveObjectModel>?, TResponseModel<DocumentSchemeConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabOfDocumentSchemeConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<MoveObjectModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.MoveTabOfDocumentSchemeAsync(payload, token);
    }
}
