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
    /// Save Board
    /// </summary>
    public void SaveBoard(BoardStockSharpModel req);

    /// <summary>
    /// Сохранить в БД инструмент
    /// </summary>
    public void SaveInstrument(InstrumentTradeStockSharpModel instrument);

    /// <summary>
    /// SaveOrder
    /// </summary>
    public void SaveOrder(OrderStockSharpModel req);

    /// <summary>
    /// Save Portfolio
    /// </summary>
    public void SavePortfolio(PortfolioStockSharpModel portfolio);
}