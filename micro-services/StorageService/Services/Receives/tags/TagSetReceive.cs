////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// TagSetReceive
/// </summary>
public class TagSetReceive(IParametersStorage serializeStorageRepo)
    : IResponseReceive<TagSetModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TagSetReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TagSetModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        ResponseBaseModel res = await serializeStorageRepo.TagSetAsync(req, token);
        return res;
    }
}