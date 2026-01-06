////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить страницу анкеты/опроса
/// </summary>
public class GetTabOfDocumentSchemeConstructorReceive(IConstructorService conService)
    : IResponseReceive<int, TResponseModel<TabOfDocumentSchemeConstructorModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetTabOfDocumentSchemeConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB?>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetTabOfDocumentSchemeAsync(payload, token);
    }
}