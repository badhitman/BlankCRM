////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace StorageService;

/// <summary>
/// LogsNavigationImpl
/// </summary>
public class LogsNavigationImpl(IDbContextFactory<NLogsContext> logsDbFactory) : ILogsService
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRowAsync(TPaginationRequestStandardModel<int> req, CancellationToken token = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync(token);
        IQueryable<NLogRecordModelDB> q = ctx.Logs.AsQueryable();

        TPaginationResponseModel<NLogRecordModelDB> res = new()
        {
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
        };

        if (!await q.AnyAsync(x => x.Id == req.Payload, cancellationToken: token))
            return res;

        IOrderedQueryable<NLogRecordModelDB> oq = req.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.RecordTime)
          : q.OrderByDescending(x => x.RecordTime);

        res.PageNum = (await oq.CountAsync(cancellationToken: token)) / req.PageSize;

        if (!await oq.Skip(res.PageNum * req.PageSize).Take(req.PageSize).AnyAsync(x => x.Id == req.Payload, cancellationToken: token))
            res.PageNum++;

        res.Response = [.. await oq.Skip(res.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token)];

        if (!res.Response.Any(x => x.Id == req.Payload))
            return await GoToPageForRowAsync(req, token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
    {
        Dictionary<string, int> LevelsAvailable = [];
        Dictionary<string, int> ApplicationsAvailable = [];
        Dictionary<string, int> ContextsPrefixesAvailable = [];
        Dictionary<string, int> LoggersAvailable = [];

        DateTime? minDate = null;
        DateTime? maxDate = null;

        IQueryable<NLogRecordModelDB> QuerySet(IQueryable<NLogRecordModelDB> q)
        {
            if (req.StartAt.HasValue)
            {
                DateTime _dt = req.StartAt.Value.SetKindUtc();
                q = q.Where(x => x.RecordTime >= _dt);
            }
            if (req.FinalOff.HasValue)
            {
                DateTime _dt = req.FinalOff.Value.SetKindUtc().Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                q = q.Where(x => x.RecordTime <= _dt);
            }

            return q;
        }

        await Task.WhenAll([
                Task.Run(async () => {
                    using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync();
                    minDate = await ctx.Logs.MinAsync(x => x.RecordTime);
                }, token),
                Task.Run(async () => {
                    using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync();
                    maxDate = await ctx.Logs.MaxAsync(x => x.RecordTime);
                }, token),
                Task.Run(async () => {
                    using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync();
                    (await QuerySet(ctx.Logs.AsQueryable()).GroupBy(x => x.RecordLevel).Select(x => new KeyValuePair<string, int>(x.Key, x.Count())).ToListAsync())
                        .ForEach(x => LevelsAvailable.Add(x.Key, x.Value));
                }, token),
                Task.Run(async () => {
                    using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync();
                    (await QuerySet(ctx.Logs.AsQueryable()).GroupBy(x => x.ApplicationName).Select(x => new KeyValuePair<string, int>(x.Key, x.Count())).ToListAsync())
                        .ForEach(x => ApplicationsAvailable.Add(x.Key, x.Value));
                }, token),
                Task.Run(async () => {
                    using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync();
                    (await QuerySet(ctx.Logs.AsQueryable()).GroupBy(x => x.ContextPrefix).Select(x => new KeyValuePair<string, int>(x.Key, x.Count())).ToListAsync())
                        .ForEach(x => ContextsPrefixesAvailable.Add(x.Key , x.Value));
                }, token),
                Task.Run(async () => {
                    using NLogsContext ctx = await logsDbFactory.CreateDbContextAsync();
                    (await QuerySet(ctx.Logs.AsQueryable()).GroupBy(x => x.Logger).Select(x => new KeyValuePair<string, int>(x.Key, x.Count())).ToListAsync())
                        .ForEach(x => LoggersAvailable.Add(x.Key , x.Value));
                }, token),
            ]);

        return new()
        {
            Response = new()
            {
                ContextsPrefixesAvailable = ContextsPrefixesAvailable,
                ApplicationsAvailable = ApplicationsAvailable,
                LoggersAvailable = LoggersAvailable,
                LevelsAvailable = LevelsAvailable,
                StartAt = minDate,
                FinalOff = maxDate,
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using NLogsContext context = await logsDbFactory.CreateDbContextAsync(token);
        IQueryable<NLogRecordModelDB> q = context.Logs.AsQueryable();

        if (req.Payload is not null && req.Payload.StartAt.HasValue)
        {
            DateTime _dt = req.Payload.StartAt.Value;
            q = q.Where(x => x.RecordTime >= _dt);
        }
        if (req.Payload is not null && req.Payload.FinalOff.HasValue)
        {
            DateTime _dt = req.Payload.FinalOff.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            q = q.Where(x => x.RecordTime <= _dt);
        }

        if (req.Payload is not null && req.Payload.LevelsFilter is not null && req.Payload.LevelsFilter.Length != 0)
            q = q.Where(x => req.Payload.LevelsFilter.Contains(x.RecordLevel));

        if (req.Payload is not null && req.Payload.LoggersFilter is not null && req.Payload.LoggersFilter.Length != 0)
            q = q.Where(x => req.Payload.LoggersFilter.Contains(x.Logger));

        if (req.Payload is not null && req.Payload.ContextsPrefixesFilter is not null && req.Payload.ContextsPrefixesFilter.Length != 0)
            q = q.Where(x => req.Payload.ContextsPrefixesFilter.Contains(x.ContextPrefix));

        if (req.Payload is not null && req.Payload.ApplicationsFilter is not null && req.Payload.ApplicationsFilter.Length != 0)
            q = q.Where(x => req.Payload.ApplicationsFilter.Contains(x.ApplicationName));

        IOrderedQueryable<NLogRecordModelDB> oq = req.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.RecordTime)
          : q.OrderByDescending(x => x.RecordTime);

        int trc = await q.CountAsync(cancellationToken: token);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = trc,
            Response = [.. await oq.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token)]
        };
    }
}