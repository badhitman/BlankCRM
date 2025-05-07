////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// StockSharpDriverTransmission
/// </summary>
public partial class StockSharpDriverTransmission(IMQTTClient mqClient) : IDriverStockSharpService
{
    /// <inheritdoc/>
    public async Task<AboutConnectResponseModel> AboutConnect(CancellationToken? cancellationToken = null)
        => await mqClient.MqRemoteCallAsync<AboutConnectResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AboutConnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Connect(CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Disconnect(CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DisconnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderRegisterStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpDriverReceive, token: cancellationToken) ?? new();
}