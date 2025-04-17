////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Сохранить данные формы документа из сессии
/// </summary>
public class SaveSessionFormReceive(IConstructorService conService) : IResponseReceive<SaveConstructorSessionRequestModel?, TResponseModel<ValueDataForSessionOfDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SaveSessionFormReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>?> ResponseHandleActionAsync(SaveConstructorSessionRequestModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.SaveSessionFormAsync(payload, token);
    }
}
