////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// ManageStockSharpTransmission
/// </summary>
public partial class ManageStockSharpTransmission(IMQTTClient mqClient) : IManageStockSharpService
{
    /// <inheritdoc/>
    public async Task<AboutDatabasesResponseModel> AboutDatabases(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<AboutDatabasesResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AboutDatabasesStockSharpReceive, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FixMessageAdapterModelDB[]>> AdaptersGetAsync(int[] req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<FixMessageAdapterModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.AdaptersGetStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<FixMessageAdapterModelDB>> AdaptersSelectAsync(TPaginationRequestStandardModel<AdaptersRequestModel> req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<FixMessageAdapterModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.AdaptersSelectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteAdapterAsync(int adapterId, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteAdapterStockSharpReceive, adapterId, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GenerateRegularCashFlowsAsync(CashFlowStockSharpRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.GenerateRegularCashFlowsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderStockSharpViewModel>> OrdersSelectAsync(TPaginationRequestStandardModel<OrdersSelectStockSharpRequestModel> req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<OrderStockSharpViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersSelectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FixMessageAdapterModelDB>> UpdateOrCreateAdapterAsync(FixMessageAdapterModelDB req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<FixMessageAdapterModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateAdapterStockSharpReceive, req, token: cancellationToken) ?? new();
}