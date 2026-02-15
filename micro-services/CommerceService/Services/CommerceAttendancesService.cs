////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Attendance
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    #region records
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<RecordsAttendanceModelDB> q = context
            .AttendancesReg
            .Where(x => x.ContextName == req.Payload.ContextName)
            .AsQueryable();

        if (req.Payload.AfterDateUpdate.HasValue)
            q = q.Where(x => x.DateExecute >= DateOnly.FromDateTime(req.Payload.AfterDateUpdate.Value));

        if (req.Payload.OfferFilter is not null && req.Payload.OfferFilter.Length != 0)
            q = q.Where(x => req.Payload.OfferFilter.Any(i => i == x.OfferId));

        if (req.Payload.NomenclatureFilter is not null && req.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => req.Payload.NomenclatureFilter.Any(i => i == x.NomenclatureId));

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.AfterDateUpdate || (x.LastUpdatedAtUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.AfterDateUpdate));

        IOrderedQueryable<RecordsAttendanceModelDB> oq = req.SortingDirection == DirectionsEnum.Up
           ? q.OrderBy(x => x.StartPart).ThenByDescending(x => x.LastUpdatedAtUTC)
           : q.OrderByDescending(x => x.StartPart).ThenByDescending(x => x.LastUpdatedAtUTC);

        IQueryable<RecordsAttendanceModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<RecordsAttendanceModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = req.Payload.IncludeExternalData ? [.. await inc_query.ToArrayAsync(cancellationToken: token)] : [.. await pq.ToArrayAsync(cancellationToken: token)]
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AttendanceRecordsCreateAsync(TAuthRequestStandardModel<CreateAttendanceRequestModel> workSchedules, CancellationToken token = default)
    {
        if (workSchedules.Payload is null)
            return ResponseBaseModel.CreateError("workSchedules.Payload is null");

        if (string.IsNullOrWhiteSpace(workSchedules.SenderActionUserId))
            return ResponseBaseModel.CreateError("string.IsNullOrWhiteSpace(workSchedules.SenderActionUserId)");

        List<WorkScheduleBaseModel> records = workSchedules.Payload.Records;
        ResponseBaseModel res = new();
        records.ForEach(x =>
        {
            ValidateReportModel ck = GlobalTools.ValidateObject(x);
            if (!ck.IsValid)
                res.Messages.InjectException(ck.ValidationResults);
        });
        string msg, waMsg;
        if (!res.Success())
        {
            msg = $"Ошибка запроса: {res.Message()}";
            loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(workSchedules, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        TResponseModel<UserInfoModel[]> actorRes = await identityRepo.GetUsersOfIdentityAsync([workSchedules.SenderActionUserId], token);
        if (!actorRes.Success() || actorRes.Response is null || actorRes.Response.Length == 0)
        {
            res.AddRangeMessages(actorRes.Messages);
            return res;
        }

        UserInfoModel actor = actorRes.Response[0];

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        int nomenclatureId = await context.Offers.Where(x => x.Id == workSchedules.Payload.OfferId).Select(x => x.NomenclatureId).FirstAsync(cancellationToken: token);

        List<RecordsAttendanceModelDB> recordsForAdd = [.. records.Select(x => new RecordsAttendanceModelDB()
        {
            AuthorIdentityUserId = workSchedules.SenderActionUserId,
            ContextName = Routes.ATTENDANCES_CONTROLLER_NAME,
            DateExecute = x.Date,
            StartPart = TimeOnly.FromTimeSpan(x.StartPart),
            EndPart = TimeOnly.FromTimeSpan(x.EndPart),
            CreatedAtUTC = DateTime.UtcNow,
            LastUpdatedAtUTC = DateTime.UtcNow,
            OfferId = workSchedules.Payload.OfferId,
            NomenclatureId = nomenclatureId,
            OrganizationId = x.Organization.Id,
            Version = Guid.NewGuid(),
            StatusDocument = StatusesDocumentsEnum.Created,
            Name = "Новая запись"
        })];

        LockTransactionModelDB[] offersLocked = [.. recordsForAdd
            .Select(x => new LockTransactionModelDB()
            {
                LockerName = $"{nameof(RecordsAttendanceModelDB)} /{x.DateExecute}: {x.StartPart}-{x.EndPart}",
                LockerId = x.OfferId,
                LockerAreaId = x.OrganizationId,
                 Marker = nameof(AttendanceRecordsCreateAsync),
            })];
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);

        try
        {
            await context.LockTransactions.AddRangeAsync(offersLocked);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(AttendanceRecordsCreateAsync)}: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(workSchedules, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        WorkFindRequestModel req = new()
        {
            OffersFilter = [workSchedules.Payload.OfferId],
            ContextName = Routes.ATTENDANCES_CONTROLLER_NAME,
            StartDate = records.Min(x => x.Date),
            EndDate = records.Max(x => x.Date),
        };

        List<WorkScheduleModel> WorksSchedulesViews = default!;
        TResponseModel<int?> res_RubricIssueForCreateOrder = default!;
        TResponseModel<string?>? CommerceNewOrderSubjectNotification = null, CommerceNewOrderBodyNotification = null, CommerceNewOrderBodyNotificationTelegram = null;

        List<Task> tasks = [
                Task.Run(async () => { CommerceNewOrderSubjectNotification = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderSubjectNotification, token); }, token),
                Task.Run(async () => { CommerceNewOrderBodyNotification = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderBodyNotification, token); }, token),
                Task.Run(async () => { CommerceNewOrderBodyNotificationTelegram = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderBodyNotificationTelegram, token); }, token),
                Task.Run(async () => { res_RubricIssueForCreateOrder = await StorageTransmissionRepo.ReadParameterAsync<int?>(GlobalStaticCloudStorageMetadata.RubricIssueForCreateAttendanceOrder, token); }, token),
                Task.Run(async () =>
                {
                    WorksFindResponseModel get_balance = await WorksSchedulesFindAsync(req, [.. recordsForAdd.Select(x => x.OrganizationId).Distinct()]);
                    WorksSchedulesViews = get_balance.WorksSchedulesViews;
                }, token)
            ];

        if (string.IsNullOrWhiteSpace(_webConf.ClearBaseUri))
        {
            tasks.Add(Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(_webConf.ClearBaseUri))
                {
                    TelegramBotConfigModel wc = await webTransmissionRepo.GetWebConfigAsync();
                    _webConf.BaseUri = wc.ClearBaseUri;
                }
            }, token));
        }

        await Task.WhenAll(tasks);

        tasks.Clear();

        foreach (IGrouping<int, WorkScheduleModel> rec in records.GroupBy(x => x.Organization.Id))
        {
            List<WorkScheduleModel> b_crop_list = [.. WorksSchedulesViews.Where(x => x.Organization.Id == rec.Key)];

            foreach (WorkScheduleModel subNode in rec)
            {
                int cbInd = b_crop_list.FindIndex(x => x == subNode);
                if (cbInd < 0)
                    res.AddError($"Не хватает слота: {subNode}! Удалите или измените данную запись");
                else if (b_crop_list[cbInd].QueueCapacity == 1)
                    b_crop_list.RemoveAt(cbInd);
                else if (b_crop_list[cbInd].QueueCapacity > 1)
                    b_crop_list[cbInd].QueueCapacity--;
            }
        }

        if (!res.Success())
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось сформировать резерв: ";
            loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(workSchedules, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        TAuthRequestStandardModel<UniversalUpdateRequestModel> issue_new = new()
        {
            SenderActionUserId = workSchedules.SenderActionUserId,
            Payload = new()
            {
                Name = "Услуга",
                ParentId = res_RubricIssueForCreateOrder.Response,
                Description = "Услуга",
            },
        };

        TResponseModel<int> issue = await HelpDeskRepo.IssueCreateOrUpdateAsync(issue_new, token);
        if (!issue.Success())
        {
            await transaction.RollbackAsync(token);
            res.Messages.AddRange(issue.Messages);
            return res;
        }

        recordsForAdd.ForEach(x => x.HelpDeskId = issue.Response);

        await context.AttendancesReg.AddRangeAsync(recordsForAdd, token);
        await context.SaveChangesAsync(token);

        PulseRequestModel reqPulse = new()
        {
            Payload = new()
            {
                Payload = new()
                {
                    Description = $"Бронь: {string.Join(";", recordsForAdd.Select(x => x.ToString()))};",
                    IssueId = issue.Response,
                    PulseType = PulseIssuesTypesEnum.OrderAttendance,
                    Tag = Routes.CREATE_ACTION_NAME,
                },
                SenderActionUserId = workSchedules.SenderActionUserId,
            }
        };
        await HelpDeskRepo.PulsePushAsync(reqPulse, false, token);

        string subject_email = "Создана новая бронь";
        DateTime _dt = DateTime.UtcNow.GetCustomTime();
        string _dtAsString = $"{_dt.ToString("d", GlobalStaticConstants.RU)} {_dt.ToString("t", GlobalStaticConstants.RU)}";
        string _about_order = $"Новая бронь {_dtAsString}";

        if (CommerceNewOrderSubjectNotification?.Success() == true && !string.IsNullOrWhiteSpace(CommerceNewOrderSubjectNotification.Response))
            subject_email = CommerceNewOrderSubjectNotification.Response;

        subject_email = IHelpDeskService.ReplaceTags(subject_email, _dt, issue.Response, StatusesDocumentsEnum.Created, subject_email, _webConf.ClearBaseUri, _about_order);
        res.AddSuccess(subject_email);
        msg = $"<p>Заказ <b>'{issue_new.Payload.Name}' от [{_dtAsString}]</b> успешно создан.</p>" +
                $"<p>/<a href='{_webConf.ClearBaseUri}'>{_webConf.ClearBaseUri}</a>/</p>";
        string msg_for_tg = msg.Replace("<p>", "").Replace("</p>", "");

        waMsg = $"Заказ '{issue_new.Payload.Name}' от [{_dtAsString}] успешно создан.\n{_webConf.ClearBaseUri}";

        if (CommerceNewOrderBodyNotification?.Success() == true && !string.IsNullOrWhiteSpace(CommerceNewOrderBodyNotification.Response))
            msg = CommerceNewOrderBodyNotification.Response;
        msg = IHelpDeskService.ReplaceTags(msg, _dt, issue.Response, StatusesDocumentsEnum.Created, msg, _webConf.ClearBaseUri, _about_order);

        if (CommerceNewOrderBodyNotificationTelegram?.Success() == true && !string.IsNullOrWhiteSpace(CommerceNewOrderBodyNotificationTelegram.Response))
            msg_for_tg = CommerceNewOrderBodyNotificationTelegram.Response;
        msg_for_tg = IHelpDeskService.ReplaceTags(msg_for_tg, _dt, issue.Response, StatusesDocumentsEnum.Created, msg_for_tg, _webConf.ClearBaseUri, _about_order);

        tasks = [identityRepo.SendEmailAsync(new() { Email = actor.Email!, Subject = subject_email, TextMessage = msg }, false, token)];

        if (actor.TelegramId.HasValue)
            tasks.Add(tgRepo.SendTextMessageTelegramAsync(new() { Message = msg_for_tg, UserTelegramId = actor.TelegramId!.Value }, false, token));

        if (!string.IsNullOrWhiteSpace(actor.PhoneNumber) && GlobalTools.IsPhoneNumber(actor.PhoneNumber!))
        {
            tasks.Add(Task.Run(async () =>
            {
                TResponseModel<string?> CommerceNewOrderBodyNotificationWhatsapp = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderBodyNotificationWhatsapp);
                if (CommerceNewOrderBodyNotificationWhatsapp.Success() && !string.IsNullOrWhiteSpace(CommerceNewOrderBodyNotificationWhatsapp.Response))
                    waMsg = CommerceNewOrderBodyNotificationWhatsapp.Response;

                await tgRepo.SendWappiMessageAsync(new() { Number = actor.PhoneNumber!, Text = IHelpDeskService.ReplaceTags(waMsg, _dt, issue.Response, StatusesDocumentsEnum.Created, waMsg, _webConf.ClearBaseUri, _about_order, true) }, false);
            }, token));
        }

        loggerRepo.LogInformation(msg_for_tg);
        context.LockTransactions.RemoveRange(offersLocked);

        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        res.AddSuccess("Ok");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> AttendanceRecordsDeleteAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(req.SenderActionUserId))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "string.IsNullOrEmpty(req.SenderActionUserId)" }] };

        if (req.Payload is null || req.Payload.Length == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null || req.Payload.Length == 0" }] };

        UserInfoModel actor = default!;

        TResponseModel<RecordsAttendanceModelDB[]> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IIncludableQueryable<RecordsAttendanceModelDB, OfferModelDB> qr = context.AttendancesReg
            .Where(x => req.Payload.Contains(x.Id))
            .Include(x => x.Organization!)
            .Include(x => x.Offer!);

        await Task.WhenAll([
            Task.Run(async () => { res.Response = await qr.ToArrayAsync(token); }, token),
            Task.Run(async () => {
                TResponseModel<UserInfoModel[]> actorRes = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]);
                if (!actorRes.Success() || actorRes.Response is null || actorRes.Response.Length != 1)
                {
                    res.AddRangeMessages(actorRes.Messages);
                    res.AddError($"Пользователь `{req.SenderActionUserId}` не найден в БД");
                }
                else
                    actor = actorRes.Response[0];
             }, token)]);

        if (!res.Success() || res.Response is null || res.Response.Length == 0)
            return res;

        foreach (RecordsAttendanceModelDB orderAttendanceDB in res.Response)
        {
            if (orderAttendanceDB.AuthorIdentityUserId == req.SenderActionUserId || actor.IsAdmin || actor.Roles?.Contains(GlobalStaticConstantsRoles.Roles.System) == true)
            {
                context.Remove(orderAttendanceDB);
                await context.SaveChangesAsync(token);
                res.AddSuccess("Запись успешно удалена");

                if (orderAttendanceDB.HelpDeskId.HasValue)
                {
                    PulseRequestModel reqPulse = new()
                    {
                        Payload = new()
                        {
                            Payload = new()
                            {
                                Description = $"Запись удалена - {orderAttendanceDB}",
                                IssueId = orderAttendanceDB.HelpDeskId.Value,
                                PulseType = PulseIssuesTypesEnum.OrderAttendance,
                                Tag = Routes.DELETE_ACTION_NAME,
                            },
                            SenderActionUserId = req.SenderActionUserId,
                        }
                    };

                    await HelpDeskRepo.PulsePushAsync(reqPulse, false, token);
                }
            }
            else
                res.AddError($"Недостаточно прав для удаления `{orderAttendanceDB}`");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RecordsAttendanceModelDB>>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default)
    {
        TResponseModel<List<RecordsAttendanceModelDB>> res = new();

        if (string.IsNullOrWhiteSpace(req.SenderActionUserId) || req.Payload is null)
        {
            res.AddError($"`{nameof(req.SenderActionUserId)}` not set (or `{nameof(req.Payload)}`) is null");
            return res;
        }

        TResponseModel<UserInfoModel[]> actorRes = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId], token);
        if (!actorRes.Success() || actorRes.Response is null || actorRes.Response.Length == 0)
        {
            res.AddRangeMessages(actorRes.Messages);
            return res;
        }
        UserInfoModel actor = actorRes.Response[0];
        string msg;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        res.Response = await context
            .AttendancesReg
            .Where(x => x.HelpDeskId == req.Payload.DocumentId && x.StatusDocument != req.Payload.Step)
            .ToListAsync(cancellationToken: token);

        if (res.Response.Count == 0)
        {
            msg = "Изменение не требуется (документы для обновления отсутствуют)";
            loggerRepo.LogInformation($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddInfo($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию объекта");
            return res;
        }

        LockTransactionModelDB[] offersLocked = [.. res.Response
           .Select(x => new LockTransactionModelDB()
           {
               LockerName = $"{nameof(RecordsAttendanceModelDB)} /{x.DateExecute}: {x.StartPart}-{x.EndPart}",
               LockerId = x.OfferId,
               LockerAreaId = x.OrganizationId,
               Marker = nameof(StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync),
           })];

        using IDbContextTransaction transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
        try
        {
            await context.LockTransactions.AddRangeAsync(offersLocked);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync)}: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }

        PulseRequestModel reqPulse = new()
        {
            Payload = new()
            {
                Payload = new()
                {
                    Description = "",
                    IssueId = req.Payload.DocumentId,
                    PulseType = PulseIssuesTypesEnum.OrderAttendance,
                    Tag = Routes.CHANGE_ACTION_NAME,
                },
                SenderActionUserId = req.SenderActionUserId,
            }
        };

        if (req.Payload.Step == StatusesDocumentsEnum.Canceled)
        {
            res.Response.ForEach(x => x.StatusDocument = StatusesDocumentsEnum.Canceled);
            context.UpdateRange(res.Response);
            //
            reqPulse.Payload.Payload.Description += $"Отмена брони: {string.Join(";", res.Response.Select(x => x.ToString()))};";
            reqPulse.Payload.Payload.Tag = Routes.CANCEL_ACTION_NAME;
        }
        else
        {
            WorkFindRequestModel get_balance_req = new()
            {
                OffersFilter = res.Response.Select(x => x.OfferId).Distinct().ToArray(),
                ContextName = Routes.ATTENDANCES_CONTROLLER_NAME,
                StartDate = res.Response.Min(x => x.DateExecute),
                EndDate = res.Response.Max(x => x.DateExecute),
            };
            WorksFindResponseModel get_balance = await WorksSchedulesFindAsync(get_balance_req, res.Response.Select(x => x.OrganizationId).Distinct().ToArray(), token);

            foreach (IGrouping<int, WorkScheduleModel> rec in res.Response.GroupBy(x => x.OrganizationId))
            {
                List<WorkScheduleModel> b_crop_list = [.. get_balance.WorksSchedulesViews.Where(x => x.Organization.Id == rec.Key)];

                foreach (WorkScheduleModel subNode in rec)
                {
                    int cbInd = b_crop_list.FindIndex(x => x == subNode);
                    if (cbInd < 0)
                        res.AddError($"Не хватает слота (не возможно восстановить бронь): {subNode}! Удалите или измените данную запись");
                    else if (b_crop_list[cbInd].QueueCapacity == 1)
                        b_crop_list.RemoveAt(cbInd);
                    else if (b_crop_list[cbInd].QueueCapacity > 1)
                        b_crop_list[cbInd].QueueCapacity--;
                }
            }

            if (!res.Success())
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось сформировать резерв:";
                loggerRepo.LogError($"{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}\n{JsonConvert.SerializeObject(res, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
                return res;
            }

            reqPulse.Payload.Payload.Description += $"Восстановление записей/брони: {string.Join(";", res.Response.Select(x => x.ToString()))};";
            reqPulse.Payload.Payload.Tag = Routes.SET_ACTION_NAME;
        }
        await HelpDeskRepo.PulsePushAsync(reqPulse, false, token);
        context.LockTransactions.RemoveRange(offersLocked);
        await context.SaveChangesAsync(token);
        int _rc = await context
                            .AttendancesReg
                            .Where(x => x.HelpDeskId == req.Payload.DocumentId)
                            .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.StatusDocument, req.Payload.Step)
                            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
                            .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        res.AddSuccess($"Запрос смены статуса заказа услуг {(_rc == 0 ? "[не требуется]" : "[выполнен успешно]")}");

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> RecordsAttendancesByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
    {
        if (req.IssueIds.Length == 0)
            return new()
            {
                Response = [],
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Запрос не может быть пустым" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RecordsAttendanceModelDB> q = context
            .AttendancesReg
            .Where(x => req.IssueIds.Any(y => y == x.HelpDeskId))
            .AsQueryable();

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<RecordsAttendanceModelDB, NomenclatureModelDB?> inc_query = q
            .Include(x => x.Organization)
            .Include(x => x.Offer!)
            .Include(x => x.Nomenclature);

        return new()
        {
            Response = req.IncludeExternalData
            ? [.. await inc_query.ToArrayAsync(cancellationToken: token)]
            : [.. await q.ToArrayAsync(cancellationToken: token)],
        };
    }
    #endregion

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel> paginationWorkSchedulesRequest, CancellationToken token = default)
    {
        if (paginationWorkSchedulesRequest.Payload is null)
        {
            loggerRepo.LogError($"{nameof(paginationWorkSchedulesRequest)}.{nameof(paginationWorkSchedulesRequest.Payload)} is null");
            return new();
        }

        if (paginationWorkSchedulesRequest.PageSize < 10)
            paginationWorkSchedulesRequest.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WeeklyScheduleModelDB> q = context
            .WeeklySchedules.Where(x => x.ContextName == paginationWorkSchedulesRequest.Payload.ContextName);

        if (paginationWorkSchedulesRequest.Payload.OfferFilter is not null && paginationWorkSchedulesRequest.Payload.OfferFilter.Length != 0)
            q = q.Where(x => paginationWorkSchedulesRequest.Payload.OfferFilter.Any(i => i == x.OfferId));
        else
            q = q.Where(x => x.OfferId == null || x.OfferId == 0);

        if (paginationWorkSchedulesRequest.Payload.NomenclatureFilter is not null && paginationWorkSchedulesRequest.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => paginationWorkSchedulesRequest.Payload.NomenclatureFilter.Any(i => i == x.NomenclatureId));

        if (paginationWorkSchedulesRequest.Payload.Weekdays is not null && paginationWorkSchedulesRequest.Payload.Weekdays.Length != 0)
            q = q.Where(x => paginationWorkSchedulesRequest.Payload.Weekdays.Any(y => y == x.Weekday));

        if (paginationWorkSchedulesRequest.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= paginationWorkSchedulesRequest.Payload.AfterDateUpdate || (x.LastUpdatedAtUTC == DateTime.MinValue && x.CreatedAtUTC >= paginationWorkSchedulesRequest.Payload.AfterDateUpdate));

        IOrderedQueryable<WeeklyScheduleModelDB> oq = paginationWorkSchedulesRequest.SortingDirection == DirectionsEnum.Up
           ? q.OrderBy(x => x.StartPart).ThenByDescending(x => x.LastUpdatedAtUTC)
           : q.OrderByDescending(x => x.StartPart).ThenByDescending(x => x.LastUpdatedAtUTC);

        IQueryable<WeeklyScheduleModelDB> pq = oq
            .Skip(paginationWorkSchedulesRequest.PageNum * paginationWorkSchedulesRequest.PageSize)
            .Take(paginationWorkSchedulesRequest.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WeeklyScheduleModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature);

        return new()
        {
            PageNum = paginationWorkSchedulesRequest.PageNum,
            PageSize = paginationWorkSchedulesRequest.PageSize,
            SortingDirection = paginationWorkSchedulesRequest.SortingDirection,
            SortBy = paginationWorkSchedulesRequest.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = paginationWorkSchedulesRequest.Payload.IncludeExternalData ? [.. await inc_query.ToArrayAsync(cancellationToken: token)] : [.. await pq.ToArrayAsync(cancellationToken: token)]
        };
    }

    /// <inheritdoc/>
    public async Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, int[]? organizationsFilter = null, CancellationToken token = default)
    {
        List<DayOfWeek> weeks = [];
        List<DateOnly> dates = [];

        for (DateOnly dt = req.StartDate; dt <= req.EndDate; dt = dt.AddDays(1))
        {
            dates.Add(dt);
            if (weeks.Count != 7 && !weeks.Contains(dt.DayOfWeek))
                weeks.Add(dt.DayOfWeek);
        }

        List<WeeklyScheduleModelDB> WeeklySchedules = default!;
        List<CalendarScheduleModelDB> CalendarsSchedules = default!;
        List<RecordsAttendanceModelDB> OrdersAttendances = default!;
        List<OrganizationContractorModel> OrganizationsContracts = default!;

        await Task.WhenAll([
            Task.Run(async ()=> {
                using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();
                WeeklySchedules = await context.WeeklySchedules
                    .Where(x => !x.IsDisabled && x.ContextName == req.ContextName && (x.OfferId == null || req.OffersFilter.Any(y => y == x.OfferId)) && weeks.Contains(x.Weekday))
                    .ToListAsync();
             }, token),
            Task.Run(async ()=> {
                using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();
                CalendarsSchedules = await context.CalendarsSchedules
                    .Where(x => !x.IsDisabled && x.ContextName == req.ContextName && dates.Contains(x.DateScheduleCalendar))
                    .ToListAsync();
            }, token),
             Task.Run(async ()=> {
                using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();

                IQueryable<RecordsAttendanceModelDB> q = context
                    .AttendancesReg
                    .Where(x => x.ContextName == req.ContextName && x.DateExecute >= req.StartDate && x.DateExecute <= req.EndDate)
                    .Where(x => req.OffersFilter.Any(y => y == x.OfferId));

                if(organizationsFilter is not null && organizationsFilter.Length != 0)
                     q = q.Where(x => organizationsFilter.Any(y => y == x.OrganizationId));

                OrdersAttendances = await q
                    .Include(x => x.Offer!)
                    .ThenInclude(x => x.Nomenclature)
                    .ToListAsync();
            }, token),
            Task.Run(async ()=> {
                using CommerceContext context = await commerceDbFactory.CreateDbContextAsync();

                IQueryable<OrganizationContractorModel> q = context
                    .Contractors
                    .Where(x => x.OfferId == null || req.OffersFilter.Any(y => y == x.OfferId));

                if(organizationsFilter is not null && organizationsFilter.Length != 0)
                     q = q.Where(x => organizationsFilter.Any(y => y == x.OrganizationId));

                OrganizationsContracts = await q
                    .Include(x => x.Offer)
                    .Include(x => x.Organization!)
                    .ToListAsync();
            }, token)
        ]);

        return new WorksFindResponseModel(req.StartDate, req.EndDate, WeeklySchedules, CalendarsSchedules, OrganizationsContracts, OrdersAttendances);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WeeklyScheduleCreateOrUpdateAsync(TAuthRequestStandardModel<WeeklyScheduleModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        WeeklyScheduleModelDB weklySchedule = req.Payload;

        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        weklySchedule.Name = weklySchedule.Name.Trim();
        weklySchedule.Description = weklySchedule.Description?.Trim();
        weklySchedule.NormalizedNameUpper = weklySchedule.Name.ToUpper();
        weklySchedule.LastUpdatedAtUTC = DateTime.UtcNow;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        WeeklyScheduleModelDB? doubleWeeklySchedule = await context
            .WeeklySchedules
            .Where(x => x.Id != weklySchedule.Id && x.OfferId == weklySchedule.OfferId && x.Weekday == weklySchedule.Weekday)
            .Where(x => (weklySchedule.StartPart > x.StartPart && weklySchedule.StartPart < x.EndPart) || (weklySchedule.EndPart > x.StartPart && weklySchedule.EndPart < x.EndPart) ||
                                         (x.StartPart > weklySchedule.StartPart && x.StartPart < weklySchedule.EndPart) || (x.EndPart > weklySchedule.StartPart && x.EndPart < weklySchedule.EndPart))
            .FirstOrDefaultAsync(cancellationToken: token);

        if (doubleWeeklySchedule is not null)
        {
            res.AddError($"Конфликт с существующей записью: {doubleWeeklySchedule.Name} [{doubleWeeklySchedule.StartPart} - {doubleWeeklySchedule.EndPart}]");
            return res;
        }

        if (weklySchedule.Id < 1)
        {
            weklySchedule.IsDisabled = true;
            weklySchedule.Id = 0;
            weklySchedule.CreatedAtUTC = DateTime.UtcNow;
            await context.WeeklySchedules.AddAsync(weklySchedule, token);
            await context.SaveChangesAsync(token);
            res.Response = weklySchedule.Id;
        }
        else
        {
            res.Response = await context.WeeklySchedules
                .Where(w => w.Id == weklySchedule.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.NormalizedNameUpper, weklySchedule.NormalizedNameUpper)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
                .SetProperty(p => p.Description, weklySchedule.Description)
                .SetProperty(p => p.IsDisabled, weklySchedule.IsDisabled)
                .SetProperty(p => p.StartPart, weklySchedule.StartPart)
                .SetProperty(p => p.EndPart, weklySchedule.EndPart)
                .SetProperty(p => p.QueueCapacity, weklySchedule.QueueCapacity)
                .SetProperty(p => p.Name, weklySchedule.Name), cancellationToken: token);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WeeklyScheduleModelDB> q = context
            .WeeklySchedules
            .Where(x => req.Any(y => x.Id == y));

        return await q
            .Include(x => x.Offer!)
            .Include(x => x.Nomenclature)
            .ToListAsync(cancellationToken: token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CalendarScheduleUpdateOrCreateAsync(TAuthRequestStandardModel<CalendarScheduleModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        CalendarScheduleModelDB calendarSchedule = req.Payload;
        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        calendarSchedule.Name = calendarSchedule.Name.Trim();
        calendarSchedule.Description = calendarSchedule.Description?.Trim();
        calendarSchedule.NormalizedNameUpper = calendarSchedule.Name.ToUpper();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        CalendarScheduleModelDB? doubleCalendarSchedule = await context
            .CalendarsSchedules
            .Where(x => x.Id != calendarSchedule.Id && x.OfferId == calendarSchedule.OfferId && x.DateScheduleCalendar == calendarSchedule.DateScheduleCalendar)
            .Where(x => (calendarSchedule.StartPart > x.StartPart && calendarSchedule.StartPart < x.EndPart) || (calendarSchedule.EndPart > x.StartPart && calendarSchedule.EndPart < x.EndPart) ||
                                         (x.StartPart > calendarSchedule.StartPart && x.StartPart < calendarSchedule.EndPart) || (x.EndPart > calendarSchedule.StartPart && x.EndPart < calendarSchedule.EndPart))
            .FirstOrDefaultAsync(cancellationToken: token);

        if (doubleCalendarSchedule is not null)
        {
            res.AddError($"Конфликт с существующей записью: {doubleCalendarSchedule.Name} [{doubleCalendarSchedule.StartPart} - {doubleCalendarSchedule.EndPart}]");
            return res;
        }

        if (calendarSchedule.Id < 1)
        {
            calendarSchedule.IsDisabled = true;
            calendarSchedule.Id = 0;
            calendarSchedule.CreatedAtUTC = DateTime.UtcNow;
            calendarSchedule.LastUpdatedAtUTC = DateTime.UtcNow;

            await context.CalendarsSchedules.AddAsync(calendarSchedule, token);
            await context.SaveChangesAsync(token);
            res.Response = calendarSchedule.Id;
        }
        else
        {
            res.Response = await context.CalendarsSchedules
                .Where(x => x.Id == calendarSchedule.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, calendarSchedule.Name)
                .SetProperty(p => p.Description, calendarSchedule.Description)
                .SetProperty(p => p.IsDisabled, calendarSchedule.IsDisabled)
                .SetProperty(p => p.QueueCapacity, calendarSchedule.QueueCapacity)
                .SetProperty(p => p.EndPart, calendarSchedule.EndPart)
                .SetProperty(p => p.StartPart, calendarSchedule.StartPart)
                .SetProperty(p => p.DateScheduleCalendar, calendarSchedule.DateScheduleCalendar)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
                .SetProperty(p => p.NormalizedNameUpper, calendarSchedule.NormalizedNameUpper), cancellationToken: token);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default)
    {
        if (req.Payload?.Payload is null)
        {
            return new()
            {
                Messages = [new() { Text = "req.Payload?.Payload is null", TypeMessage = MessagesTypesEnum.Error }]
            };
        }

        if (req.Payload.PageSize < 10)
            req.Payload.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DateOnly _dtp = DateOnly.FromDateTime(DateTime.UtcNow);

        IQueryable<CalendarScheduleModelDB> q = context
            .CalendarsSchedules
            .Where(x => !req.Payload.Payload.ActualOnly || x.DateScheduleCalendar >= _dtp);

        if (req.Payload.Payload.OfferFilter is not null && req.Payload.Payload.OfferFilter.Length != 0)
            q = q.Where(x => req.Payload.Payload.OfferFilter.Any(i => i == x.OfferId));

        if (req.Payload.Payload.NomenclatureFilter is not null && req.Payload.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => req.Payload.Payload.NomenclatureFilter.Any(i => i == x.NomenclatureId));

        if (req.Payload.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.Payload.AfterDateUpdate || (x.LastUpdatedAtUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.Payload.AfterDateUpdate));

        IOrderedQueryable<CalendarScheduleModelDB> oq = req.Payload.SortingDirection == DirectionsEnum.Up
           ? q.OrderBy(x => x.DateScheduleCalendar).ThenBy(x => x.StartPart).ThenByDescending(x => x.LastUpdatedAtUTC)
           : q.OrderByDescending(x => x.DateScheduleCalendar).ThenByDescending(x => x.StartPart).ThenByDescending(x => x.LastUpdatedAtUTC);

        IQueryable<CalendarScheduleModelDB> pq = oq
            .Skip(req.Payload.PageNum * req.Payload.PageSize)
            .Take(req.Payload.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<CalendarScheduleModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Offer)
            .Include(x => x.Nomenclature);

        CalendarScheduleModelDB[] res = req.Payload.Payload.IncludeExternalData
            ? await inc_query.ToArrayAsync(cancellationToken: token)
            : await pq.ToArrayAsync(cancellationToken: token);

        return new()
        {
            Response = new()
            {
                PageNum = req.Payload.PageNum,
                PageSize = req.Payload.PageSize,
                SortingDirection = req.Payload.SortingDirection,
                SortBy = req.Payload.SortBy,
                TotalRowsCount = await q.CountAsync(cancellationToken: token),
                Response = [.. res]
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        TResponseModel<List<CalendarScheduleModelDB>> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<CalendarScheduleModelDB> q = context
            .CalendarsSchedules
            .Where(x => req.Payload.Any(y => x.Id == y));

        return new()
        {
            Response = await q
            .Include(x => x.Offer!)
            .Include(x => x.Nomenclature)
            .ToListAsync(cancellationToken: token)
        };
    }
}