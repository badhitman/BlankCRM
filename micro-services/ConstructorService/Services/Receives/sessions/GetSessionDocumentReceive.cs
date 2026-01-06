////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить сессию
/// </summary>
public class GetSessionDocumentReceive(IConstructorService conService) 
    : IResponseReceive<SessionGetModel?, TResponseModel<SessionOfDocumentDataModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetSessionDocumentConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB?>?> ResponseHandleActionAsync(SessionGetModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.GetSessionDocumentAsync(payload, token);
    }
}
