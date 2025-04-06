////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// HaierProffRuTransmission
/// </summary>
public class HaierProffRuTransmission(IRabbitClient rabbitClient) : IFeedsHaierProffRuService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownloadAndSaveHaierProffReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<FeedItemHaierModel>>> ProductsFeedGetAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<FeedItemHaierModel>>>(GlobalStaticConstants.TransmissionQueues.ProductsFeedGetHaierProffReceive, token: token) ?? new();
}