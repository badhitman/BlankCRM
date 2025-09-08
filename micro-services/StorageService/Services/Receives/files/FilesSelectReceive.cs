////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// FilesSelectReceive
/// </summary>
public class FilesSelectReceive(ILogger<FilesSelectReceive> loggerRepo, IFilesStorage serializeStorageRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<SelectMetadataRequestModel>?, TPaginationResponseModel<StorageFileModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FilesSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<StorageFileModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await serializeStorageRepo.FilesSelectAsync(req, token);
    }
}