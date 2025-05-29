////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// IFlushStockSharpService
/// </summary>
public interface IFlushStockSharpService
{
    /// <summary>
    /// Сохранить в БД инструмент
    /// </summary>
    public Task<TResponseModel<InstrumentTradeStockSharpViewModel>> SaveInstrument(InstrumentTradeStockSharpModel instrument);

    /// <summary>
    /// Save Portfolio
    /// </summary>
    public Task<TResponseModel<PortfolioStockSharpViewModel>> SavePortfolio(PortfolioStockSharpModel portfolio);

    /// <summary>
    /// SaveOrder
    /// </summary>
    public Task<TResponseModel<OrderStockSharpViewModel>> SaveOrder(OrderStockSharpModel req);

    /// <summary>
    /// SaveTrade
    /// </summary>
    public Task<TResponseModel<MyTradeStockSharpViewModel>> SaveTrade(MyTradeStockSharpModel myTrade);

    /// <summary>
    /// SaveExchange
    /// </summary>
    public Task<TResponseModel<int>> SaveExchange(ExchangeStockSharpModel req);

    /// <summary>
    /// Save Board
    /// </summary>
    public Task<TResponseModel<int>> SaveBoard(BoardStockSharpModel req);
}