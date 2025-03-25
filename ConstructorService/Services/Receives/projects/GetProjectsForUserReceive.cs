////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// GetProjectsForUserReceive
/// </summary>
public class GetProjectsForUserReceive(IConstructorService conService) : IResponseReceive<GetProjectsForUserRequestModel?, TResponseModel<ProjectViewModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ProjectsForUserReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ProjectViewModel[]>?> ResponseHandleActionAsync(GetProjectsForUserRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await conService.GetProjectsForUserAsync(req, token);
    }
}