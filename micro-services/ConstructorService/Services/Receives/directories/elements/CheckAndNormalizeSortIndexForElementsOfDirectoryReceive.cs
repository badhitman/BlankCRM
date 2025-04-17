////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// CheckAndNormalizeSortIndexForElementsOfDirectoryReceive
/// </summary>
public class CheckAndNormalizeSortIndexForElementsOfDirectoryReceive(IConstructorService conService) : IResponseReceive<int, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CheckAndNormalizeSortIndexForElementsOfDirectoryReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(payload, token);
    }
}