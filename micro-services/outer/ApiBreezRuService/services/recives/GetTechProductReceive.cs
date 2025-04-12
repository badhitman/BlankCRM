////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// GetTechProductReceive
/// </summary>
public class GetTechProductReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<TechRequestBreezModel?, TResponseModel<List<TechProductRealBreezRuModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetTechProductBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechProductRealBreezRuModel>>?> ResponseHandleActionAsync(TechRequestBreezModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.GetTechProductAsync(req, token);
    }
}