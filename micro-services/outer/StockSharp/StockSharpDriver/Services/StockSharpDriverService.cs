////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace StockSharpDriver;

/// <summary>
/// StockSharpDriverService
/// </summary>
public class StockSharpDriverService(IDbContextFactory<StockSharpAppContext> toolsDbFactory) : IStockSharpDriverService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<BoardStockSharpModel>>> GetBoardsAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<BoardStockSharpModelDB> q = ids is null || ids.Length == 0
            ? context.Boards.AsQueryable()
            : context.Boards.Where(x => ids.Contains(x.Id));
        List<BoardStockSharpModelDB> data = await q.ToListAsync(cancellationToken: cancellationToken);

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
        List<ExchangeStockSharpModelDB> data = await q.ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<InstrumentTradeStockSharpModel>>> GetInstrumentsAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<InstrumentStockSharpModelDB> q = ids is null || ids.Length == 0
            ? context.Instruments.AsQueryable()
            : context.Instruments.Where(x => ids.Contains(x.Id));
        List<InstrumentStockSharpModelDB> data = await q.ToListAsync(cancellationToken: cancellationToken);

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
        List<OrderStockSharpModelDB> data = await q.ToListAsync(cancellationToken: cancellationToken);

        return new()
        {
            Response = [.. data]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PortfolioStockSharpModel>>> GetPortfoliosAsync(int[] ids = null, CancellationToken cancellationToken = default)
    {
        using StockSharpAppContext context = await toolsDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<PortfolioTradeModelDB> q = ids is null || ids.Length == 0
            ? context.Portfolios.AsQueryable()
            : context.Portfolios.Where(x => ids.Contains(x.Id));
        List<PortfolioTradeModelDB> data = await q.ToListAsync(cancellationToken: cancellationToken);

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