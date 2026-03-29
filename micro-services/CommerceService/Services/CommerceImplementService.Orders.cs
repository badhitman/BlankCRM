////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml;
using Newtonsoft.Json;
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
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
    {
        if (req.IssueIds.Length == 0)
            return new()
            {
                Response = [],
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Запрос не может быть пустым" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OrderDocumentModelDB> q = context
            .OrdersB2B
            .Where(x => req.IssueIds.Any(y => y == x.HelpDeskId))
            .AsQueryable();

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<OrderDocumentModelDB, NomenclatureModelDB?> inc_query = q
            .Include(x => x.Organization)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Office)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Rows!)
            .ThenInclude(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature);

        return new()
        {
            Response = req.IncludeExternalData
            ? [.. await inc_query.ToArrayAsync(cancellationToken: token)]
            : [.. await q.ToArrayAsync(cancellationToken: token)],
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        TResponseModel<OrderDocumentModelDB[]> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OrderDocumentModelDB> q = context
            .OrdersB2B
            .Where(x => req.Payload.Any(y => x.Id == y));

        res.Response = await q
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Office)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Rows!)
            .ThenInclude(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req, CancellationToken token = default)
    {
        if (req.Payload?.Payload is null)
        {
            loggerRepo.LogError("req.Payload?.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OrderDocumentModelDB> q = context
            .OrdersB2B
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.Payload.SenderActionUserId) && !req.Payload.SenderActionUserId.Equals(GlobalStaticConstantsRoles.Roles.System))
            q = q.Where(x => x.AuthorIdentityUserId == req.Payload.SenderActionUserId);

        if (req.Payload.Payload.OrganizationFilter.HasValue && req.Payload.Payload.OrganizationFilter.Value != 0)
            q = q.Where(x => x.OrganizationId == req.Payload.Payload.OrganizationFilter);

        if (req.Payload.Payload.AddressForOrganizationFilter.HasValue && req.Payload.Payload.AddressForOrganizationFilter.Value != 0)
            q = q.Where(x => context.OfficesForOrders.Any(y => y.OrderId == x.Id && y.OfficeId == req.Payload.Payload.AddressForOrganizationFilter));

        if (req.Payload.Payload.OfferFilter is not null && req.Payload.Payload.OfferFilter.Length != 0)
            q = q.Where(x => context.RowsOrders.Any(y => y.OrderId == x.Id && req.Payload.Payload.OfferFilter.Any(i => i == y.OfferId)));

        if (req.Payload.Payload.NomenclatureFilter is not null && req.Payload.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => context.RowsOrders.Any(y => y.OrderId == x.Id && req.Payload.Payload.NomenclatureFilter.Any(i => i == y.NomenclatureId)));

        if (req.Payload.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.Payload.AfterDateUpdate || (x.LastUpdatedAtUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.Payload.AfterDateUpdate));

        if (req.Payload.Payload.StatusesFilter is not null && req.Payload.Payload.StatusesFilter.Length != 0)
            q = q.Where(x => req.Payload.Payload.StatusesFilter.Any(y => y == x.StatusDocument));

        IOrderedQueryable<OrderDocumentModelDB> oq = req.SortingDirection == DirectionsEnum.Up
           ? q.OrderBy(x => x.CreatedAtUTC)
           : q.OrderByDescending(x => x.CreatedAtUTC);

        IQueryable<OrderDocumentModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<OrderDocumentModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Organization)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Office)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Rows!)
            .ThenInclude(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = req.Payload.Payload.IncludeExternalData ? [.. await inc_query.ToArrayAsync(cancellationToken: token)] : [.. await pq.ToArrayAsync(cancellationToken: token)]
        };
    }

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> OrderUpdateOrCreateAsync(TAuthRequestStandardModel<OrderDocumentModelDB> req, CancellationToken token = default)
    {
        DocumentNewVersionResponseModel res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        OrderDocumentModelDB order = req.Payload;


        ValidateReportModel ck = GlobalTools.ValidateObject(order);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }
        order.Name = order.Name.Trim();

        TResponseModel<UserInfoModel[]> actor = await identityRepo.GetUsersOfIdentityAsync([order.AuthorIdentityUserId], token);
        if (!actor.Success() || actor.Response is null || actor.Response.Length == 0)
        {
            res.AddRangeMessages(actor.Messages);
            return res;
        }

        string msg, waMsg;
        DateTime dtu = DateTime.UtcNow;
        order.LastUpdatedAtUTC = dtu;

        OfferModelDB?[] allOffersReq = [.. order.OfficesTabs!
            .SelectMany(x => x.Rows!)
            .Select(x => x.Offer)
            .DistinctBy(x => x!.Id)];

        allOffersReq = GlobalTools.CreateDeepCopy(allOffersReq)!;

        List<Task> tasks;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: token);

        if (order.Id < 1)
        {
            if (order.OfficesTabs is null || order.OfficesTabs.Count == 0)
            {
                res.AddError($"В заказе отсутствуют адреса доставки");
                return res;
            }

            order.OfficesTabs.ForEach(x =>
            {
                if (x.Rows is null || x.Rows.Count == 0)
                    res.AddError($"Для адреса доставки '{x.Office?.Name}' не указана номенклатура");
                else if (x.Rows.Any(x => x.Quantity < 1))
                    res.AddError($"В адресе доставки '{x.Office?.Name}' есть номенклатура без количества");
                else if (x.Rows.Count != x.Rows.GroupBy(x => x.OfferId).Count())
                    res.AddError($"В адресе доставки '{x.Office?.Name}' ошибка в таблице товаров: оффер встречается более одного раза");

                if (x.WarehouseId < 1)
                    res.AddError($"В адресе доставки '{x.Office?.Name}' не корректный склад #{x.WarehouseId}");
            });
            if (!res.Success())
                return res;

            int[] rubricsIds = [.. order.OfficesTabs.Select(x => x.WarehouseId).Distinct()];
            TResponseModel<List<RubricStandardModel>> getRubrics = new();
            if (rubricsIds.Length != 0)
            {
                getRubrics = await RubricsRepo.RubricsGetAsync(rubricsIds, token);
                if (!getRubrics.Success())
                {
                    res.AddRangeMessages(getRubrics.Messages);
                    return res;
                }

                if (getRubrics.Response is null || getRubrics.Response.Count != rubricsIds.Length)
                {
                    res.AddError($"Некоторые склады (rubric`s) не найдены");
                    return res;
                }
            }
            res.DocumentNewVersion = Guid.NewGuid();
            order.Id = 0;
            order.CreatedAtUTC = dtu;
            order.LastUpdatedAtUTC = dtu;

            order.Version = res.DocumentNewVersion.Value;
            order.StatusDocument = StatusesDocumentsEnum.Created;

            var _offersOfDocument = order.OfficesTabs
                           .SelectMany(x => x.Rows!.Select(y => new { x.WarehouseId, Row = y }))
                           .ToArray();

            LockTransactionModelDB[] offersLocked = [.. _offersOfDocument.Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.Row.OfferId,
                LockerAreaId = x.WarehouseId,
                Marker = nameof(OrderUpdateOrCreateAsync),
            })];

            try
            {
                await context.LockTransactions.AddRangeAsync(offersLocked);
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось выполнить команду блокировки БД {nameof(OrderUpdateOrCreateAsync)}: ";
                loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(order, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}{ex.Message}");
                return res;
            }

            int[] _offersIds = [.. allOffersReq.Select(x => x!.Id)];
            List<OfferAvailabilityModelDB> registersOffersDb = default!;
            TResponseModel<int?> res_RubricIssueForCreateOrder = default!;
            TResponseModel<string?>
                CommerceNewOrderSubjectNotification = default!,
                CommerceNewOrderBodyNotification = default!,
                CommerceNewOrderBodyNotificationTelegram = default!;

            tasks = [
                Task.Run(async () => { CommerceNewOrderSubjectNotification = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderSubjectNotification); }, token),
                Task.Run(async () => { CommerceNewOrderBodyNotification = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderBodyNotification); }, token),
                Task.Run(async () => { CommerceNewOrderBodyNotificationTelegram = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderBodyNotificationTelegram); }, token),
                Task.Run(async () => { res_RubricIssueForCreateOrder = await StorageTransmissionRepo.ReadParameterAsync<int?>(GlobalStaticCloudStorageMetadata.RubricIssueForCreateOrder);}, token),
                Task.Run(async () => { registersOffersDb = await context.OffersAvailability.Where(x => _offersIds.Any(y => y == x.OfferId)).ToListAsync();}, token)];

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

            order.OfficesTabs.ForEach(tabAddr =>
            {
                tabAddr.Rows!.ForEach(rowDoc =>
                {
                    OfferAvailabilityModelDB? rowReg = registersOffersDb.FirstOrDefault(x => x.OfferId == rowDoc.OfferId && x.WarehouseId == tabAddr.WarehouseId);
                    OfferModelDB offerInfo = allOffersReq.First(x => x?.Id == rowDoc.OfferId)!;

                    if (rowReg is null)
                        res.AddError($"'{offerInfo.Name}' (склад: `{getRubrics.Response?.First(x => x.Id == tabAddr.WarehouseId).Name}`) нет в наличии");
                    else if (rowReg.Quantity < rowDoc.Quantity)
                        res.AddError($"'{offerInfo.Name}' (склад: `{getRubrics.Response?.First(x => x.Id == tabAddr.WarehouseId).Name}`) не достаточно. Текущий остаток: {rowReg.Quantity}");
                });
            });
            if (!res.Success())
            {
                await transaction.RollbackAsync(token);
                return res;
            }

            order.PrepareForSave();
            order.CreatedAtUTC = dtu;

            await context.OrdersB2B.AddAsync(order, token);
            await context.SaveChangesAsync(token);
            res.Response = order.Id;

            foreach (TabOfficeForOrderModelDb tabAddr in order.OfficesTabs)
            {
                foreach (RowOfOrderDocumentModelDB rowDoc in tabAddr.Rows!)
                {
                    OfferAvailabilityModelDB rowReg = registersOffersDb.First(x => x.OfferId == rowDoc.OfferId && x.WarehouseId == tabAddr.WarehouseId);
                    //OfferModelDB offerInfo = allOffersReq.First(x => x?.Id == rowDoc.OfferId)!;
                    rowReg.Quantity -= rowDoc.Quantity;
                    context.OffersAvailability.Update(rowReg);
                }
            }

            TAuthRequestStandardModel<UniversalUpdateRequestModel> issue_new = new()
            {
                SenderActionUserId = order.AuthorIdentityUserId,
                Payload = new()
                {
                    Name = order.Name,
                    ParentId = res_RubricIssueForCreateOrder.Response,
                    Description = $"Новый заказ.\n{order.Description}".Trim(),
                },
            };

            TResponseModel<int> issue = await HelpDeskRepo.IssueCreateOrUpdateAsync(issue_new, token);
            if (!issue.Success())
            {
                await transaction.RollbackAsync(token);
                res.Messages.AddRange(issue.Messages);
                return res;
            }

            order.HelpDeskId = issue.Response;
            context.OrdersB2B.Update(order);

            string subject_email = "Создан новый заказ";
            DateTime _dt = DateTime.UtcNow.GetCustomTime();
            string _dtAsString = $"{_dt.ToString("d", GlobalStaticConstants.RU)} {_dt.ToString("t", GlobalStaticConstants.RU)}";
            string _about_order = $"'{order.Name}' {_dtAsString}";

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

            tasks = [identityRepo.SendEmailAsync(new() { Email = actor.Response[0].Email!, Subject = subject_email, TextMessage = msg }, false, token)];

            if (actor.Response[0].TelegramId.HasValue)
                tasks.Add(tgRepo.SendTextMessageTelegramAsync(new() { Message = msg_for_tg, UserTelegramId = actor.Response[0].TelegramId!.Value }, false, token));

            if (!string.IsNullOrWhiteSpace(actor.Response[0].PhoneNumber) && GlobalTools.IsPhoneNumber(actor.Response[0].PhoneNumber!))
            {
                tasks.Add(Task.Run(async () =>
                {
                    TResponseModel<string?> CommerceNewOrderBodyNotificationWhatsapp = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.CommerceNewOrderBodyNotificationWhatsapp);
                    if (CommerceNewOrderBodyNotificationWhatsapp.Success() && !string.IsNullOrWhiteSpace(CommerceNewOrderBodyNotificationWhatsapp.Response))
                        waMsg = CommerceNewOrderBodyNotificationWhatsapp.Response;

                    await tgRepo.SendWappiMessageAsync(new() { Number = actor.Response[0].PhoneNumber!, Text = IHelpDeskService.ReplaceTags(waMsg, _dt, issue.Response, StatusesDocumentsEnum.Created, waMsg, _webConf.ClearBaseUri, _about_order, true) }, false);
                }, token));
            }

            loggerRepo.LogInformation(msg_for_tg);
            context.LockTransactions.RemoveRange(offersLocked);
            tasks.Add(context.SaveChangesAsync(token));
            await Task.WhenAll(tasks);
            await transaction.CommitAsync(token);
            return res;
        }

        OrderDocumentModelDB order_document = await context.OrdersB2B.FirstAsync(x => x.Id == order.Id, cancellationToken: token);
        if (order_document.Version != order.Version)
        {
            msg = "Документ был кем-то изменён пока был открытым";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(order, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Обновите сначала документ (F5)");
            return res;
        }

        if (order_document.Name == order.Name && order_document.Description == order.Description)
        {
            res.AddInfo($"Документ #{order.Id} не требует обновления");
            return res;
        }
        res.DocumentNewVersion = Guid.NewGuid();
        res.Response = await context.OrdersB2B
            .Where(x => x.Id == order.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, order.Name)
            .SetProperty(p => p.Description, order.Description)
            .SetProperty(p => p.Version, res.DocumentNewVersion)
            .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        res.AddSuccess($"Обновление `документа-заказа` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> RowForOrderUpdateOrCreateAsync(TAuthRequestStandardModel<RowOfOrderDocumentModelDB> req, CancellationToken token = default)
    {
        DocumentNewVersionResponseModel res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        RowOfOrderDocumentModelDB row = req.Payload;

        if (row.Quantity == 0)
        {
            res.AddError($"Количество не может быть нулевым");
            return res;
        }
        //loggerRepo.LogInformation($"{nameof(row)}:{JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderRowsQueryRecord> queryDocumentDb = from r in context.RowsOrders
                                                           join d in context.OrdersB2B.Where(x => x.Id == row.OrderId) on r.OrderId equals d.Id
                                                           join t in context.OfficesForOrders.Where(x => x.Id == row.OfficeOrderTabId) on r.OfficeOrderTabId equals t.Id
                                                           join o in context.Offers on r.OfferId equals o.Id
                                                           join g in context.Nomenclatures on r.NomenclatureId equals g.Id
                                                           select new OrderRowsQueryRecord(d, t, r, o, g);

        var orderRowRecordDb = await queryDocumentDb
            .Select(x => new
            {
                x.TabAddress.WarehouseId,
                x.Document.StatusDocument,
                OfferName = x.Offer.Name,
                GoodsName = x.Goods.Name,
            })
            .FirstAsync(cancellationToken: token);

        loggerRepo.LogInformation($"{nameof(orderRowRecordDb)}: {JsonConvert.SerializeObject(orderRowRecordDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

        bool conflict = await context.RowsOrders
            .AnyAsync(x => x.Id != row.Id && x.OfficeOrderTabId == row.OfficeOrderTabId && x.OfferId == row.OfferId, cancellationToken: token);

        if (orderRowRecordDb.WarehouseId == 0)
            res.AddError($"В документе не указан склад: обновление невозможно");

        if (conflict)
            res.AddError($"В документе уже существует этот оффер. Установите ему требуемое количество.");

        if (!res.Success())
            return res;

        RowOfOrderDocumentModelDB? rowOfOrderDb = row.Id > 0
           ? await context.RowsOrders.FirstAsync(x => x.Id == row.Id, cancellationToken: token)
           : null;

        using IDbContextTransaction transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = row.OfferId,
            LockerAreaId = orderRowRecordDb.WarehouseId,
            Marker = nameof(RowForOrderUpdateOrCreateAsync),
        }];

        if (rowOfOrderDb is not null && rowOfOrderDb.OfferId != row.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowOfOrderDb.OfferId,
                LockerAreaId = orderRowRecordDb.WarehouseId,
                Marker = nameof(RowForOrderUpdateOrCreateAsync),
            });
        }

        string msg;
        try
        {
            await context.LockTransactions.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить {nameof(RowForOrderUpdateOrCreateAsync)} ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            res.Messages.InjectException(ex);
            return res;
        }
        int[] _offersIds = [.. lockers.Select(x => x.LockerId).Distinct()];
        OfferAvailabilityModelDB[] regsOfferAv = await context
            .OffersAvailability
            .Where(x => _offersIds.Any(y => y == x.OfferId))
            .Include(x => x.Offer)
            .ToArrayAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv = regsOfferAv
            .FirstOrDefault(x => x.OfferId == row.OfferId && x.WarehouseId == orderRowRecordDb.WarehouseId);

        if (regOfferAv is null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
        {
            regOfferAv = new()
            {
                OfferId = row.OfferId,
                NomenclatureId = row.NomenclatureId,
                WarehouseId = orderRowRecordDb.WarehouseId,
            };
            await context.OffersAvailability.AddAsync(regOfferAv, token);
        }

        OfferAvailabilityModelDB? regOfferAvStorno = null;
        if (rowOfOrderDb is not null && rowOfOrderDb.OfferId != row.OfferId)
        {
            loggerRepo.LogInformation($"{nameof(rowOfOrderDb)}: {JsonConvert.SerializeObject(rowOfOrderDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            regOfferAvStorno = regsOfferAv.FirstOrDefault(x => x.OfferId == rowOfOrderDb.OfferId && x.WarehouseId == orderRowRecordDb.WarehouseId);

            if (regOfferAvStorno is null)
            {
                regOfferAvStorno = new()
                {
                    OfferId = rowOfOrderDb.OfferId,
                    NomenclatureId = rowOfOrderDb.NomenclatureId,
                    WarehouseId = orderRowRecordDb.WarehouseId,
                };
                await context.OffersAvailability.AddAsync(regOfferAvStorno, token);
            }
        }
        res.DocumentNewVersion = Guid.NewGuid();
        DateTime dtu = DateTime.UtcNow;
        await context.OrdersB2B
                .Where(x => x.Id == row.OrderId)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, res.DocumentNewVersion)
                .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        if (row.Id < 1)
        {
            if (regOfferAv is not null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
            {
                if (regOfferAv.Quantity < row.Quantity)
                    res.AddError($"Количество '{regOfferAv.Offer?.GetName()}' недостаточно: [{regOfferAv.Quantity}] < [{row.Quantity}]");
                else
                {
                    regOfferAv.Quantity -= row.Quantity;
                    context.OffersAvailability.Update(regOfferAv);
                }
            }
            else if (regOfferAv is null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
                res.AddError($"Остаток ['{orderRowRecordDb.OfferName}' - '{orderRowRecordDb.GoodsName}'] отсутствует");

            if (regOfferAvStorno is not null)
            {
                regOfferAvStorno.Quantity += row.Quantity;
                if (regOfferAvStorno.Id > 0)
                    context.OffersAvailability.Update(regOfferAvStorno);
            }

            if (!res.Success())
            {
                await transaction.RollbackAsync(token);
                return res;
            }

            row.Version = Guid.NewGuid();
            await context.RowsOrders.AddAsync(row, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Товар добавлен к заказу");
            res.Response = row.Id;
        }
        else
        {
            if (rowOfOrderDb!.Version != row.Version)
            {
                await transaction.RollbackAsync(token);
                msg = "Строка документа была уже кем-то изменена";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
                return res;
            }

            decimal _delta = row.Quantity - rowOfOrderDb.Quantity;
            if (_delta == 0)
                res.AddInfo("Количество не изменилось");
            else if (regOfferAv is not null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
            {
                regOfferAv.Quantity += _delta;
                if (regOfferAv.Id > 0)
                    context.OffersAvailability.Update(regOfferAv);

                if (regOfferAvStorno is not null)
                {
                    regOfferAvStorno.Quantity -= _delta;
                    context.OffersAvailability.Update(regOfferAvStorno);
                }
            }

            res.Response = await context.RowsOrders
              .Where(x => x.Id == row.Id)
              .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Quantity, row.Quantity)
              .SetProperty(p => p.Amount, row.Amount)
              .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
        }

        if (!res.Success())
        {
            await transaction.RollbackAsync(token);
            return res;
        }

        context.LockTransactions.RemoveRange(lockers);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        res.AddSuccess($"Обновление `строки документа-заказа` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<RowOrderDocumentRecord[]>> RowsDeleteFromOrderAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<RowOrderDocumentRecord[]> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }
        int[] rowsDel = req.Payload;

        rowsDel = [.. rowsDel.Distinct()];
        if (!rowsDel.Any(x => x > 0))
        {
            res.AddError($"Пустой запрос > {nameof(RowsDeleteFromOrderAsync)}");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfOrderDocumentModelDB> mainQuery = context.RowsOrders.Where(x => rowsDel.Any(y => y == x.Id));
        IQueryable<RowOrderDocumentRecord> q = from r in mainQuery
                                               join d in context.OrdersB2B on r.OrderId equals d.Id
                                               join t in context.OfficesForOrders on r.OfficeOrderTabId equals t.Id
                                               select new RowOrderDocumentRecord(
                                                   d.Id,
                                                   d.StatusDocument,
                                                   r.OfferId,
                                                   r.NomenclatureId,
                                                   r.Quantity,
                                                   t.WarehouseId
                                               );
        res.Response = await q
           .ToArrayAsync(cancellationToken: token);

        if (res.Response.Length == 0)
        {
            res.AddError($"Данные документа не найдены");
            return res;
        }

        DateTime dtu = DateTime.UtcNow;
        LockTransactionModelDB[] offersLocked = [.. res.Response
            .DistinctBy(x => new { x.OfferId, x.WarehouseId })
            .Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.OfferId,
                LockerAreaId = x.WarehouseId,
                Marker = nameof(RowsDeleteFromOrderAsync)
            })];

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: token);

        try
        {
            await context.LockTransactions.AddRangeAsync(offersLocked);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(RowsDeleteFromOrderAsync)}: ";
            res.AddError($"{msg}{ex.Message}");
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(rowsDel, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }

        int[] _offersIds = [.. res.Response.Select(x => x.OfferId).Distinct()];

        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .ToListAsync(cancellationToken: token);

        int[] documents_ids = [.. res.Response.Select(x => x.DocumentId).Distinct()];
        await context.OrdersB2B.Where(x => documents_ids.Any(y => y == x.Id)).ExecuteUpdateAsync(set => set.SetProperty(p => p.Version, Guid.NewGuid()).SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        foreach (RowOrderDocumentRecord rowOfOrderElementRecord in res.Response.Where(x => x.DocumentStatus != StatusesDocumentsEnum.Canceled))
        {
            OfferAvailabilityModelDB? offerRegister = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfOrderElementRecord.OfferId && x.WarehouseId == rowOfOrderElementRecord.WarehouseId);
            loggerRepo.LogInformation($"{nameof(rowOfOrderElementRecord)}: {JsonConvert.SerializeObject(rowOfOrderElementRecord, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            if (offerRegister is not null)
            {
                offerRegister.Quantity += rowOfOrderElementRecord.Quantity;
                context.OffersAvailability.Update(offerRegister);
            }
            else
            {
                offerRegister = new()
                {
                    WarehouseId = rowOfOrderElementRecord.WarehouseId,
                    NomenclatureId = rowOfOrderElementRecord.GoodsId,
                    OfferId = rowOfOrderElementRecord.OfferId,
                    Quantity = +rowOfOrderElementRecord.Quantity,
                };
                await context.OffersAvailability.AddAsync(offerRegister, token);
                registersOffersDb.Add(offerRegister);
            }
        }

        if (offersLocked.Length != 0)
            context.LockTransactions.RemoveRange(offersLocked);

        await context.SaveChangesAsync(token);
        if (await context.RowsOrders.Where(x => rowsDel.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0)
            res.AddSuccess("Изменения успешно выполнены");

        await transaction.CommitAsync(token);
        res.AddSuccess("Команда удаления выполнена");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<OrderDocumentModelDB[]> res = new();

        if (req.Payload is null)
        {
            msg = "req.Payload is null";
            loggerRepo.LogError(msg);
            res.AddError(msg);
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        res.Response = await context
            .OrdersB2B
            .Where(x => x.HelpDeskId == req.Payload.DocumentId && x.StatusDocument != req.Payload.Step)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Rows)
            .ToArrayAsync(cancellationToken: token);

        if (res.Response.Length == 0)
        {
            msg = "Изменение не требуется (документы для обновления отсутствуют)";
            loggerRepo.LogInformation($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddInfo($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию объекта");
            return res;
        }

        List<WarehouseRowDocumentRecord> _allRowsOfDocuments = [.. res.Response.SelectMany(x => x.OfficesTabs!).SelectMany(x => x.Rows!.Select(y => new WarehouseRowDocumentRecord(x.WarehouseId, y)))];

        if (_allRowsOfDocuments.Count == 0)
        {
            int _cr = await context
                    .OrdersB2B
                    .Where(x => x.HelpDeskId == req.Payload.DocumentId)
                    .ExecuteUpdateAsync(set => set.SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

            res.AddSuccess($"Запрос смены статуса заказа выполнен {(_cr == 0 ? "вхолостую (строки в документе отсутствуют)" : "успешно")}");
            return res;
        }

        LockTransactionModelDB[] offersLocked = [.. _allRowsOfDocuments
            .DistinctBy(x => new { x.WarehouseId, x.Row.OfferId })
            .Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.Row.OfferId,
                LockerAreaId = x.WarehouseId,
                Marker  = nameof(StatusOrderChangeByHelpDeskDocumentIdAsync),
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
            msg = $"Не удалось выполнить команду блокировки БД {nameof(StatusOrderChangeByHelpDeskDocumentIdAsync)}: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }

        int[] _offersIds = [.. _allRowsOfDocuments.Select(x => x.Row.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .Include(x => x.Offer)
           .ToListAsync(cancellationToken: token);

        foreach (WarehouseRowDocumentRecord rowOfWarehouseDocumentElement in _allRowsOfDocuments)
        {
            loggerRepo.LogInformation($"{nameof(rowOfWarehouseDocumentElement)}: {JsonConvert.SerializeObject(rowOfWarehouseDocumentElement, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            int _i = registersOffersDb.FindIndex(y => y.WarehouseId == rowOfWarehouseDocumentElement.WarehouseId && y.OfferId == rowOfWarehouseDocumentElement.Row.OfferId);

            if (req.Payload.Step == StatusesDocumentsEnum.Canceled)
            {
                if (_i < 0)
                {
                    OfferAvailabilityModelDB _newReg = new()
                    {
                        WarehouseId = rowOfWarehouseDocumentElement.WarehouseId,
                        NomenclatureId = rowOfWarehouseDocumentElement.Row.NomenclatureId,
                        OfferId = rowOfWarehouseDocumentElement.Row.OfferId,
                        Quantity = rowOfWarehouseDocumentElement.Row.Quantity,
                    };
                    registersOffersDb.Add(_newReg);
                    await context.OffersAvailability.AddAsync(_newReg, token);
                }
                else
                    registersOffersDb[_i].Quantity += rowOfWarehouseDocumentElement.Row.Quantity;
            }
            else
            {
                if (_i < 0)
                    res.AddError($"Отсутствуют остатки [{rowOfWarehouseDocumentElement.Row.Offer?.Name}] - списание {{{rowOfWarehouseDocumentElement.Row.Quantity}}} невозможно");
                else if (registersOffersDb[_i].Quantity < rowOfWarehouseDocumentElement.Row.Quantity)
                    res.AddError($"Недостаточно остатков [{rowOfWarehouseDocumentElement.Row.Offer?.Name}] - списание {{{rowOfWarehouseDocumentElement.Row.Quantity}}} отклонено");
                else
                    registersOffersDb[_i].Quantity -= rowOfWarehouseDocumentElement.Row.Quantity;
            }
        }

        if (!res.Success())
        {
            await transaction.RollbackAsync(token);
            msg = $"Отказ изменения статуса: не достаточно остатков!";
            loggerRepo.LogError($"{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}\n{JsonConvert.SerializeObject(res, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        context.UpdateRange(registersOffersDb.Where(x => x.Id > 0));
        if (offersLocked.Length != 0)
            context.LockTransactions.RemoveRange(offersLocked);

        await context.SaveChangesAsync(token);
        int _rc = await context
                            .OrdersB2B
                            .Where(x => x.HelpDeskId == req.Payload.DocumentId)
                            .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.StatusDocument, req.Payload.Step)
                            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
                            .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        res.AddSuccess($"Запрос смены статуса заказа {(_rc == 0 ? "[не требуется]" : $"[выполнен успешно] - изменений: {_rc}")}");
        return res;
    }
}