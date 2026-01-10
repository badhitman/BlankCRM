////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Read file
/// </summary>
public class ReadFileReceive(IFilesStorage serializeStorageRepo)
    : IResponseReceive<TAuthRequestStandardModel<RequestFileReadModel>?, TResponseModel<FileContentModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>?> ResponseHandleActionAsync(TAuthRequestStandardModel<RequestFileReadModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.ReadFileAsync(req, token);
    }
}