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
    /// <inheritdoc/>
    public void SaveInstrument(InstrumentTradeModel instrument)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void SavePortfolio(PortfolioTradeModel portfolio)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        throw new NotImplementedException();
    }
}