////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// GetProductsReceive
/// </summary>
public class GetProductsReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<object?, TResponseModel<List<ProductBreezRuModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetProductsBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductBreezRuModel>>?> ResponseHandleActionAsync(object? payload, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.GetProductsAsync(token);
    }
}