////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить форму
/// </summary>
public class GetFormConstructorReceive(IConstructorService conService) 
    : IResponseReceive<int, TResponseModel<FormConstructorModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetFormConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB?>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetFormAsync(payload, token);
    }
}