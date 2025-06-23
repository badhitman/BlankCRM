////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Драйвер StockSharp (NET6)
/// </summary>
public interface IDriverStockSharpService : IStockSharpBaseService
{
    /// <summary>
    /// About Connection
    /// </summary>
    public Task<AboutConnectResponseModel> AboutConnection(CancellationToken? cancellationToken = default);

    /// <summary>
    /// Connect
    /// </summary>
    public Task<ResponseBaseModel> Connect(ConnectRequestModel req, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Disconnect
    /// </summary>
    public Task<ResponseBaseModel> Disconnect(CancellationToken? cancellationToken = default);

    /// <summary>
    /// Terminate
    /// </summary>
    public Task<ResponseBaseModel> Terminate(CancellationToken? cancellationToken = default);


    /// <summary>
    /// Order Register
    /// </summary>
    public Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Strategy Start
    /// </summary>
    public Task<ResponseBaseModel> StartStrategy(StrategyStartRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Strategy Stop
    /// </summary>
    public Task<ResponseBaseModel> StopStrategy(StrategyStopRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// ShiftCurve
    /// </summary>
    public Task<ResponseBaseModel> ShiftCurve(ShiftCurveRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// LimitsStrategiesUpdate
    /// </summary>
    public Task<ResponseBaseModel> LimitsStrategiesUpdate(LimitsStrategiesUpdateRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// OrderRegisterRequest
    /// </summary>
    public Task<OrderRegisterRequestResponseModel> OrderRegisterRequestAsync(OrderRegisterRequestModel req, CancellationToken cancellationToken = default);
}