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
    public async Task<TResponseModel<List<BrandRealBreezRuModel>>> GetBrandsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BrandRealBreezRuModel>>>(GlobalStaticConstants.TransmissionQueues.GetBrandsBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CategoryRealBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<CategoryRealBreezRuModel>>>(GlobalStaticConstants.TransmissionQueues.GetCategoriesBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductRealBreezRuModel>>> GetProductsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<ProductRealBreezRuModel>>>(GlobalStaticConstants.TransmissionQueues.GetProductsBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryRealBreezRuModel>>> GetTechCategoryAsync(TechRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<TechCategoryRealBreezRuModel>>>(GlobalStaticConstants.TransmissionQueues.GetTechCategoryBreezReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechProductRealBreezRuModel>>> GetTechProductAsync(TechRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<TechProductRealBreezRuModel>>>(GlobalStaticConstants.TransmissionQueues.GetTechProductBreezReceive, req, token: token) ?? new();

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