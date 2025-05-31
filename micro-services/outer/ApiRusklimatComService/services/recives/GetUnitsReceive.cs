////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// GetUnits
/// </summary>
public class GetUnitsReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<object?, TResponseModel<UnitsRusklimatResponseModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetUnitsRusklimatReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<UnitsRusklimatResponseModel?>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.GetUnitsAsync(token);
    }
}
