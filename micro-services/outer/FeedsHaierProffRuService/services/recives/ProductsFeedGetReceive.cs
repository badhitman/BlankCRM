////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.HaierProff;

/// <summary>
/// ProductsFeedGet
/// </summary>
public class ProductsFeedGetReceive(IFeedsHaierProffRuService haierRepo)
    : IResponseReceive<object?, TResponseModel<List<FeedItemHaierModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductsFeedGetHaierProffReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<FeedItemHaierModel>>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await haierRepo.ProductsFeedGetAsync(token);
    }
}