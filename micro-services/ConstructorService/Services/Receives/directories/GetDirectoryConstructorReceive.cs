////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// GetDirectoryReceive
/// </summary>
public class GetDirectoryConstructorReceive(IConstructorService conService) 
    : IResponseReceive<int, TResponseModel<EntryDescriptionModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoryConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetDirectoryAsync(payload, token);
    }
}