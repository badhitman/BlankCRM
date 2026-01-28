////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// ReadFileDataAboutPosition
/// </summary>
public class ReadFileDataAboutPositionReceive(IFilesStorage serializeStorageRepo)
    : IResponseReceive<ReadFileDataAboutPositionRequestModel?, TResponseModel<Dictionary<DirectionsEnum, byte[]>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileDataAboutPositionReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<Dictionary<DirectionsEnum, byte[]>>?> ResponseHandleActionAsync(ReadFileDataAboutPositionRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.ReadFileDataAboutPositionAsync(req, token);
    }
}