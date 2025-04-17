////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.HaierProff;

/// <summary>
/// DownloadAndSave
/// </summary>
public class DownloadAndSaveReceive(IFeedsHaierProffRuService haierRepo)
    : IResponseReceive<object?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DownloadAndSaveHaierProffReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await haierRepo.DownloadAndSaveAsync(token);
    }
}