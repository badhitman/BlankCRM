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
    : IResponseReceive<object?, TResponseModel<PropertiesRusklimatResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetPropertiesRusklimatReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PropertiesRusklimatResponseModel>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.GetPropertiesAsync(token);
    }
}
