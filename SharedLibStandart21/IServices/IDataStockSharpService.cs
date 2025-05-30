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
public interface IDataStockSharpService
{
    /// <summary>
    /// GetMarkersForInstrumentAsync
    /// </summary>
    public Task<TResponseModel<List<MarkerInstrumentStockSharpViewModel>>> GetMarkersForInstrumentAsync(int InstrumentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить инструменты по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> GetInstrumentsAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить профили по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<PortfolioStockSharpViewModel>>> GetPortfoliosAsync(int[]? ids = null, CancellationToken cancellationToken = default);

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
    public Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(InstrumentsRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentSetFavoriteAsync
    /// </summary>
    public Task<ResponseBaseModel> InstrumentFavoriteToggleAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default);    
}