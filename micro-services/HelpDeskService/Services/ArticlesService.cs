////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace HelpDeskService;

/// <summary>
/// Articles
/// </summary>
public class ArticlesService(IDbContextFactory<HelpDeskContext> helpdeskDbFactory) : IArticlesService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> ArticleCreateOrUpdateAsync(ArticleModelDB article, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        Regex rx = new(@"\s+", RegexOptions.Compiled);
        article.Name = rx.Replace(article.Name.Trim(), " ");
        if (string.IsNullOrWhiteSpace(article.Name))
        {
            res.AddError("Укажите название");
            return res;
        }

        article.NormalizedNameUpper = article.Name.ToUpper();
        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);

        DateTime dtu = DateTime.UtcNow;
        if (article.Id < 1)
        {
            article.Id = 0;
            article.CreatedAtUTC = dtu;

            await context.AddAsync(article, token);
            await context.SaveChangesAsync(token);
            res.Response = article.Id;
            res.AddSuccess("Статья успешно создана");
            return res;
        }
        res.Response = await context.Articles
            .Where(a => a.Id == article.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.LastUpdatedAtUTC, dtu)
            .SetProperty(p => p.Name, article.Name)
            .SetProperty(p => p.Description, article.Description)
            .SetProperty(p => p.NormalizedNameUpper, article.NormalizedNameUpper), cancellationToken: token);

        if (res.Response < 1)
            res.AddInfo("Запрос не вызвал измений в БД");

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ArticleModelDB[]>> ArticlesReadAsync(int[] req, CancellationToken token = default)
    {
        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);
#if DEBUG
        var _res = await context.Articles
            .Where(x => req.Any(y => y == x.Id))
            .Include(x => x.RubricsJoins!)
            .ThenInclude(x => x.Rubric)
            .ToArrayAsync(cancellationToken: token);
#endif
        return new()
        {
            Response = await context.Articles
            .Where(x => req.Any(y => y == x.Id))
            .Include(x => x.RubricsJoins!)
            .ThenInclude(x => x.Rubric)
            .ToArrayAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ArticleModelDB>> ArticlesSelectAsync(TPaginationRequestStandardModel<SelectArticlesRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new();

        if (req.PageSize < 5)
            req.PageSize = 5;

        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);

        IQueryable<ArticleModelDB> q = context
            .Articles
            .Where(x => x.ProjectId == req.Payload.ProjectId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.Payload.SearchQuery))
        {
            req.Payload.SearchQuery = req.Payload.SearchQuery.ToUpper();

            q = from article_element in q
                join rj_element in context.RubricsArticlesJoins on article_element.Id equals rj_element.ArticleId into outer_rj
                from rj_item in outer_rj.DefaultIfEmpty()
                join rubric_element in context.Rubrics on rj_item.RubricId equals rubric_element.Id into outer_rubric
                from rubric_item in outer_rubric.DefaultIfEmpty()
                where article_element.NormalizedNameUpper!.Contains(req.Payload.SearchQuery) || rubric_item.NormalizedNameUpper!.Contains(req.Payload.SearchQuery)
                select article_element;
        }

        IQueryable<ArticleModelDB> oq = req.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.LastUpdatedAtUTC).ThenBy(x => x.CreatedAtUTC).Skip(req.PageNum * req.PageSize).Take(req.PageSize)
          : q.OrderByDescending(x => x.LastUpdatedAtUTC).ThenByDescending(x => x.CreatedAtUTC).Skip(req.PageNum * req.PageSize).Take(req.PageSize);

        var inc = oq
            .Include(x => x.RubricsJoins)
            ;

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = [.. req.Payload.IncludeExternal ? await inc.ToListAsync(cancellationToken: token) : await oq.ToListAsync(cancellationToken: token)]
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricsForArticleSetAsync(RubricsSetModel req, CancellationToken token = default)
    {
        using HelpDeskContext context = await helpdeskDbFactory.CreateDbContextAsync(token);
        int resChanges;
        if (req.RubricsIds.Length == 0)
        {
            resChanges = await context.RubricsArticlesJoins.Where(x => x.ArticleId == req.OwnerId).ExecuteDeleteAsync(cancellationToken: token);
            return resChanges == 0
                ? ResponseBaseModel.CreateInfo("У статьи нет рубрик")
                : ResponseBaseModel.CreateSuccess("Удалены все рубрики для статьи");
        }
        RubricArticleJoinModelDB[] rubrics_db = await context
            .RubricsArticlesJoins
            .Where(x => x.ArticleId == req.OwnerId)
            .ToArrayAsync(cancellationToken: token);
        ResponseBaseModel res = new();
        int[] _ids = [.. rubrics_db.Where(x => !req.RubricsIds.Contains(x.RubricId)).Select(x => x.Id)];
        if (_ids.Length != 0)
        {
            resChanges = await context.RubricsArticlesJoins
                .Where(x => _ids.Any(y => y == x.Id))
                .ExecuteDeleteAsync(cancellationToken: token);

            res.AddInfo($"Удалено рубрик: {resChanges}");
        }

        _ids = [.. req.RubricsIds.Where(x => !rubrics_db.Any(y => y.RubricId == x))];
        if (_ids.Length != 0)
        {
            await context.RubricsArticlesJoins.AddRangeAsync(_ids.Select(x => new RubricArticleJoinModelDB() { ArticleId = req.OwnerId, RubricId = x }), token);
            resChanges = await context.SaveChangesAsync(token);

            res.AddSuccess($"Добавлено рубрик: {resChanges}");
        }

        return res;
    }
}