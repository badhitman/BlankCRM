////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// ProjectsReadReceive
/// </summary>
public class ProjectsReadReceive(IConstructorService conService) : IResponseReceive<int[]?, List<ProjectModelDb>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProjectsReadReceive;

    /// <inheritdoc/>
    public async Task<List<ProjectModelDb>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await conService.ReadProjectsAsync(req, token);
    }
}