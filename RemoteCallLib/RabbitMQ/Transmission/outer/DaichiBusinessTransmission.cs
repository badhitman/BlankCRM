////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace RemoteCallLib;

/// <summary>
/// DaichiBusinessTransmission
/// </summary>
public class DaichiBusinessTransmission(IRabbitClient rabbitClient) : IDaichiBusinessApiTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DownloadAndSaveDaichiReceive, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RabbitMqManagementResponseModel>>>(TransmissionQueues.HealthCheckDaichiReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ParameterUpdateAsync(ParameterEntryDaichiModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.ParameterUpdateDaichiReceive, req, false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ProductUpdateAsync(ProductDaichiModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.ProductUpdateDaichiReceive, req, false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsDaichiBusinessResultModel?>> ProductsGetAsync(ProductsRequestDaichiModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProductsDaichiBusinessResultModel?>>(TransmissionQueues.ProductsGetDaichiReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel?>> ProductsParamsGetAsync(ProductParamsRequestDaichiModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProductsParamsDaichiBusinessResponseModel?>>(TransmissionQueues.ProductsParamsGetDaichiReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<StoresDaichiBusinessResponseModel?>> StoresGetAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<StoresDaichiBusinessResponseModel?>>(TransmissionQueues.StoresGetDaichiReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductDaichiModelDB>> ProductsSelectAsync(DaichiRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ProductDaichiModelDB>>(TransmissionQueues.ProductsSelectDaichiReceive, req, token: token) ?? new();
}