////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Read parameter
/// </summary>
public class ReadParameterReceive(IParametersStorage serializeStorageRepo, ILogger<ReadParameterReceive> LoggerRepo)
    : IResponseReceive<StorageMetadataModel?, TResponseModel<StorageCloudParameterPayloadModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadCloudParameterReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageCloudParameterPayloadModel>?> ResponseHandleActionAsync(StorageMetadataModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await serializeStorageRepo.ReadParameterAsync(req, token);
    }
}