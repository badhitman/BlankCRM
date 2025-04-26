////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace StockSharpDriver;

/// <summary>
/// StockSharpDataService
/// </summary>
public class StockSharpDataService(IDbContextFactory<StockSharpAppContext> toolsDbFactory) : IStockSharpDataService
{
    public void SaveBoard(BoardStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void SaveInstrument(InstrumentTradeStockSharpModel instrument)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        throw new NotImplementedException();
    }

    public void SaveOrder(OrderStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void SavePortfolio(PortfolioStockSharpModel portfolio)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        throw new NotImplementedException();
    }
}