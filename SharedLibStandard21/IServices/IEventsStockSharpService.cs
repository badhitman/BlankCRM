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
    /// ToastClientShow
    /// </summary>
    public Task<ResponseBaseModel> ToastClientShow(ToastShowClientModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// UpdateConnectionHandle
    /// </summary>
    public Task<ResponseBaseModel> UpdateConnectionHandle(UpdateConnectionHandleModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// TelegramBotStarting
    /// </summary>
    public Task<ResponseBaseModel> TelegramBotStarting(UserTelegramBaseModel bot, CancellationToken cancellationToken = default);

    /// <summary>
    /// BoardReceived
    /// </summary>
    public Task<ResponseBaseModel> BoardReceived(BoardStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentReceived
    /// </summary>
    public Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// OrderReceived
    /// </summary>
    public Task<ResponseBaseModel> OrderReceived(OrderStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// PortfolioReceived
    /// </summary>
    public Task<ResponseBaseModel> PortfolioReceived(PortfolioStockSharpViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// PositionReceived
    /// </summary>
    public Task<ResponseBaseModel> PositionReceived(PositionStockSharpModel position, CancellationToken cancellationToken = default);

    /// <summary>
    /// Security changed.
    /// </summary>
    public Task<ResponseBaseModel> ValuesChangedEvent(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default);
}