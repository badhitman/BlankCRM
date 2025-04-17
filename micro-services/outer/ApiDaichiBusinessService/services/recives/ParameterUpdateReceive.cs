////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// ParameterUpdate
/// </summary>
public class ParameterUpdateReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<ParameterEntryDaichiModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ParameterUpdateDaichiReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ParameterEntryDaichiModelDB? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await daichiRepo.ParameterUpdateAsync(req, token);
    }
}