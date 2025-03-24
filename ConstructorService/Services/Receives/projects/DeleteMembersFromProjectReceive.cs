////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// DeleteMembersFromProjectReceive
/// </summary>
public class DeleteMembersFromProjectReceive(IConstructorService conService) : IResponseReceive<UsersProjectModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.DeleteMembersFromProjectReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(UsersProjectModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await conService.DeleteMembersFromProject(req, token);
    }
}