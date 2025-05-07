////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// StockSharp - служба доступа к данным БД
/// </summary>
public interface IStockSharpDataService
{
    /// <summary>
    /// SaveExchange
    /// </summary>
    public int SaveExchange(ExchangeStockSharpModel req);

    /// <summary>
    /// Save Board
    /// </summary>
    public int SaveBoard(BoardStockSharpModel req);

    /// <summary>
    /// Сохранить в БД инструмент
    /// </summary>
    public int SaveInstrument(InstrumentTradeStockSharpModel instrument);

    /// <summary>
    /// Save Portfolio
    /// </summary>
    public int SavePortfolio(PortfolioStockSharpModel portfolio);

    /// <summary>
    /// SaveOrder
    /// </summary>
    public int SaveOrder(OrderStockSharpModel req);

    /// <summary>
    /// Получить инструменты по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<InstrumentTradeStockSharpModel>>> GetInstrumentsAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить профили по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<PortfolioStockSharpModel>>> GetPortfoliosAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить площадки бирж по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<BoardStockSharpModel>>> GetBoardsAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить биржи по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<ExchangeStockSharpModel>>> GetExchangesAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить заказы по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<OrderStockSharpModel>>> GetOrdersAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentsSelectAsync
    /// </summary>
    public Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(TPaginationRequestStandardModel<InstrumentsRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentSetFavoriteAsync
    /// </summary>
    public Task<ResponseBaseModel> InstrumentFavoriteToggleAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// OrderRegister
    /// </summary>
    public Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default);
}