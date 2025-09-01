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
    #region CashFlow
    /// <summary>
    /// CashFlow Update
    /// </summary>
    public Task<ResponseBaseModel> CashFlowUpdateAsync(CashFlowViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get CashFlowList for instrument
    /// </summary>
    public Task<TResponseModel<List<CashFlowViewModel>>> CashFlowList(int instrumentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete CashFlow
    /// </summary>
    public Task<ResponseBaseModel> CashFlowDelete(int cashFlowId, CancellationToken cancellationToken = default);
    #endregion

    #region Instrument
    /// <summary>
    /// UpdateInstrumentAsync
    /// </summary>
    public Task<ResponseBaseModel> UpdateInstrumentAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// SetMarkersForInstrumentAsync
    /// </summary>
    public Task<ResponseBaseModel> SetMarkersForInstrumentAsync(SetMarkersForInstrumentRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// GetMarkersForInstrumentAsync
    /// </summary>
    public Task<TResponseModel<List<MarkerInstrumentStockSharpViewModel>>> GetMarkersForInstrumentAsync(int instrumentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить инструменты по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> GetInstrumentsAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentsSelectAsync
    /// </summary>
    public Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(InstrumentsRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read instruments for dashboard ready
    /// </summary>
    public Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> ReadTradeInstrumentsAsync(CancellationToken cancellationToken = default);
    #endregion

    #region rubrics/instruments
    /// <summary>
    /// RubricsInstrumentUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> RubricsInstrumentUpdateAsync(RubricsInstrumentUpdateModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// InstrumentRubricUpdateAsync
    /// </summary>
    public Task<ResponseBaseModel> InstrumentRubricUpdateAsync(InstrumentRubricUpdateModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// GetRubricsForInstrumentAsync
    /// </summary>
    public Task<TResponseModel<List<UniversalBaseModel>>> GetRubricsForInstrumentAsync(int idInstrument, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить инструменты для рубрики
    /// </summary>
    public Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> GetInstrumentsForRubricAsync(int idRubric, CancellationToken cancellationToken = default);
    #endregion

    /// <summary>
    /// Get portfolios by id`s
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<PortfolioStockSharpViewModel>>> GetPortfoliosAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить площадки бирж по их идентификаторам
    /// </summary>
    /// <returns>Если идентификаторы не установлены, тогда возвращаются все</returns>
    public Task<TResponseModel<List<BoardStockSharpViewModel>>> GetBoardsAsync(int[]? ids = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Поиск Board`s, подходящие под запрос
    /// </summary>
    public Task<TResponseModel<List<BoardStockSharpViewModel>>> FindBoardsAsync(BoardStockSharpModel req, CancellationToken cancellationToken = default);

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
}