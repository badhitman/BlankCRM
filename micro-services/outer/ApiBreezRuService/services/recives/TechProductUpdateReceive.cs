////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// TechProductUpdateReceive
/// </summary>
public class TechProductUpdateReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<TechProductBreezRuModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.TechProductUpdateBreezReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TechProductBreezRuModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.TechProductUpdateAsync(req,token);
    }
}