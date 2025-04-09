////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// RusklimatComTransmission
/// </summary>
public class RusklimatComTransmission(IRabbitClient rabbitClient) : IRusklimatComApiService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownloadAndSaveRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<CategoriesRusklimatResponseModel>> GetCategoriesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<CategoriesRusklimatResponseModel>>(GlobalStaticConstants.TransmissionQueues.GetCategoriesRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsRusklimatResponseModel>> GetProductsAsync(RusklimatPaginationRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProductsRusklimatResponseModel>>(GlobalStaticConstants.TransmissionQueues.GetProductsRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PropertiesRusklimatResponseModel>> GetPropertiesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<PropertiesRusklimatResponseModel>>(GlobalStaticConstants.TransmissionQueues.GetPropertiesRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UnitsRusklimatResponseModel>> GetUnitsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UnitsRusklimatResponseModel>>(GlobalStaticConstants.TransmissionQueues.GetUnitsRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RabbitMqManagementResponseModel>>>(GlobalStaticConstants.TransmissionQueues.HealthCheckRusklimatReceive, token: token) ?? new();
}