////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// StockSharp - events initiator
/// </summary>
public interface IEventsStockSharp : IEventsNotify
{
    /// <summary>
    /// Update Connection - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> UpdateConnectionHandle(UpdateConnectionHandleModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// TelegramBot starting - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> TelegramBotStarting(UserTelegramBaseModel bot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Board actuality - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> BoardReceived(BoardStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Instrument actuality - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Order actuality - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> OrderReceived(OrderStockSharpModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Portfolio actuality - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> PortfolioReceived(PortfolioStockSharpViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Position actuality - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> PositionReceived(PositionStockSharpModel position, CancellationToken cancellationToken = default);

    /// <summary>
    /// Security changed - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> ValuesChangedEvent(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// DashboardTrade actuality - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> DashboardTradeUpdate(DashboardTradeStockSharpModel req, CancellationToken cancellationToken = default);
}