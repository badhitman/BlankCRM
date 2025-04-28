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
            boardDb = new BoardStockSharpModelDB().Bind(req);
            boardDb.CreatedAtUTC = DateTime.UtcNow;

            boardDb.ExchangeId = exchange.Id;
            boardDb.Exchange = null;

            context.Boards.Add(boardDb);
        }
        else
        {
            boardDb.SetUpdate(req);
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
            exchangeDb = new ExchangeStockSharpModelDB().Bind(req);
            context.Exchanges.Add(exchangeDb);
        }
        else
        {
            exchangeDb.SetUpdate(req);
            context.Exchanges.Update(exchangeDb);
        }
        context.SaveChanges();
        return exchangeDb.Id;
    }

    /// <inheritdoc/>
    public int SaveInstrument(InstrumentTradeStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        BoardStockSharpModelDB board = null;
        if (!string.IsNullOrWhiteSpace(req.Board.Code))
            board = context.Boards.First(x => x.Id == SaveBoard(req.Board));

        InstrumentStockSharpModelDB instrumentDb = context.Instruments
            .FirstOrDefault(x => x.Name == req.Name && x.Code == req.Code && x.BoardId == board.Id);

        if (instrumentDb is null)
        {
            instrumentDb = new InstrumentStockSharpModelDB().Bind(req);
            instrumentDb.CreatedAtUTC = DateTime.UtcNow;
            instrumentDb.BoardId = board.Id;
            instrumentDb.Board = null;

            context.Instruments.Add(instrumentDb);
        }
        else
        {
            instrumentDb.SetUpdate(req);

            instrumentDb.BoardId = board.Id;
            instrumentDb.Board = null;

            context.Instruments.Update(instrumentDb);
        }
        context.SaveChanges();
        return instrumentDb.Id;
    }

    /// <inheritdoc/>
    public int SavePortfolio(PortfolioStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        BoardStockSharpModelDB board = null;
        if (!string.IsNullOrWhiteSpace(req.Board.Code))
            board = context.Boards.First(x => x.Id == SaveBoard(req.Board));

        PortfolioTradeModelDB portDb = context.Portfolios
            .FirstOrDefault(x => x.Name == req.Name && x.DepoName == req.DepoName && x.Currency == req.Currency && x.BoardId == board.Id);

        if (portDb is null)
        {
            portDb = new PortfolioTradeModelDB().Bind(req);
            portDb.CreatedAtUTC = DateTime.UtcNow;
            portDb.BoardId = board.Id;
            portDb.Board = null;

            context.Portfolios.Add(portDb);
        }
        else
        {
            portDb.SetUpdate(req);

            portDb.BoardId = board.Id;
            portDb.Board = null;

            context.Portfolios.Update(portDb);
        }
        context.SaveChanges();
        return portDb.Id;
    }

    public int SaveOrder(OrderStockSharpModel req)
    {
        using StockSharpAppContext context = toolsDbFactory.CreateDbContext();
        OrderStockSharpModelDB orderDb = context.Orders.FirstOrDefault(x => x.TransactionId == req.TransactionId);

        InstrumentStockSharpModelDB instrumentDb = null;
        if (!string.IsNullOrWhiteSpace(req.Instrument.Name))
            instrumentDb = context.Instruments.First(x => x.Id == SaveInstrument(req.Instrument));

        PortfolioTradeModelDB portfolioDb = null;
        if (!string.IsNullOrWhiteSpace(req.Portfolio.Name))
            portfolioDb = context.Portfolios.First(x => x.Id == SavePortfolio(req.Portfolio));

        if (orderDb is null)
        {
            orderDb = new OrderStockSharpModelDB().Bind(req);
            orderDb.CreatedAtUTC = DateTime.UtcNow;
            orderDb.InstrumentId = instrumentDb.Id;
            orderDb.Instrument = null;

            orderDb.PortfolioId = portfolioDb.Id;
            orderDb.Portfolio = null;

            context.Orders.Add(orderDb);
        }
        else
        {
            orderDb.SetUpdate(req);

            orderDb.InstrumentId = instrumentDb.Id;
            orderDb.Instrument = null;

            orderDb.PortfolioId = portfolioDb.Id;
            orderDb.Portfolio = null;

            context.Orders.Update(orderDb);
        }
        context.SaveChanges();
        return orderDb.IdPK;
    }
}