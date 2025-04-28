////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using StockSharp.Algo;
using SharedLib;
using DbcLib;
using System.Threading;

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