////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Read parameter`s list
/// </summary>
public class ReadParametersReceive(IParametersStorage serializeStorageRepo)
    : IResponseReceive<StorageMetadataModel[]?, TResponseModel<List<StorageCloudParameterPayloadModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadCloudParametersReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<StorageCloudParameterPayloadModel>>?> ResponseHandleActionAsync(StorageMetadataModel[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.ReadParametersAsync(req, token);
    }
}