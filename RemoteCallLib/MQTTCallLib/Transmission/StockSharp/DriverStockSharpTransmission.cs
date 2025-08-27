////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// DriverStockSharpTransmission
/// </summary>
public partial class DriverStockSharpTransmission(IMQTTClient mqClient) : IDriverStockSharpService
{
    /// <inheritdoc/>
    public async Task<AboutConnectResponseModel> AboutConnection(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<AboutConnectResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AboutConnectStockSharpReceive, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Connect(ConnectRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Disconnect(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DisconnectStockSharpReceive, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Terminate(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TerminateStockSharpReceive, token: cancellationToken) ?? new();


    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderRegisterStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartStrategy(StrategyStartRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.StrategyStartStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopStrategy(StrategyStopRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.StrategyStopStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetStrategy(ResetStrategyRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ResetStrategyStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ShiftCurve(ShiftCurveRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ShiftCurveStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> LimitsStrategiesUpdate(LimitsStrategiesUpdateRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.LimitsStrategiesUpdateStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseSimpleModel> InitialLoad(InitialLoadRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseSimpleModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InitialLoadStockSharpDriverReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResetAllStrategies(ResetStrategyRequestBaseModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ResetAllStrategiesStockSharpDriverReceive, req, token: cancellationToken) ?? new();
}