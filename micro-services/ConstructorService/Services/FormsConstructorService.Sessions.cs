////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Net.Mail;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace ConstructorService;

/// <summary>
/// Constructor служба
/// </summary>
public partial class FormsConstructorService : IConstructorService
{
    /////////////// Пользовательский/публичный доступ к возможностям заполнения документа данными
    // Если у вас есть готовый к заполнению документ со всеми его табами и настройками, то вы можете создавать уникальные ссылки для заполнения данными
    // Каждая ссылка это всего лишь уникальный GUID к которому привязываются все данные, которые вводят конечные пользователи
    // Пользователи видят ваш документ, но сам документ данные не хранит. Хранение данных происходит в сессиях, которые вы сами выпускаете для любого вашего документа

    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionFormAsync(SaveConstructorSessionRequestModel req, CancellationToken token = default)
    {
        req.SessionValues = [.. req.SessionValues.SkipWhile(x => x.Id < 1 && string.IsNullOrWhiteSpace(x.Value))];

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(token);
        SessionOfDocumentDataModelDB? sq = await context_forms
            .Sessions
            .Include(x => x.DataSessionValues!)
            .ThenInclude(x => x.JoinFormToTab)
            .FirstOrDefaultAsync(x => x.Id == req.SessionId, cancellationToken: token);

        TResponseModel<ValueDataForSessionOfDocumentModelDB[]> res = new();

        if (sq is null)
        {
            res.AddError("Объект документа удалён");
            return res;
        }

        ValueDataForSessionOfDocumentModelDB[] _session_data = [.. sq
            .DataSessionValues!
            .Where(x => x.JoinFormToTabId == req.JoinFormToTab)];

        int[] _ids_del = [.. req.SessionValues
            .Where(x => x.Id > 0 && string.IsNullOrWhiteSpace(x.Value))
            .Select(x => x.Id)];

        if (_ids_del.Length != 0)
        {
            context_forms.RemoveRange(context_forms.Sessions.Where(x => _ids_del.Contains(x.Id)));
            _ = await context_forms.SaveChangesAsync(token);
        }

        ValueDataForSessionOfDocumentModelDB[] values_upd = [.. _session_data
            .Where(x => req.SessionValues.Any(y => y.Id == x.Id && x.Value != y.Value))
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .Select(x => { x.Value = req.SessionValues.First(y => y.Id == x.Id).Value; return x; })];

        if (values_upd.Length != 0)
        {
            context_forms.UpdateRange(values_upd);
            await context_forms.SaveChangesAsync(token);
        }

        values_upd = [.. req.SessionValues.Where(x => x.Id == 0)];
        if (values_upd.Length != 0)
        {
            await context_forms.ValuesSessions.AddRangeAsync(values_upd);
            await context_forms.SaveChangesAsync(token);
        }

        res.AddSuccess("Форма документа сохранена");
        res.Response = await context_forms
            .ValuesSessions
            .Where(x => x.OwnerId == req.SessionId && x.JoinFormToTabId == req.JoinFormToTab)
            .ToArrayAsync(cancellationToken: token);

        if (res.Response.Any(x => string.IsNullOrWhiteSpace(x.Value)))
        {
            _ids_del = [..res
                .Response
                .Where(x => string.IsNullOrWhiteSpace(x.Value))
                .Select(x => x.Id)];

            context_forms.RemoveRange(context_forms.ValuesSessions.Where(x => _ids_del.Contains(x.Id)));
            await context_forms.SaveChangesAsync(token);
            res.Response = [.. res.Response.SkipWhile(x => string.IsNullOrWhiteSpace(x.Value))];
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetStatusSessionDocumentAsync(SessionStatusModel req, CancellationToken cancellationToken = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        SessionOfDocumentDataModelDB? sq = await context_forms.Sessions.FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: cancellationToken);
        if (sq is null)
            return ResponseBaseModel.CreateError($"Сессия [{req.Id}] не найдена в БД. ошибка A85733AF-56F4-45D2-A16C-729352D1645B");

        if (sq.SessionStatus == req.Status)
            return ResponseBaseModel.CreateSuccess($"Сессия уже переведена в статус [{req.Status}] и не требует обработки");

        sq.SessionStatus = req.Status;

        await context_forms.SaveChangesAsync(cancellationToken);
        ResponseBaseModel res = new();
        string msg = $"Сессия опросника/анкетирования `{sq.SessionToken}` {req.Status}";
        if (!string.IsNullOrWhiteSpace(sq.EmailsNotifications))
        {
            try
            {
                res.AddSuccess($"Наблюдатели оповещены:{sq.EmailsNotifications}");
            }
            catch (Exception ex)
            {
                logger.LogError($"error : {ex.Message}\n: {ex.StackTrace}");
                res.AddWarning($"Не удалось отправить уведомление наблюдателям [{sq.EmailsNotifications}]. Возникло исключение 7F67AA0A-7F5F-499D-8680-73A665106D8E: {ex.Message}");
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentAsync(SessionGetModel req, CancellationToken cancellationToken = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<SessionOfDocumentDataModelDB> q = context_forms
            .Sessions
            .Where(x => x.Id == req.SessionId)
            .AsQueryable();

        IIncludableQueryable<SessionOfDocumentDataModelDB, List<FieldFormAkaDirectoryConstructorModelDB>?> inc = q
            .Include(x => x.Project)
            .Include(x => x.DataSessionValues)

            .Include(s => s.Owner) // опрос/анкета
            .ThenInclude(x => x!.Tabs!) // страницы опроса/анкеты
            .ThenInclude(x => x.JoinsForms!) // формы для страницы опроса/анкеты
            .ThenInclude(x => x.Form) // форма
            .ThenInclude(x => x!.Fields) // поля

            .Include(s => s.Owner) // опрос/анкета
            .ThenInclude(x => x!.Tabs!) // страницы опроса/анкеты
            .ThenInclude(x => x.JoinsForms!) // формы для страницы опроса/анкеты
            .ThenInclude(x => x.Form) // форма
            .ThenInclude(x => x!.FieldsDirectoriesLinks);

        TResponseModel<SessionOfDocumentDataModelDB> res = new()
        {
            Response = req.IncludeExtra
            ? await inc.FirstOrDefaultAsync(cancellationToken: cancellationToken)
            : await q.FirstOrDefaultAsync(cancellationToken: cancellationToken)
        };
        string msg;
        if (res.Response is null)
        {
            msg = $"for {nameof(req.SessionId)} = [{req.SessionId}]. SessionDocument is null. error 965BED19-5E30-4AA5-8FBD-1B3EFEFC5B1D";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> UpdateOrCreateSessionDocumentAsync(SessionOfDocumentDataModelDB session_json, CancellationToken cancellationToken = default)
    {
        TResponseModel<SessionOfDocumentDataModelDB> res = new();

        (bool IsValid, List<ValidationResult> ValidationResults) = GlobalTools.ValidateObject(session_json);
        if (!IsValid)
        {
            res.Messages.InjectException(ValidationResults);
            return res;
        }

        if (!string.IsNullOrWhiteSpace(session_json.EmailsNotifications))
        {
            string[] een = session_json.EmailsNotifications.SplitToList().Where(x => !MailAddress.TryCreate(x, out _)).ToArray();
            if (een.Length != 0)
            {
                res.AddError($"Не корректные адреса получателей. {JsonConvert.SerializeObject(een, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}. error AFDDD9DE-F36E-4FB0-9C10-ACDAF48409A8");
                return res;
            }
        }

        if (session_json.SessionStatus == SessionsStatusesEnum.None)
            session_json.SessionToken = null;

        session_json.Name = MyRegexSpices().Replace(session_json.Name.Trim(), " ");
        session_json.NormalizedUpperName = session_json.Name.ToUpper();

        TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync([session_json.AuthorUser], cancellationToken);
        if (!restUsers.Success())
            throw new Exception(restUsers.Message());

        UserInfoModel? userDb = restUsers.Response?.Single();

        if (userDb is null)
        {
            res.AddError($"Пользователь #{session_json.AuthorUser} не найден в БД");
            return res;
        }

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);

        Expression<Func<SessionOfDocumentDataModelDB, bool>> expr = x
            => x.Id != session_json.Id &&
            x.OwnerId == session_json.OwnerId &&
            x.ProjectId == session_json.ProjectId &&
            x.NormalizedUpperName == session_json.NormalizedUpperName;

        SessionOfDocumentDataModelDB? session_db = await context_forms
            .Sessions
            .FirstOrDefaultAsync(expr, cancellationToken: cancellationToken);

        if (session_db is not null)
        {
            res.AddError($"Ссылка для выбранного документа с таким именем уже существует в БД! Для одного и того же документа имена должны быть уникальны.");
            return res;
        }

        if (session_json.Id < 1)
        {
            session_json.CreatedAt = DateTime.UtcNow;
            session_json.DeadlineDate = DateTime.UtcNow.AddMinutes(_conf.Value.TimeActualityDocumentSessionMinutes);
            session_json.SessionToken = Guid.NewGuid().ToString();
            session_json.SessionStatus = SessionsStatusesEnum.InProgress;

            await context_forms.AddAsync(session_json, cancellationToken);
            await context_forms.SaveChangesAsync(cancellationToken);
            return new() { Response = session_json, Messages = [new() { TypeMessage = MessagesTypesEnum.Success, Text = $"Создана сессия #{session_json.Id}" }] };
        }
        session_db = await context_forms.Sessions.FirstOrDefaultAsync(x => x.Id == session_json.Id, cancellationToken: cancellationToken);
        string msg;
        if (session_db is null)
        {
            msg = $"Сессия #{session_json.Id} не найдена в БД";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        if (session_db.Name == session_json.Name &&
            session_db.Description == session_json.Description &&
            session_db.EmailsNotifications == session_json.EmailsNotifications &&
            session_db.SessionStatus == session_json.SessionStatus &&
            session_db.ShowDescriptionAsStartPage == session_json.ShowDescriptionAsStartPage &&
            session_db.DeadlineDate == session_json.DeadlineDate &&
            session_db.SessionToken == session_json.SessionToken)
        {
            msg = $"Сессия #{session_json.Id} не требует изменений в БД";
            res.AddInfo(msg);
            logger.LogInformation(msg);
            return res;
        }
        else if (Guid.TryParse(session_json.SessionToken, out Guid _guid_parsed) && _guid_parsed != Guid.Empty && await context_forms.Sessions.AnyAsync(x => x.Id != session_db.Id && x.SessionToken == session_json.SessionToken, cancellationToken: cancellationToken))
        {
            msg = $"Токен сессии {session_json.SessionToken} уже занят";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        else
        {
            session_db.Name = session_json.Name;
            session_db.Description = session_json.Description;
            session_db.EmailsNotifications = session_json.EmailsNotifications;
            session_db.SessionStatus = session_json.SessionStatus;
            session_db.ShowDescriptionAsStartPage = session_json.ShowDescriptionAsStartPage;
            session_db.DeadlineDate = session_json.DeadlineDate;

            if (Guid.Empty.ToString() == session_json.SessionToken)
                session_db.SessionToken = Guid.NewGuid().ToString();
            else
                session_db.SessionToken = session_json.SessionToken;

            context_forms.Update(session_db);
            msg = $"Сессия #{session_db.Id} обновлена в БД";
            res.AddInfo(msg);
            logger.LogInformation(msg);
            await context_forms.SaveChangesAsync(cancellationToken);
        }

        return await GetSessionDocumentAsync(new() { SessionId = session_json.Id }, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>> RequestSessionsDocumentsAsync(RequestSessionsDocumentsRequestPaginationModel req, CancellationToken cancellationToken = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<SessionOfDocumentDataModelDB> query = context_forms
            .Sessions
            .Include(x => x.Owner)
            .OrderByDescending(x => x.CreatedAt)
            .AsQueryable();

        if (req.DocumentSchemeId > 0)
            query = query.Where(x => x.OwnerId == req.DocumentSchemeId);

        if (req.ProjectId > 0)
            query = query.Where(x => context_forms.DocumentSchemes.Any(y => y.Id == x.OwnerId && y.ProjectId == req.ProjectId));

        if (!string.IsNullOrWhiteSpace(req.FilterUserId))
            query = query.Where(x => x.AuthorUser == req.FilterUserId);

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
        {
            Expression<Func<SessionOfDocumentDataModelDB, bool>> expr = x
                => EF.Functions.Like(x.NormalizedUpperName!, $"%{req.FindQuery.ToUpper()}%") ||
                (x.SessionToken != null && x.SessionToken.ToLower() == req.FindQuery.ToLower()) ||
                (!string.IsNullOrWhiteSpace(x.EmailsNotifications) && EF.Functions.Like(x.EmailsNotifications.ToLower(), $"%{req.FindQuery.ToLower()}%"));

            query = query.Where(expr);
        }
        int _totalRowsCount = await query.CountAsync(cancellationToken: cancellationToken);
        query = query.OrderBy(x => x.Id).Skip(req.PageSize * req.PageNum).Take(req.PageSize);

        List<SessionOfDocumentDataModelDB> response = await query.ToListAsync(cancellationToken: cancellationToken);

        if (response.Count != 0)
        {
            string[] users_ids = response.Select(x => x.AuthorUser).Distinct().ToArray();


            TResponseModel<UserInfoModel[]> restUsers = await identityRepo.GetUsersOfIdentityAsync(users_ids, cancellationToken);
            if (!restUsers.Success())
                throw new Exception(restUsers.Message());

            UserInfoModel[] users_data = restUsers.Response!;

            response.ForEach(x => { x.AuthorUser = users_data.FirstOrDefault(y => y.UserId.Equals(x.AuthorUser))?.UserName ?? x.AuthorUser; });
        }

        return new(req)
        {
            TotalRowsCount = _totalRowsCount,
            Response = response
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictStandardModel[]>> FindSessionsDocumentsByFormFieldNameAsync(FormFieldModel req, CancellationToken cancellationToken = default)
    {
        TResponseModel<EntryDictStandardModel[]> res = new();
        if (string.IsNullOrWhiteSpace(req.FieldName))
        {
            res.AddError("Не указано имя поля/колонки");
            return res;
        }
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        var q = from _vs in context_forms.ValuesSessions.Where(_vs => _vs.Name == req.FieldName)
                join _s in context_forms.Sessions on _vs.OwnerId equals _s.Id
                join _pjf in context_forms.TabsJoinsForms.Where(x => x.FormId == req.FormId) on _vs.JoinFormToTabId equals _pjf.Id
                join _qp in context_forms.TabsOfDocumentsSchemes on _pjf.TabId equals _qp.Id
                select new { Value = _vs, Session = _s, DocumentPageJoinForm = _pjf, DocumentPage = _qp };

        var data_rows = await q.ToArrayAsync(cancellationToken: cancellationToken);

        res.Response = [.. data_rows
            .GroupBy(x => x.Session.Id)
            .Select(x =>
            {
                var element_g = x.First();
                Dictionary<string, object> _d = new()
                {
                    { nameof(Enumerable.Count), x.Count() },
                    { nameof(element_g.Session.CreatedAt), element_g.Session.CreatedAt },
                    { nameof(element_g.Session.AuthorUser), element_g.Session.AuthorUser },
                    { nameof(element_g.Session.SessionStatus), element_g.Session.SessionStatus }
                };

                if (!string.IsNullOrWhiteSpace(element_g.Session.EmailsNotifications))
                    _d.Add(nameof(element_g.Session.EmailsNotifications), element_g.Session.EmailsNotifications);

                if (!string.IsNullOrWhiteSpace(element_g.Session.Editors))
                    _d.Add(nameof(element_g.Session.Editors), element_g.Session.Editors);

                if (!string.IsNullOrWhiteSpace(element_g.Session.SessionToken))
                    _d.Add(nameof(element_g.Session.SessionToken), element_g.Session.SessionToken);
                if (element_g.Session.DeadlineDate is not null)
                    _d.Add(nameof(element_g.Session.DeadlineDate), element_g.Session.DeadlineDate);
                if (element_g.Session.LastDocumentUpdateActivity is not null)
                    _d.Add(nameof(element_g.Session.LastDocumentUpdateActivity), element_g.Session.LastDocumentUpdateActivity);

                return new EntryDictStandardModel()
                {
                    Id = x.Key,
                    Name = element_g.Session.Name,
                    Tag = _d
                };
            })];
        res.AddInfo($"Получено ссылок {res.Response.Length}");
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearValuesForFieldNameAsync(FormFieldOfSessionModel req, CancellationToken cancellationToken = default)
    {
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<ValueDataForSessionOfDocumentModelDB> q = from _vs in context_forms.ValuesSessions.Where(_vs => _vs.Name == req.FieldName)
                                                             join _s in context_forms.Sessions.Where(x => !req.SessionId.HasValue || x.Id == req.SessionId.Value) on _vs.OwnerId equals _s.Id
                                                             join _pjf in context_forms.TabsJoinsForms.Where(x => x.FormId == req.FormId) on _vs.JoinFormToTabId equals _pjf.Id
                                                             select _vs;
        int _i = await q.CountAsync(cancellationToken: cancellationToken);
        if (_i == 0)
            return ResponseBaseModel.CreateSuccess("Значений нет (удалить нечего)");

        context_forms.RemoveRange(q);
        await context_forms.SaveChangesAsync(cancellationToken);

        return ResponseBaseModel.CreateSuccess($"Удалено значений: {_i}");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteSessionDocumentAsync(DeleteSessionDocumentRequestModel req, CancellationToken cancellationToken = default)
    {
        int session_id = req.DeleteSessionDocumentId;
        using ConstructorContext context_forms = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        SessionOfDocumentDataModelDB? session_db = await context_forms
            .Sessions
            .Where(x => x.Id == session_id)
            .Include(x => x.DataSessionValues)

            .Include(s => s.Owner) // опрос/анкета
            .ThenInclude(x => x!.Tabs!) // страницы опроса/анкеты
            .ThenInclude(x => x.JoinsForms!) // формы для страницы опроса/анкеты
            .ThenInclude(x => x.Form) // форма
            .ThenInclude(x => x!.Fields) // поля

            .Include(s => s.Owner) // опрос/анкета
            .ThenInclude(x => x!.Tabs!) // страницы опроса/анкеты
            .ThenInclude(x => x.JoinsForms!) // формы для страницы опроса/анкеты
            .ThenInclude(x => x.Form) // форма
            .ThenInclude(x => x!.FieldsDirectoriesLinks) // поля
            .AsSingleQuery()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (session_db is null)
            return ResponseBaseModel.CreateError($"Сессия #{session_id} не найдена в БД");

        context_forms.Remove(session_db);
        await context_forms.SaveChangesAsync(cancellationToken);

        string json_string = JsonConvert.SerializeObject(session_db, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        logger.LogWarning($"удаление сессии: {json_string}");

        return ResponseBaseModel.CreateSuccess($"Сессия #{session_id} успешно удалена из БД");
    }

}