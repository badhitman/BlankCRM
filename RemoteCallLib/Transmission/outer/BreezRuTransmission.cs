////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// BreezRuTransmission
/// </summary>
public class BreezRuTransmission(IRabbitClient rabbitClient) : IBreezRuApiService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownloadAndSaveBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuGoodsModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BreezRuGoodsModel>>>(GlobalStaticConstants.TransmissionQueues.LeftoversGetBreezReceive, nc, token: token) ?? new();
}