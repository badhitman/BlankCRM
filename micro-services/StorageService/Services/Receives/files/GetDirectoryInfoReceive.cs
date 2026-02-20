////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// GetDirectoryInfo
/// </summary>
public class GetDirectoryInfoReceive(IFilesStorage serializeStorageRepo)
    : IResponseReceive<DirectoryReadRequestModel?, TResponseModel<DirectoryReadResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoryInfoReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DirectoryReadResponseModel>?> ResponseHandleActionAsync(DirectoryReadRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.GetDirectoryInfoAsync(req, token);
    }
}