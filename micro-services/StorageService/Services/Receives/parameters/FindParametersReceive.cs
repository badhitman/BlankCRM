////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Find parameters
/// </summary>
public class FindParametersReceive(IParametersStorage serializeStorageRepo, ILogger<FindParametersReceive> LoggerRepo)
    : IResponseReceive<FindStorageBaseModel?, TResponseModel<FoundParameterModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FindCloudParameterReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FoundParameterModel[]>?> ResponseHandleActionAsync(FindStorageBaseModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await serializeStorageRepo.FindRawAsync(req, token);
    }
}