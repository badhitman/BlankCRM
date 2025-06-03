////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsTransmission;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace HelpDeskService;

/// <summary>
/// RubricsService
/// </summary>
public class RubricsService(
    IDbContextFactory<HelpDeskContext> helpdeskDbFactory,
    IMemoryCache cache) : IRubricsService
{
    static readonly TimeSpan _ts = TimeSpan.FromSeconds(5);

    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default)
    {
        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);
        IQueryable<UniversalBaseModel> q = context
            .Rubrics
            .Where(x => x.ProjectId == req.ProjectId && x.ContextName == req.ContextName)
            .Select(x => new UniversalBaseModel()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id,
                IsDisabled = x.IsDisabled,
                ParentId = x.ParentId,
                ProjectId = x.ProjectId,
                SortIndex = x.SortIndex,
            })
            .AsQueryable();

        if (req.Request < 1)
            q = q.Where(x => x.ParentId == null || x.ParentId < 1);
        else
            q = q.Where(x => x.ParentId == req.Request);

        return await q.ToListAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricMoveAsync(TRequestModel<RowMoveModel> req, CancellationToken token = default)
    {
        ResponseBaseModel res = new();

        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);

        var data = await context
        .Rubrics
        .Where(x => x.Id == req.Payload.ObjectId)
        .Select(x => new { x.Id, x.ParentId, x.Name })
        .FirstAsync(x => x.Id == req.Payload.ObjectId, cancellationToken: token);

        using IDbContextTransaction transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
        LockUniqueTokenModelDB locker = new() { Token = $"rubric-sort-upd-{data.ParentId}" };
        try
        {
            await context.AddAsync(locker, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            res.AddError($"Не удалось выполнить команду: {ex.Message}");
            return res;
        }

        List<RubricModelDB> all = await context
            .Rubrics
            .Where(x => x.ContextName == req.Payload.ContextName && x.ParentId == data.ParentId)
            .OrderBy(x => x.SortIndex)
            .ToListAsync(cancellationToken: token);

        int i = all.FindIndex(x => x.Id == data.Id);
        if (req.Payload.Direction == DirectionsEnum.Up)
        {
            if (i == 0)
            {
                res.AddInfo("Элемент уже в крайнем положении.");
            }
            else
            {
                RubricModelDB r1 = all[i - 1], r2 = all[i];
                uint val1 = r1.SortIndex, val2 = r2.SortIndex;
                r1.SortIndex = uint.MaxValue;
                context.Update(r1);
                await context.SaveChangesAsync(token);
                r2.SortIndex = val1;
                context.Update(r2);
                await context.SaveChangesAsync(token);
                r1.SortIndex = val2;
                context.Update(r1);
                await context.SaveChangesAsync(token);

                res.AddSuccess($"Рубрика '{data.Name}' сдвинута выше");
            }
        }
        else
        {
            if (i == all.Count - 1)
            {
                res.AddInfo("Элемент уже в крайнем положении.");
            }
            else
            {
                RubricModelDB r1 = all[i + 1], r2 = all[i];
                uint val1 = r1.SortIndex, val2 = r2.SortIndex;
                r1.SortIndex = uint.MaxValue;
                context.Update(r1);
                await context.SaveChangesAsync(token);
                r2.SortIndex = val1;
                context.Update(r2);
                await context.SaveChangesAsync(token);
                r1.SortIndex = val2;
                context.Update(r1);
                await context.SaveChangesAsync(token);

                res.AddSuccess($"Рубрика '{data.Name}' сдвинута ниже");
            }
        }

        all = [.. all.OrderBy(x => x.SortIndex)];

        bool nu = false;
        uint si = 0;
        all.ForEach(x =>
        {
            si++;
            nu = nu || x.SortIndex != si;
            x.SortIndex = si;
        });

        if (nu)
        {
            context.UpdateRange(all);
        }
        context.Remove(locker);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel rubric, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        Regex rx = new(@"\s+", RegexOptions.Compiled);
        rubric.Name = rx.Replace(rubric.Name.Trim(), " ");
        if (string.IsNullOrWhiteSpace(rubric.Name))
        {
            res.AddError("Объект должен иметь имя");
            return res;
        }
        rubric.NormalizedNameUpper = rubric.Name.ToUpper();
        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);

        if (await context.Rubrics.AnyAsync(x => x.Id != rubric.Id && x.ParentId == rubric.ParentId && x.Name == rubric.Name, cancellationToken: token))
        {
            res.AddError("Объект с таким именем уже существует в данном узле");
            return res;
        }

        if (rubric.Id < 1)
        {
            uint[] six = await context
                            .Rubrics
                            .Where(x => x.ParentId == rubric.ParentId)
                            .Select(x => x.SortIndex)
                            .ToArrayAsync(cancellationToken: token);

            rubric.SortIndex = six.Length == 0 ? 1 : six.Max() + 1;

            await context.AddAsync(rubric, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Объект успешно создан");
            res.Response = rubric.Id;
        }
        else
        {
            res.Response = await context
                .Rubrics
                .Where(x => x.Id == rubric.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsDisabled, rubric.IsDisabled)
                .SetProperty(p => p.Name, rubric.Name)
                .SetProperty(p => p.Description, rubric.Description), cancellationToken: token);

            res.AddSuccess("Объект успешно обновлён");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricStandardModel>?>> RubricReadAsync(int rubricId, CancellationToken token = default)
    {
        TResponseModel<List<RubricStandardModel>?> res = new();

        if (rubricId < 1)
            return res;

        string mem_key = $"{TransmissionQueues.RubricForIssuesReadHelpDeskReceive}-{rubricId}";
        if (cache.TryGetValue(mem_key, out List<RubricStandardModel>? rubric))
        {
            res.Response = rubric;
            return res;
        }

        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);

        RubricStandardModel? lpi = await context
            .Rubrics
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id == rubricId, cancellationToken: token);

        if (lpi is null)
        {
            res.AddWarning($"Рубрика #{rubricId} не найдена в БД (вероятно была удалена)");
            return res;
        }

        List<RubricStandardModel> ctrl = [lpi];

        while (lpi.Parent is not null)
        {
            ctrl.Add(await context
            .Rubrics
            .Include(x => x.Parent)
            .ThenInclude(x => x!.NestedRubrics)
            .FirstAsync(x => x.Id == lpi.Parent.Id, cancellationToken: token));
            lpi = ctrl.Last();
        }

        res.Response = ctrl;
        cache.Set(mem_key, res.Response, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_ts));
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(int[] rubricsIds, CancellationToken token = default)
    {
        TResponseModel<List<RubricStandardModel>> res = new();
        rubricsIds = [.. rubricsIds.Where(x => x > 0)];
        if (rubricsIds.Length == 0)
        {
            res.AddError("Пустой запрос");
            return res;
        }
        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);
        List<RubricModelDB> resDb = await context.Rubrics.Where(x => rubricsIds.Any(y => y == x.Id)).ToListAsync(cancellationToken: token);
        res.Response = [.. resDb.Select(x => x)];
        return res;
    }
}