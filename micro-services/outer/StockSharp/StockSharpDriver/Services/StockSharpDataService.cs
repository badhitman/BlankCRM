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
    public int SaveBoard(BoardStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        ExchangeStockSharpModelDB exchange = null;
        if (!string.IsNullOrWhiteSpace(req.Exchange.Name))
        {
            SaveExchange(req.Exchange);
            exchange = context.Exchanges.First(x => x.Name == req.Exchange.Name && x.CountryCode == req.Exchange.CountryCode);
        }
        bool withOutExchange = exchange is null;
        BoardStockSharpModelDB boardDb = context.Boards
            .FirstOrDefault(x => x.Code == req.Code && (withOutExchange || x.ExchangeId == exchange.Id));
        if (boardDb is null)
        {
            boardDb = (BoardStockSharpModelDB)req;
            context.Boards.Add(boardDb);
        }
        else
        {
            boardDb.SetUpdate((BoardStockSharpModelDB)req);
            context.Boards.Update(boardDb);
        }
        context.SaveChanges();
        return boardDb.Id;
    }

    /// <inheritdoc/>
    public int SaveExchange(ExchangeStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        ExchangeStockSharpModelDB exchangeDb = context.Exchanges
            .FirstOrDefault(x => x.Name == req.Name && x.CountryCode == req.CountryCode);
        if (exchangeDb is null)
        {
            exchangeDb = (ExchangeStockSharpModelDB)req;
            context.Exchanges.Add(exchangeDb);
        }
        else
        {
            exchangeDb.SetUpdate((ExchangeStockSharpModelDB)req);
            context.Exchanges.Update(exchangeDb);
        }
        context.SaveChanges();
        return exchangeDb.Id;
    }

    /// <inheritdoc/>
    public void SaveInstrument(InstrumentTradeStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();

        BoardStockSharpModelDB board = null;
        if (!string.IsNullOrWhiteSpace(req.Board.Code))
            board = context.Boards.First(x => x.Id == SaveBoard(req.Board));

        InstrumentStockSharpModelDB instrumentDb = context.Instruments
            .FirstOrDefault(x => x.Name == req.Name && x.Code == req.Code && x.BoardId == board.Id);

        if (instrumentDb is null)
        {
            instrumentDb = (InstrumentStockSharpModelDB)req;
            context.Instruments.Add(instrumentDb);
        }
        else
        {
            instrumentDb.SetUpdate((InstrumentStockSharpModelDB)req);
            context.Instruments.Update(instrumentDb);
        }
        context.SaveChanges();
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