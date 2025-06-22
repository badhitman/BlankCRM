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
    public async Task<AboutConnectResponseModel> AboutConnection(CancellationToken? cancellationToken = null)
        => await mqClient.MqRemoteCallAsync<AboutConnectResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AboutConnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Connect(ConnectRequestModel req, CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectStockSharpReceive, req, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Disconnect(CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DisconnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Terminate(CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TerminateStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();


    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderRegisterStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<OrderRegisterRequestResponseModel> OrderRegisterRequestAsync(OrderRegisterRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<OrderRegisterRequestResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderRegisterRequestStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpDriverReceive, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartStrategy(StrategyStartRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.StrategyStartStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopStrategy(StrategyStopRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.StrategyStopStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ShiftCurve(ShiftCurveRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ShiftCurveStockSharpDriverReceive, req, token: cancellationToken) ?? new();
}