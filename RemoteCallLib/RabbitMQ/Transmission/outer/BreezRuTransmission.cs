////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// BreezRuTransmission
/// </summary>
public class BreezRuTransmission(IMQClientRPC rabbitClient) : IBreezRuApiTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<BrandRealBreezRuModel>>> GetBrandsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BrandRealBreezRuModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBrandsBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CategoryRealBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<CategoryRealBreezRuModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetCategoriesBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductRealBreezRuModel>>> GetProductsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<ProductRealBreezRuModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetProductsBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryRealBreezRuModel>>> GetTechCategoryAsync(TechRequestBreezModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<TechCategoryRealBreezRuModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTechCategoryBreezReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechProductRealBreezRuModel>>> GetTechProductAsync(TechRequestBreezModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<TechProductRealBreezRuModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTechProductBreezReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DownloadAndSaveBreezReceive, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RabbitMqManagementResponseModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.HealthCheckBreezReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuLeftoverModel>?>> LeftoversGetAsync(string? nc = null, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<BreezRuLeftoverModel>?>>(GlobalStaticConstantsTransmission.TransmissionQueues.LeftoversGetBreezReceive, nc, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TechProductUpdateAsync(TechProductBreezRuModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TechProductUpdateBreezReceive, req, false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ProductUpdateAsync(ProductBreezRuModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ProductUpdateBreezReceive, req, false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ProductViewBreezRuModeld>> ProductsSelectAsync(BreezRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ProductViewBreezRuModeld>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProductsSelectBreezReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CategoryUpdateAsync(CategoryBreezRuModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CategoryUpdateBreezReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TechCategoryUpdateAsync(TechCategoryBreezRuModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TechCategoryUpdateBreezReceive, req, token: token) ?? new();
}