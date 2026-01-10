////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// FilesSelectReceive
/// </summary>
public class FilesSelectReceive(IFilesStorage serializeStorageRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<SelectMetadataRequestModel>?, TPaginationResponseStandardModel<StorageFileModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FilesSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<StorageFileModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.FilesSelectAsync(req, token);
    }
}