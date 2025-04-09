////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// DownloadAndSaveReceive
/// </summary>
public class DownloadAndSaveReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<object?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.DownloadAndSaveBreezReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.DownloadAndSaveAsync(token);
    }
}