////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// GetBrandsReceive
/// </summary>
public class GetBrandsReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<object?, TResponseModel<List<BrandRealBreezRuModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetBrandsBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BrandRealBreezRuModel>>?> ResponseHandleActionAsync(object? payload, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.GetBrandsAsync( token);
    }
}