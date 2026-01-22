////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// GetDirectoriesReceive
/// </summary>
public class GetDirectoriesConstructorReceive(IConstructorService conService) 
    : IResponseReceive<ProjectFindModel?, TResponseModel<EntryStandardModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoriesConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryStandardModel[]>?> ResponseHandleActionAsync(ProjectFindModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.GetDirectoriesAsync(payload, token);
    }
}