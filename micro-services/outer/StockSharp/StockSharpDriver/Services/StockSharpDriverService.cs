////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using StockSharp.Algo;
using SharedLib;
using DbcLib;
using System.Threading;
using StockSharp.BusinessEntities;
using Ecng.Common;
using StockSharp.Messages;

namespace StockSharpDriver;

/// <summary>
/// StockSharpDriverService
/// </summary>
public class StockSharpDriverService(IDbContextFactory<StockSharpAppContext> toolsDbFactory, Connector connector) : IStockSharpDriverService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<InstrumentTradeStockSharpModel>>> GetInstrumentsAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<InstrumentStockSharpModelDB> q = ids is null || ids.Length == 0
            ? context.Instruments.AsQueryable()
            : context.Instruments.Where(x => ids.Contains(x.Id));
        List<InstrumentStockSharpModelDB> data = await q.Include(x => x.Board).ThenInclude(x => x.Exchange).ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BoardStockSharpModel>>> GetBoardsAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<BoardStockSharpModelDB> q = ids is null || ids.Length == 0
            ? context.Boards.AsQueryable()
            : context.Boards.Where(x => ids.Contains(x.Id));

        List<BoardStockSharpModelDB> data = await q
            .Include(x => x.Exchange)
            .ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ExchangeStockSharpModel>>> GetExchangesAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<ExchangeStockSharpModelDB> q = ids is null || ids.Length == 0
            ? context.Exchanges.AsQueryable()
            : context.Exchanges.Where(x => ids.Contains(x.Id));
        List<ExchangeStockSharpModelDB> data = await q.Include(x => x.Boards).ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<OrderStockSharpModel>>> GetOrdersAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<OrderStockSharpModelDB> q = ids is null || ids.Length == 0
            ? context.Orders.AsQueryable()
            : context.Orders.Where(x => ids.Contains(x.IdPK));
        List<OrderStockSharpModelDB> data = await q.Include(x => x.Instrument).Include(x => x.Portfolio).ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(TPaginationRequestStandardModel<InstrumentsRequestModel> req, CancellationToken cancellationToken = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<InstrumentStockSharpModelDB> q = context.Instruments.Where(x => req.Payload.FavoriteFilter == null || x.IsFavorite == req.Payload.FavoriteFilter).AsQueryable();

        List<InstrumentStockSharpModelDB> _data = await q
            .Include(x => x.Board)
            .ThenInclude(x => x.Exchange)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Board.Code)
            .Skip(req.PageSize * req.PageNum)
            .Take(req.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: cancellationToken),
            SortingDirection = req.SortingDirection,
            Response = [.. _data.Select(x => new InstrumentTradeStockSharpViewModel().Bind(x))]
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentFavoriteToggleAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        InstrumentStockSharpModelDB instrumentDb = await context.Instruments.FirstOrDefaultAsync(x => x.IdRemote == req.IdRemote, cancellationToken: cancellationToken);
        if (instrumentDb is null)
            return ResponseBaseModel.CreateError("Инструмент не найден");

        instrumentDb.IsFavorite = !instrumentDb.IsFavorite;
        context.Update(instrumentDb);
        await context.SaveChangesAsync(cancellationToken: cancellationToken);

        return ResponseBaseModel.CreateSuccess("Запрос выполнен");
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default)
    {
        ExchangeBoard board = req.Instrument.Board is null
            ? null
            : connector.ExchangeBoards.FirstOrDefault(x => x.Code == req.Instrument.Board.Code && (x.Exchange.Name == req.Instrument.Board.Exchange.Name || x.Exchange.CountryCode.ToString() == req.Instrument.Board.Exchange.CountryCode.ToString()));
        Security currentSec = connector.Securities.FirstOrDefault(x => x.Name == req.Instrument.Name && x.Code == req.Instrument.Code && x.Board.Code == board.Code && x.Board.Exchange.Name == board.Exchange.Name && x.Board.Exchange.CountryCode == board.Exchange.CountryCode);
        if (currentSec is null)
            return Task.FromResult(ResponseBaseModel.CreateError($"Инструмент не найден: {req.Instrument}"));

        Portfolio selectedPortfolio = connector.Portfolios.FirstOrDefault(x => x.ClientCode == req.Portfolio.ClientCode && x.Name == req.Portfolio.Name);
        if (selectedPortfolio is null)
            return Task.FromResult(ResponseBaseModel.CreateError($"Портфель не найден: {req.Portfolio}"));

        Order order = new()
        {
            // устанавливается тип заявки, в данном примере лимитный
            Type = (OrderTypes)Enum.Parse(typeof(OrderTypes), Enum.GetName(req.OrderType)),
            // устанавливается портфель для исполнения заявки
            Portfolio = selectedPortfolio,
            // устанавливается объём заявки
            Volume = req.Volume,
            // устанавливается цена заявки
            Price = req.Price,
            // устанавливается инструмент
            Security = currentSec,
            // устанавливается направление заявки, в данном примере покупка
            Side = (Sides)Enum.Parse(typeof(Sides), Enum.GetName(req.Side))
        };

        connector.RegisterOrder(order);
        return Task.FromResult(ResponseBaseModel.CreateInfo("Заявка отправлена на регистрацию"));
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PortfolioStockSharpModel>>> GetPortfoliosAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<PortfolioTradeModelDB> q = ids is null || ids.Length == 0
            ? context.Portfolios.AsQueryable()
            : context.Portfolios.Where(x => ids.Contains(x.Id));
        List<PortfolioTradeModelDB> data = await q.Include(x => x.Board).ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
    {
        //StockSharp.Algo.Connector Connector = new();
        return Task.FromResult(ResponseBaseModel.CreateSuccess($"Ok - {nameof(StockSharpDriverService)}"));
    }
}