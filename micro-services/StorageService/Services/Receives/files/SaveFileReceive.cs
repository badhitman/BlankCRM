////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Save file
/// </summary>
public class SaveFileReceive(IFilesStorage serializeStorageRepo)
    : IResponseReceive<TAuthRequestStandardModel<StorageFileMetadataModel>?, TResponseModel<StorageFileModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SaveFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<StorageFileMetadataModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.SaveFileAsync(req, token);
    }
}