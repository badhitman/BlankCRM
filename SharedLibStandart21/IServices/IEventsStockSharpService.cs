////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// StockSharp - события
/// </summary>
public interface IEventsStockSharpService
{
    /// <summary>
    /// BoardReceived
    /// </summary>
    public Task<ResponseBaseModel> BoardReceived(BoardStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentReceived
    /// </summary>
    public Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// OrderReceived
    /// </summary>
    public Task<ResponseBaseModel> OrderReceived(OrderStockSharpModel req);

    /// <summary>
    /// PortfolioReceived
    /// </summary>
    public Task<ResponseBaseModel> PortfolioReceived(PortfolioStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Security changed.
    /// </summary>
    public Task<ResponseBaseModel> ValuesChangedEvent(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default);
}