////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Read parameter`s list
/// </summary>
public class ReadParametersReceive(IParametersStorage serializeStorageRepo, ILogger<ReadParametersReceive> LoggerRepo)
    : IResponseReceive<StorageMetadataModel[]?, TResponseModel<List<StorageCloudParameterPayloadModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadCloudParametersReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<StorageCloudParameterPayloadModel>>?> ResponseHandleActionAsync(StorageMetadataModel[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await serializeStorageRepo.ReadParametersAsync(req, token);
    }
}