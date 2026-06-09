////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// LogsClearReceive
/// </summary>
public class LogsClearReceive(ILogsService storeRepo)
    : IResponseReceive<LogsClearRequestModel?, LogsClearResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.LogsClearStorageReceive;

    /// <inheritdoc/>
    public async Task<LogsClearResponseModel?> ResponseHandleActionAsync(LogsClearRequestModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await storeRepo.LogsClearAsync(payload, token);
    }
}