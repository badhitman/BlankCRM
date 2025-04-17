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
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DownloadAndSaveHaierProffReceive, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RabbitMqManagementResponseModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.HealthCheckHaierProffReceive, token: token) ?? new();
    
    /// <inheritdoc/>
    public async Task<TResponseModel<List<FeedItemHaierModel>>> ProductsFeedGetAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<FeedItemHaierModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProductsFeedGetHaierProffReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductHaierModelDB>> ProductsSelectAsync(HaierRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ProductHaierModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProductsSelectHaierProffReceive, req, token: token) ?? new();
}