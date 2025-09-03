////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Driver StockSharp (NET6)
/// </summary>
public interface IDriverStockSharpService
{
    /// <summary>
    /// About Connection
    /// </summary>
    public Task<AboutConnectResponseModel> AboutConnection(AboutConnectionRequestModel? req = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Connect
    /// </summary>
    public Task<ResponseBaseModel> Connect(ConnectRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnect
    /// </summary>
    public Task<ResponseBaseModel> Disconnect(CancellationToken cancellationToken = default);

    /// <summary>
    /// Forcefully destroy a connection and create a new one in its place
    /// </summary>
    /// <remarks>
    /// May be required in rare cases when the connection freezes
    /// </remarks>
    public Task<ResponseBaseModel> Terminate(CancellationToken cancellationToken = default);


    /// <summary>
    /// Order register
    /// </summary>
    public Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initial Load (Curve)
    /// </summary>
    public Task<ResponseSimpleModel> InitialLoad(InitialLoadRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Start strategy
    /// </summary>
    public Task<ResponseBaseModel> StartStrategy(StrategyStartRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reset strategy (individual)
    /// </summary>
    public Task<ResponseBaseModel> ResetStrategy(ResetStrategyRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reset strategies (All)
    /// </summary>
    public Task<ResponseBaseModel> ResetAllStrategies(ResetStrategyRequestBaseModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stop strategy
    /// </summary>
    public Task<ResponseBaseModel> StopStrategy(StrategyStopRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shift <c>SBond.ModelPrice</c> <c>Curve.BondList</c>
    /// </summary>
    /// <remarks>
    /// <code>
    /// SBond? SBnd = SBondList.FirstOrDefault(s => s.UnderlyingSecurity.Code == bnd.MicexCode);
    /// if (SBnd is not null)
    /// {
    ///     decimal yield = SBnd.GetYieldForPrice(CurveCurrent.CurveDate, bnd.ModelPrice / 100);
    ///     if (yield > 0)
    ///         bnd.ModelPrice = Math.Round(100 * SBnd.GetPriceFromYield(CurveCurrent.CurveDate, yield + req.YieldChange / 10000, true), 2);
    /// }
    /// </code>
    /// </remarks>
    public Task<ResponseBaseModel> ShiftCurve(ShiftCurveRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Group adjustment of limits for trade strategy <code>LowLimit</code> <code>HighLimit</code>
    /// </summary>
    public Task<ResponseBaseModel> LimitsStrategiesUpdate(LimitsStrategiesUpdateRequestModel req, CancellationToken cancellationToken = default);
}