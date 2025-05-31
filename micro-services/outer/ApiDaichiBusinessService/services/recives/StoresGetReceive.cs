////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// StoresGetReceive
/// </summary>
public class StoresGetReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<object?, TResponseModel<StoresDaichiBusinessResponseModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.StoresGetDaichiReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<StoresDaichiBusinessResponseModel?>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await daichiRepo.StoresGetAsync(token);
    }
}