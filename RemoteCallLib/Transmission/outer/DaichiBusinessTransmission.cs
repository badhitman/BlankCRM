////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// DaichiBusinessTransmission
/// </summary>
public class DaichiBusinessTransmission(IRabbitClient rabbitClient) : IDaichiBusinessApiService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownloadAndSaveDaichiReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsDaichiBusinessResultModel>> ProductsGetAsync(ProductsRequestDaichiModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProductsDaichiBusinessResultModel>>(GlobalStaticConstants.TransmissionQueues.ProductsGetDaichiReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel>> ProductsParamsGetAsync(ProductParamsRequestDaichiModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProductsParamsDaichiBusinessResponseModel>>(GlobalStaticConstants.TransmissionQueues.ProductsParamsGetDaichiReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<StoresDaichiBusinessResponseModel>> StoresGetAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<StoresDaichiBusinessResponseModel>>(GlobalStaticConstants.TransmissionQueues.StoresGetDaichiReceive, token: token) ?? new();
}