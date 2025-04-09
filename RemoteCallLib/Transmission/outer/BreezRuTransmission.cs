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
    public async Task<TResponseModel<List<BrandBreezRuModel>>> GetBrandsAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CategoryBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductBreezRuModel>>> GetProductsAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryBreezRuModel>>> GetTechCategoryAsync(TechRequestModel req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechProductBreezRuResponseModel>>> GetTechProductAsync(TechRequestModel req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownloadAndSaveBreezReceive, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RabbitMqManagementResponseModel>>>(GlobalStaticConstants.TransmissionQueues.HealthCheckBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuLeftoverModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BreezRuLeftoverModel>>>(GlobalStaticConstants.TransmissionQueues.LeftoversGetBreezReceive, nc, token: token) ?? new();
}