////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// GetCategoriesReceive
/// </summary>
public class GetCategoriesReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<object?, TResponseModel<List<CategoryRealBreezRuModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetCategoriesBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CategoryRealBreezRuModel>>?> ResponseHandleActionAsync(object? payload, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.GetCategoriesAsync( token);
    }
}