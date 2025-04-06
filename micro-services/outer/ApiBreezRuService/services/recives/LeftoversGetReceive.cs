////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// LeftoversGetAsyncReceive
/// </summary>
public class LeftoversGetReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<string?, TResponseModel<List<BreezRuGoodsModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.LeftoversGetBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuGoodsModel>>?> ResponseHandleActionAsync(string? nc, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.LeftoversGetAsync(nc, token);
    }
}