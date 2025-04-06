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
    public async Task<CategoriesRusklimatResponseModel> GetCategoriesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<CategoriesRusklimatResponseModel>(GlobalStaticConstants.TransmissionQueues.GetCategoriesRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ProductsRusklimatResponseModel> GetProductsAsync(PaginationRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ProductsRusklimatResponseModel>(GlobalStaticConstants.TransmissionQueues.GetProductsRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<PropertiesRusklimatResponseModel> GetPropertiesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<PropertiesRusklimatResponseModel>(GlobalStaticConstants.TransmissionQueues.GetPropertiesRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<UnitsRusklimatResponseModel> GetUnitsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<UnitsRusklimatResponseModel>(GlobalStaticConstants.TransmissionQueues.GetUnitsRusklimatReceive, token: token) ?? new();
}