////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Установить значение свойства сессии
/// </summary>
public class SetValueFieldSessionDocumentDataReceive(IConstructorService conService) 
    : IResponseReceive<SetValueFieldDocumentDataModel?, TResponseModel<SessionOfDocumentDataModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetValueFieldSessionDocumentDataConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB?>?> ResponseHandleActionAsync(SetValueFieldDocumentDataModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.SetValueFieldSessionDocumentDataAsync(payload, token);
    }
}
