////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить (или создать) сессию опроса/анкеты
/// </summary>
public class UpdateOrCreateSessionDocumentReceive(IConstructorService conService) 
    : IResponseReceive<SessionOfDocumentDataModelDB?, TResponseModel<SessionOfDocumentDataModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateSessionDocumentReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB?>?> ResponseHandleActionAsync(SessionOfDocumentDataModelDB? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.UpdateOrCreateSessionDocumentAsync(payload, token);
    }
}
