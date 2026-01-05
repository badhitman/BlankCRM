////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Save file
/// </summary>
public class SaveFileReceive(ILogger<SaveFileReceive> LoggerRepo, IFilesStorage serializeStorageRepo)
    : IResponseReceive<TAuthRequestModel<StorageFileMetadataModel>?, TResponseModel<StorageFileModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SaveFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>?> ResponseHandleActionAsync(TAuthRequestModel<StorageFileMetadataModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await serializeStorageRepo.SaveFileAsync(req, token);
    }
}