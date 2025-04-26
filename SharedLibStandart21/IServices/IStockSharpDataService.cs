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
    /// Сохранить в БД инструмент
    /// </summary>
    public void SaveInstrument(InstrumentTradeModel instrument);

    /// <summary>
    /// SavePortfolio
    /// </summary>
    public void SavePortfolio(PortfolioTradeModel portfolio);
}