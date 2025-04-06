////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// GetProperties
/// </summary>
public class GetPropertiesReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<object?, PropertiesRusklimatResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetPropertiesRusklimatReceive;

    /// <inheritdoc/>
    public async Task<PropertiesRusklimatResponseModel?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.GetPropertiesAsync(token);
    }
}
