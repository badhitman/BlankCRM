////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace RemoteCallLib;

/// <summary>
/// RusklimatComTransmission
/// </summary>
public class RusklimatComTransmission(IMQClientRPC rabbitClient) : IRusklimatComApiTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DownloadAndSaveRusklimatReceive, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<CategoriesRusklimatResponseModel?>> GetCategoriesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<CategoriesRusklimatResponseModel?>>(TransmissionQueues.GetCategoriesRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsRusklimatResponseModel?>> GetProductsAsync(RusklimatPaginationRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProductsRusklimatResponseModel?>>(TransmissionQueues.GetProductsRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PropertiesRusklimatResponseModel?>> GetPropertiesAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<PropertiesRusklimatResponseModel?>>(TransmissionQueues.GetPropertiesRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UnitsRusklimatResponseModel?>> GetUnitsAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UnitsRusklimatResponseModel?>>(TransmissionQueues.GetUnitsRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RabbitMqManagementResponseModel>>>(TransmissionQueues.HealthCheckRusklimatReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ProductRusklimatModelDB>> ProductsSelectAsync(RusklimatRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ProductRusklimatModelDB>>(TransmissionQueues.ProductsSelectRusklimatReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProductAsync(ProductRusklimatModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.ProductUpdateRusklimatReceive, req, false, token: token) ?? new();
}