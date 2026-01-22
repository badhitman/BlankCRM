////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// GetMembersOfProjectReceive
/// </summary>
public class GetMembersOfProjectConstructorReceive(IConstructorService conService) 
    : IResponseReceive<int, TResponseModel<EntryAltStandardModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetMembersOfProjectConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltStandardModel[]>?> ResponseHandleActionAsync(int req, CancellationToken token = default)
    {
        return await conService.GetMembersOfProjectAsync(req, token);
    }
}