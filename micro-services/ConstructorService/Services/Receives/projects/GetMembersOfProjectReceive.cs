////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// GetMembersOfProjectReceive
/// </summary>
public class GetMembersOfProjectReceive(IConstructorService conService) : IResponseReceive<int, TResponseModel<EntryAltModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetMembersOfProjectReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltModel[]>?> ResponseHandleActionAsync(int req, CancellationToken token = default)
    {
        return await conService.GetMembersOfProjectAsync(req, token);
    }
}