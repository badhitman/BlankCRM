////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

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
}