////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml;
using System.Data;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> NomenclatureUpdateOrCreateAsync(NomenclatureModelDB nom, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (string.IsNullOrWhiteSpace(nom.Name))
        {
            res.AddError("Nomenclature required Name");
            return res;
        }
        nom.Name = nom.Name.Trim();
        // loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(nom, GlobalStaticConstants.JsonSerializerSettings)}");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        string msg, about = $"'{nom.Name}' /{nom.BaseUnit}";
        NomenclatureModelDB? nomenclature_db = await context.Nomenclatures.FirstOrDefaultAsync(x => x.Name == nom.Name && x.BaseUnit == nom.BaseUnit && x.Id != nom.Id, cancellationToken: token);

        if (nomenclature_db is not null)
        {
            msg = $"Ошибка создания Номенклатуры {about}. Такой объект уже существует #{nomenclature_db.Id}. Требуется уникальное сочетание имени и единицы измерения";
            loggerRepo.LogWarning(msg);
            res.AddError(msg);
            return res;
        }
        DateTime dtu = DateTime.UtcNow;
        nom.LastUpdatedAtUTC = dtu;

        if (nom.Id < 1)
        {
            nom.Id = 0;
            nom.CreatedAtUTC = dtu;
            nomenclature_db = nom;
            nom.SortIndex = await context.Nomenclatures.AnyAsync(cancellationToken: token)
                ? await context.Nomenclatures.MaxAsync(x => x.SortIndex, cancellationToken: token) + 1
                : 0;

            await context.Nomenclatures.AddAsync(nomenclature_db, token);
            await context.SaveChangesAsync(token);
            msg = $"Номенклатура {about} создана #{nomenclature_db.Id}";
            loggerRepo.LogInformation(msg);
            res.AddSuccess(msg);
            res.Response = nomenclature_db.Id;
            return res;
        }

        res.Response = await context.Nomenclatures
            .Where(x => x.Id == nom.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, nom.Name)
            .SetProperty(p => p.NormalizedNameUpper, nom.Name.ToUpper().Trim())
            .SetProperty(p => p.Description, nom.Description)
            .SetProperty(p => p.BaseUnit, nom.BaseUnit)
            .SetProperty(p => p.IsDisabled, nom.IsDisabled)
            .SetProperty(p => p.ContextName, nom.ContextName)
            .SetProperty(p => p.ProjectId, nom.ProjectId)
            .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        msg = $"Обновление номенклатуры {about} выполнено";
        loggerRepo.LogInformation(msg);
        res.AddSuccess(msg);
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricsForNomenclaturesSetAsync(TAuthRequestStandardModel<RubricsSetModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError($"req.Payload is null > {nameof(RubricsForNomenclaturesSetAsync)}");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        int resChanges;
        if (req.Payload.RubricsIds.Length == 0)
        {
            resChanges = await context.NomenclaturesRubricsJoins.Where(x => x.NomenclatureId == req.Payload.OwnerId).ExecuteDeleteAsync(cancellationToken: token);
            return resChanges == 0
                ? ResponseBaseModel.CreateInfo("У номенклатуры нет рубрик")
                : ResponseBaseModel.CreateSuccess("Удалены все рубрики для номенклатуры");
        }
        NomenclatureRubricJoinModelDB[] rubrics_db = await context
            .NomenclaturesRubricsJoins
            .Where(x => x.NomenclatureId == req.Payload.OwnerId)
            .ToArrayAsync(cancellationToken: token);
        ResponseBaseModel res = new();
        int[] _ids = [.. rubrics_db.Where(x => !req.Payload.RubricsIds.Contains(x.RubricId)).Select(x => x.Id)];
        if (_ids.Length != 0)
        {
            resChanges = await context.NomenclaturesRubricsJoins
                .Where(x => _ids.Any(y => y == x.Id))
                .ExecuteDeleteAsync(cancellationToken: token);

            res.AddInfo($"Удалено рубрик: {resChanges}");
        }

        _ids = [.. req.Payload.RubricsIds.Where(x => !rubrics_db.Any(y => y.RubricId == x))];
        if (_ids.Length != 0)
        {
            await context.NomenclaturesRubricsJoins.AddRangeAsync(_ids.Select(x => new NomenclatureRubricJoinModelDB() { NomenclatureId = req.Payload.OwnerId, RubricId = x }), token);
            resChanges = await context.SaveChangesAsync(token);
            res.AddSuccess($"Добавлено рубрик: {resChanges}");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        if (req.Payload is null || req.Payload.Length == 0)
            return new()
            {
                Messages = [new() { Text = "req.Payload is null || req.Payload.Length == 0", TypeMessage = MessagesTypesEnum.Error }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context
            .Nomenclatures
            .Where(x => req.Payload.Any(y => x.Id == y))
            .Include(x => x.Offers)
            .Include(x => x.RubricsJoins)
            .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError($"req.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<NomenclatureModelDB> q = string.IsNullOrEmpty(req.Payload.ContextName)
            ? context.Nomenclatures.Where(x => x.ContextName == null || x.ContextName == "").AsQueryable()
            : context.Nomenclatures.Where(x => x.ContextName == req.Payload.ContextName).AsQueryable();

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.AfterDateUpdate);

        IOrderedQueryable<NomenclatureModelDB> oq = req.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.CreatedAtUTC)
          : q.OrderByDescending(x => x.CreatedAtUTC);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = [.. await oq.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token)]
        };
    }
}