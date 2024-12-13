﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Attendance
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WorkScheduleUpdate(WorkScheduleModelDB req)
    {
        TResponseModel<int> res = new() { Response = 0 };
        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.NormalizedNameUpper = req.Name.ToUpper();
        req.SortIndex = 0;
        req.Nomenclature = null;
        req.Offer = null;
        req.LastAtUpdatedUTC = DateTime.UtcNow;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();
        if (req.Id < 1)
        {
            req.Id = 0;
            req.CreatedAtUTC = req.LastAtUpdatedUTC;
            context.Add(req);
            await context.SaveChangesAsync();
            res.Response = req.Id;
        }
        else
        {
            context.Update(req);
            res.Response = await context.SaveChangesAsync();
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<WorkScheduleModelDB>>> WorkSchedulesSelect(TPaginationRequestModel<WorkSchedulesSelectRequestModel> req)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();

        IQueryable<WorkScheduleModelDB> q = context
            .WorksSchedules
            .AsQueryable();

        if (req.Payload.OfferFilter.HasValue && req.Payload.OfferFilter.Value != 0)
            q = q.Where(x => context.RowsOfOrdersDocuments.Any(y => y.OrderDocumentId == x.Id && y.OfferId == req.Payload.OfferFilter));

        if (req.Payload.NomenclatureFilter.HasValue && req.Payload.NomenclatureFilter.Value != 0)
            q = q.Where(x => context.RowsOfOrdersDocuments.Any(y => y.OrderDocumentId == x.Id && y.NomenclatureId == req.Payload.NomenclatureFilter));

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastAtUpdatedUTC >= req.Payload.AfterDateUpdate || (x.LastAtUpdatedUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.AfterDateUpdate));

        IOrderedQueryable<WorkScheduleModelDB> oq = req.SortingDirection == VerticalDirectionsEnum.Up
           ? q.OrderBy(x => x.StartPart)
           : q.OrderByDescending(x => x.StartPart);

        IQueryable<WorkScheduleModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WorkScheduleModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature);

        return new()
        {
            Response = new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = await q.CountAsync(),
                Response = req.Payload.IncludeExternalData ? [.. await inc_query.ToArrayAsync()] : [.. await pq.ToArrayAsync()]
            },
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<WorkScheduleModelDB[]>> WorkSchedulesRead(int[] req)
    {
        TResponseModel<WorkScheduleModelDB[]> res = new();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();

        IQueryable<WorkScheduleModelDB> q = context
            .WorksSchedules
            .Where(x => req.Any(y => x.Id == y));

        res.Response = await q
            .Include(x => x.Offer!)
            .Include(x => x.Nomenclature)
            .ToArrayAsync();

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WorkScheduleCalendarUpdate(WorkScheduleCalendarModelDB req)
    {
        TResponseModel<int> res = new() { Response = 0 };
        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.NormalizedNameUpper = req.Name.ToUpper();
        req.SortIndex = 0;
        req.Nomenclature = null;
        req.Offer = null;
        req.LastAtUpdatedUTC = DateTime.UtcNow;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();
        if (req.Id < 1)
        {
            req.Id = 0;
            req.CreatedAtUTC = req.LastAtUpdatedUTC;
            context.Add(req);
            await context.SaveChangesAsync();
            res.Response = req.Id;
        }
        else
        {
            context.Update(req);
            res.Response = await context.SaveChangesAsync();
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<WorkScheduleCalendarModelDB>>> WorkScheduleCalendarsSelect(TPaginationRequestModel<WorkScheduleCalendarsSelectRequestModel> req)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();

        IQueryable<WorkScheduleCalendarModelDB> q = context
            .WorksSchedulesCalendar
            .AsQueryable();

        if (req.Payload.OfferFilter.HasValue && req.Payload.OfferFilter.Value != 0)
            q = q.Where(x => context.RowsOfOrdersDocuments.Any(y => y.OrderDocumentId == x.Id && y.OfferId == req.Payload.OfferFilter));

        if (req.Payload.NomenclatureFilter.HasValue && req.Payload.NomenclatureFilter.Value != 0)
            q = q.Where(x => context.RowsOfOrdersDocuments.Any(y => y.OrderDocumentId == x.Id && y.NomenclatureId == req.Payload.NomenclatureFilter));

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastAtUpdatedUTC >= req.Payload.AfterDateUpdate || (x.LastAtUpdatedUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.AfterDateUpdate));

        IOrderedQueryable<WorkScheduleCalendarModelDB> oq = req.SortingDirection == VerticalDirectionsEnum.Up
           ? q.OrderBy(x => x.DateScheduleCalendar).ThenBy(x => x.StartPart)
           : q.OrderByDescending(x => x.DateScheduleCalendar).ThenByDescending(x => x.StartPart);

        IQueryable<WorkScheduleCalendarModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WorkScheduleCalendarModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature);

        return new()
        {
            Response = new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = await q.CountAsync(),
                Response = req.Payload.IncludeExternalData ? [.. await inc_query.ToArrayAsync()] : [.. await pq.ToArrayAsync()]
            },
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<WorkScheduleCalendarModelDB[]>> WorkScheduleCalendarsRead(int[] req)
    {
        TResponseModel<WorkScheduleCalendarModelDB[]> res = new();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();

        IQueryable<WorkScheduleCalendarModelDB> q = context
            .WorksSchedulesCalendar
            .Where(x => req.Any(y => x.Id == y));

        res.Response = await q
            .Include(x => x.Offer!)
            .Include(x => x.Nomenclature)
            .ToArrayAsync();

        return res;
    }
}