////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using HtmlGenerator.html5.textual;
using HtmlGenerator.html5.tables;
using HtmlGenerator.html5.areas;
using DocumentFormat.OpenXml;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService(
    IIdentityTransmission identityRepo,
    IDbContextFactory<CommerceContext> commerceDbFactory,
    IWebTransmission webTransmissionRepo,
    IHelpDeskTransmission HelpDeskRepo,
    IRubricsTransmission RubricsRepo,
    ITelegramTransmission tgRepo,
    ILogger<CommerceImplementService> loggerRepo,
    WebConfigModel _webConf,
    IParametersStorageTransmission StorageTransmissionRepo) : ICommerceService
{
    #region payment-document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestStandardModel<PaymentDocumentBaseModel> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null || req.Payload.Amount <= 0)
        {
            res.AddError("Сумма платежа должна быть больше нуля");
            return res;
        }
        if (req.Payload.OrderId < 1)
        {
            res.AddError("Не указан документ-заказ");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DateTime dtu = DateTime.UtcNow;

        PaymentDocumentModelDb? payment_db = null;
        if (!string.IsNullOrWhiteSpace(req.Payload.ExternalDocumentId))
        {
            payment_db = await context
               .PaymentsB2B
               .FirstOrDefaultAsync(x => x.ExternalDocumentId == req.Payload.ExternalDocumentId, cancellationToken: token);

            req.Payload.Id = req.Payload.Id > 0 ? req.Payload.Id : payment_db?.Id ?? 0;
        }

        if (req.Payload.Id < 1)
        {
            payment_db = new()
            {
                Name = req.Payload.Name,
                Amount = req.Payload.Amount,
                OrderId = req.Payload.OrderId,
                ExternalDocumentId = req.Payload.ExternalDocumentId,
            };

            await context.AddAsync(payment_db, token);
            await context.SaveChangesAsync(token);

            res.AddSuccess("Платёж добавлен");
            res.Response = req.Payload.Id;
            return res;
        }

        res.Response = await context.PaymentsB2B
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, req.Payload.Name)
            .SetProperty(p => p.Amount, req.Payload.Amount), cancellationToken: token);

        await context.OrdersB2B
               .Where(x => x.Id == req.Payload.OrderId)
               .ExecuteUpdateAsync(set => set.SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);


        if (!string.IsNullOrWhiteSpace(req.Payload.ExternalDocumentId) && payment_db?.ExternalDocumentId != req.Payload.ExternalDocumentId)
            res.Response = await context.PaymentsB2B
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.ExternalDocumentId, req.Payload.ExternalDocumentId), cancellationToken: token);

        res.AddSuccess($"Обновление `{GetType().Name}` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DateTime dtu = DateTime.UtcNow;
        await context.OrdersB2B
                .Where(x => context.PaymentsB2B.Any(y => y.Id == req.Payload && y.OrderId == x.Id))
                .ExecuteUpdateAsync(set => set.SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        return ResponseBaseModel.CreateInfo($"Изменений бд: {await context.PaymentsB2B.Where(x => x.Id == req.Payload).ExecuteDeleteAsync(cancellationToken: token)}");
    }
    #endregion

    #region price-rule
    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        TResponseModel<List<PriceRuleForOfferModelDB>> res = new();

        if (req.Payload is null)
        {
            res.AddError("Offer not set fo request");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context
            .PricesRules.Where(x => req.Payload.Any(y => x.OfferId == y))
            .Include(x => x.Offer)
            .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestStandardModel<PriceRuleForOfferModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.Payload is null)
        {
            res.AddError("price update body request: is null");
            return res;
        }

        if (req.Payload.Name is not null)
            req.Payload.Name = req.Payload.Name.Trim();

        if (req.Payload.QuantityRule <= 1)
        {
            res.AddError("Количество должно быть больше одного");
            return res;
        }
        if (await context.PricesRules.AnyAsync(x => x.Id != req.Payload.Id && x.OfferId == req.Payload.OfferId && x.QuantityRule == req.Payload.QuantityRule, cancellationToken: token))
        {
            res.AddError("Правило с таким количеством уже существует");
            return res;
        }

        if (req.Payload.Id < 1)
        {
            req.Payload.CreatedAtUTC = DateTime.UtcNow;
            req.Payload.LastUpdatedAtUTC = DateTime.UtcNow;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Создано новое правило ценообразования");
        }
        else
        {
            await context
                .PricesRules
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.PriceRule, req.Payload.PriceRule)
                .SetProperty(p => p.QuantityRule, req.Payload.QuantityRule)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

            res.AddSuccess("Правило ценообразования обновлено");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        ResponseBaseModel res = new();

        if (await context.PricesRules.Where(x => x.Id == req.Payload).ExecuteDeleteAsync(cancellationToken: token) > 0)
            res.AddSuccess("Правило ценообразования успешно удалено");
        else
            res.AddInfo("Правило отсутствует");

        return res;
    }
    #endregion

    #region offers
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadOffersAsync(List<NomenclatureScopeModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.Count == 0)
            return ResponseBaseModel.CreateWarning("List<NomenclatureScopeModel> Count == 0");

        string? _nameUpper;
        List<ResultMessage> Messages = [];
        foreach (NomenclatureScopeModel _n in req)
        {
            if (!Enum.TryParse<UnitsOfMeasurementEnum>(_n.BaseUnit, true, out UnitsOfMeasurementEnum _bu))
            {
                Messages.Add(new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Единица измерения `{_n.BaseUnit}` не корректная (не удалось конвертировать)" });
                continue;
            }

            _nameUpper = _n.Name?.ToUpper().Trim();
            NomenclatureModelDB? readNomenclature = await context.Nomenclatures
                .FirstOrDefaultAsync(x => x.NormalizedNameUpper == _nameUpper, cancellationToken: token);

            if (readNomenclature is null)
            {
                Messages.Add(new() { TypeMessage = MessagesTypesEnum.Info, Text = $"Создана номенклатура: {_n.Name}" });
                readNomenclature = new()
                {
                    IsDisabled = _n.IsDisabled,
                    NormalizedNameUpper = _nameUpper,
                    BaseUnit = _bu,
                    Description = _n.Description,
                    Name = _n.Name,
                    CreatedAtUTC = DateTime.UtcNow,
                };
                await context.Nomenclatures.AddAsync(readNomenclature, token);
                await context.SaveChangesAsync(cancellationToken: token);
            }

            foreach (OfferScopeModel _off in _n.Offers)
            {
                if (!Enum.TryParse(_off.OfferUnit, true, out _bu))
                {
                    Messages.Add(new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Единица измерения `{_off.OfferUnit}` не корректная (не удалось конвертировать)" });
                    continue;
                }

                // _nameUpper = _off.Name?.ToUpper().Trim();
                OfferModelDB? readOffer = await context.Offers
                    .FirstOrDefaultAsync(x => x.NomenclatureId == readNomenclature.Id && x.Name == _off.Name, cancellationToken: token);

                if (readOffer is null)
                {
                    Messages.Add(new() { TypeMessage = MessagesTypesEnum.Info, Text = $"Создан оффер: {_off.Name}" });
                    readOffer = new()
                    {
                        IsDisabled = _off.IsDisabled,
                        Description = _off.Description,
                        Name = _off.Name,
                        Multiplicity = _off.Multiplicity,
                        NomenclatureId = readNomenclature.Id,
                        Price = _off.Price,
                        Weight = _off.Weight,
                        ShortName = _off.ShortName,
                        QuantitiesTemplate = _off.QuantitiesTemplate,
                        OfferUnit = _bu,
                        CreatedAtUTC = DateTime.UtcNow,
                    };

                    await context.Offers.AddAsync(readOffer, token);
                    await context.SaveChangesAsync(cancellationToken: token);
                }
            }
        }

        if (Messages.Count != 0)
            return ResponseBaseModel.Create(Messages);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestStandardModel<OfferModelDB> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (!string.IsNullOrWhiteSpace(req.Payload.QuantitiesTemplate))
        {
            System.Collections.Immutable.ImmutableList<decimal> idss = req.Payload.QuantitiesTemplate.SplitToDecimalList();

            if (idss.Count == 0)
            {
                res.AddError("Формат доступных значений не корректный");
                return res;
            }
            req.Payload.QuantitiesTemplate = string.Join(" ", idss.Order());
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DateTime dtu = DateTime.UtcNow;
        if (req.Payload.Id < 1)
        {
            req.Payload = new()
            {
                Name = req.Payload.Name,
                QuantitiesTemplate = req.Payload.QuantitiesTemplate,
                CreatedAtUTC = dtu,
                Description = req.Payload.Description,
                ShortName = req.Payload.ShortName,
                IsDisabled = req.Payload.IsDisabled,
                Multiplicity = req.Payload.Multiplicity,
                NomenclatureId = req.Payload.NomenclatureId,
                OfferUnit = req.Payload.OfferUnit,
                Price = req.Payload.Price,
                LastUpdatedAtUTC = dtu,
                Weight = req.Payload.Weight,
            };

            await context.AddAsync(req.Payload, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Предложение добавлено");
            res.Response = req.Payload.Id;
            return res;
        }

        res.Response = await context.Offers
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, req.Payload.Name)
            .SetProperty(p => p.Description, req.Payload.Description)
            .SetProperty(p => p.QuantitiesTemplate, req.Payload.QuantitiesTemplate)
            .SetProperty(p => p.ShortName, req.Payload.ShortName)
            .SetProperty(p => p.IsDisabled, req.Payload.IsDisabled)
            .SetProperty(p => p.Multiplicity, req.Payload.Multiplicity)
            .SetProperty(p => p.NomenclatureId, req.Payload.NomenclatureId)
            .SetProperty(p => p.OfferUnit, req.Payload.OfferUnit)
            .SetProperty(p => p.Price, req.Payload.Price)
            .SetProperty(p => p.Weight, req.Payload.Weight)
            .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        res.AddSuccess($"Обновление `{GetType().Name}` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default)
    {
        if (req.Payload?.Payload is null)
        {
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload?.Payload is null" }]
            };
        }

        if (req.Payload.PageSize < 10)
            req.Payload.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OfferModelDB> q = from o in context.Offers
                                     join n in context.Nomenclatures.Where(x => x.ContextName == req.Payload.Payload.ContextName || ((req.Payload.Payload.ContextName == null || req.Payload.Payload.ContextName == "") && (x.ContextName == null || x.ContextName == ""))) on o.NomenclatureId equals n.Id
                                     select o;

        if (req.Payload.Payload.NomenclatureFilter is not null && req.Payload.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => req.Payload.Payload.NomenclatureFilter.Any(y => y == x.NomenclatureId));

        if (req.Payload.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.Payload.AfterDateUpdate);

        IOrderedQueryable<OfferModelDB> oq = req.Payload.SortingDirection == DirectionsEnum.Up
          ? q.OrderBy(x => x.CreatedAtUTC)
          : q.OrderByDescending(x => x.CreatedAtUTC);

        return new()
        {
            Response = new()
            {
                PageNum = req.Payload.PageNum,
                PageSize = req.Payload.PageSize,
                SortingDirection = req.Payload.SortingDirection,
                SortBy = req.Payload.SortBy,
                TotalRowsCount = await q.CountAsync(cancellationToken: token),
                Response = [.. await oq.Skip(req.Payload.PageNum * req.Payload.PageSize).Take(req.Payload.PageSize).Include(x => x.Nomenclature).ToArrayAsync(cancellationToken: token)]
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        TResponseModel<OfferModelDB[]> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (!req.Payload.Any(x => x > 0))
        {
            res.AddError($"Пустой запрос > {nameof(OffersReadAsync)}");
            return res;
        }
        req.Payload = [.. req.Payload.Where(x => x > 0)];
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        res.Response = await context
            .Offers
            .Where(x => req.Payload.Any(y => x.Id == y))
            .Include(x => x.Nomenclature)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        ResponseBaseModel res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int lc = await context
            .OrdersB2B
            .Where(x => context.RowsOrders.Any(y => y.OrderId == x.Id && y.OfferId == req.Payload))
            .CountAsync(cancellationToken: token);

        string msg;
        if (lc != 0)
        {
            msg = $"Оффер не может быть удалён т.к. используется в заказах: {lc} шт.";
            res.AddError(msg);
            loggerRepo.LogError(msg);
            return res;
        }

        if (await context.Offers.Where(x => x.Id == req.Payload).ExecuteDeleteAsync(cancellationToken: token) > 0)
        {
            msg = $"Оффер #{req.Payload} успешно удалён";
            loggerRepo.LogInformation(msg);
            res.AddSuccess(msg);
        }
        else
        {
            msg = $"Оффер #{req.Payload} отсутствует в БД. Возможно, он был удалён ранее";
            res.AddInfo(msg);
            loggerRepo.LogWarning($"{msg}. Оффер #{req} удалён");
        }

        return res;
    }
    #endregion

    #region nomenclatures
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB nom, CancellationToken token = default)
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

            await context.AddAsync(nomenclature_db, token);
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

    #endregion

    #region orders
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
    public async Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        req.Name = req.Name.Trim();

        TResponseModel<UserInfoModel[]> actor = await identityRepo.GetUsersOfIdentityAsync([req.AuthorIdentityUserId], token);
        if (!actor.Success() || actor.Response is null || actor.Response.Length == 0)
        {
            res.AddRangeMessages(actor.Messages);
            return res;
        }

        string msg, waMsg;
        DateTime dtu = DateTime.UtcNow;
        req.LastUpdatedAtUTC = dtu;

        OfferModelDB?[] allOffersReq = [.. req.OfficesTabs!
            .SelectMany(x => x.Rows!)
            .Select(x => x.Offer)
            .DistinctBy(x => x!.Id)];

        allOffersReq = GlobalTools.CreateDeepCopy(allOffersReq)!;

        List<Task> tasks;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: token);

        if (req.Id < 1)
        {
            if (req.OfficesTabs is null || req.OfficesTabs.Count == 0)
            {
                res.AddError($"В заказе отсутствуют адреса доставки");
                return res;
            }

            req.OfficesTabs.ForEach(x =>
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

            int[] rubricsIds = [.. req.OfficesTabs.Select(x => x.WarehouseId).Distinct()];
            TResponseModel<List<RubricStandardModel>> getRubrics = await RubricsRepo.RubricsGetAsync(rubricsIds, token);
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

            req.Id = 0;
            req.CreatedAtUTC = dtu;
            req.LastUpdatedAtUTC = dtu;
            req.Version = Guid.NewGuid();
            req.StatusDocument = StatusesDocumentsEnum.Created;

            var _offersOfDocument = req.OfficesTabs
                           .SelectMany(x => x.Rows!.Select(y => new { x.WarehouseId, Row = y }))
                           .ToArray();

            LockTransactionModelDB[] offersLocked = [.. _offersOfDocument.Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.Row.OfferId,
                LockerAreaId = x.WarehouseId,
                Marker = nameof(OrderUpdateAsync),
            })];

            try
            {
                await context.AddRangeAsync(offersLocked);
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось выполнить команду блокировки БД {nameof(OrderUpdateAsync)}: ";
                loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
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

            req.OfficesTabs.ForEach(tabAddr =>
            {
                tabAddr.Rows!.ForEach(rowDoc =>
                {
                    OfferAvailabilityModelDB? rowReg = registersOffersDb.FirstOrDefault(x => x.OfferId == rowDoc.OfferId && x.WarehouseId == tabAddr.WarehouseId);
                    OfferModelDB offerInfo = allOffersReq.First(x => x?.Id == rowDoc.OfferId)!;

                    if (rowReg is null)
                        res.AddError($"'{offerInfo.Name}' (склад: `{getRubrics.Response.First(x => x.Id == tabAddr.WarehouseId).Name}`) нет в наличии");
                    else if (rowReg.Quantity < rowDoc.Quantity)
                        res.AddError($"'{offerInfo.Name}' (склад: `{getRubrics.Response.First(x => x.Id == tabAddr.WarehouseId).Name}`) не достаточно. Текущий остаток: {rowReg.Quantity}");
                });
            });
            if (!res.Success())
            {
                await transaction.RollbackAsync(token);
                return res;
            }

            req.PrepareForSave();
            req.CreatedAtUTC = dtu;

            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.Response = req.Id;

            foreach (TabOfficeForOrderModelDb tabAddr in req.OfficesTabs)
            {
                foreach (RowOfOrderDocumentModelDB rowDoc in tabAddr.Rows!)
                {
                    OfferAvailabilityModelDB rowReg = registersOffersDb.First(x => x.OfferId == rowDoc.OfferId && x.WarehouseId == tabAddr.WarehouseId);
                    //OfferModelDB offerInfo = allOffersReq.First(x => x?.Id == rowDoc.OfferId)!;
                    rowReg.Quantity -= rowDoc.Quantity;
                    context.Update(rowReg);
                }
            }

            TAuthRequestStandardModel<UniversalUpdateRequestModel> issue_new = new()
            {
                SenderActionUserId = req.AuthorIdentityUserId,
                Payload = new()
                {
                    Name = req.Name,
                    ParentId = res_RubricIssueForCreateOrder.Response,
                    Description = $"Новый заказ.\n{req.Description}".Trim(),
                },
            };

            TResponseModel<int> issue = await HelpDeskRepo.IssueCreateOrUpdateAsync(issue_new, token);
            if (!issue.Success())
            {
                await transaction.RollbackAsync(token);
                res.Messages.AddRange(issue.Messages);
                return res;
            }

            req.HelpDeskId = issue.Response;
            context.Update(req);

            string subject_email = "Создан новый заказ";
            DateTime _dt = DateTime.UtcNow.GetCustomTime();
            string _dtAsString = $"{_dt.ToString("d", GlobalStaticConstants.RU)} {_dt.ToString("t", GlobalStaticConstants.RU)}";
            string _about_order = $"'{req.Name}' {_dtAsString}";

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
            context.RemoveRange(offersLocked);
            tasks.Add(context.SaveChangesAsync(token));
            await Task.WhenAll(tasks);
            await transaction.CommitAsync(token);
            return res;
        }

        OrderDocumentModelDB order_document = await context.OrdersB2B.FirstAsync(x => x.Id == req.Id, cancellationToken: token);
        if (order_document.Version != req.Version)
        {
            msg = "Документ был кем-то изменён пока был открытым";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Обновите сначала документ (F5)");
            return res;
        }

        if (order_document.Name == req.Name && order_document.Description == req.Description)
        {
            res.AddInfo($"Документ #{req.Id} не требует обновления");
            return res;
        }

        res.Response = await context.OrdersB2B
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, req.Name)
            .SetProperty(p => p.Description, req.Description)
            .SetProperty(p => p.Version, Guid.NewGuid())
            .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        res.AddSuccess($"Обновление `документа-заказа` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForOrderUpdateAsync(RowOfOrderDocumentModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (req.Quantity == 0)
        {
            res.AddError($"Количество не может быть нулевым");
            return res;
        }
        loggerRepo.LogInformation($"{nameof(req)}:{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderRowsQueryRecord> queryDocumentDb = from r in context.RowsOrders
                                                           join d in context.OrdersB2B.Where(x => x.Id == req.OrderId) on r.OrderId equals d.Id
                                                           join t in context.OfficesForOrders.Where(x => x.Id == req.OfficeOrderTabId) on r.OfficeOrderTabId equals t.Id
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
            .AnyAsync(x => x.Id != req.Id && x.OfficeOrderTabId == req.OfficeOrderTabId && x.OfferId == req.OfferId, cancellationToken: token);

        if (orderRowRecordDb.WarehouseId == 0)
            res.AddError($"В документе не указан склад: обновление невозможно");

        if (conflict)
            res.AddError($"В документе уже существует этот оффер. Установите ему требуемое количество.");

        if (!res.Success())
            return res;

        RowOfOrderDocumentModelDB? rowOfOrderDb = req.Id > 0
           ? await context.RowsOrders.FirstAsync(x => x.Id == req.Id, cancellationToken: token)
           : null;

        using IDbContextTransaction transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            LockerAreaId = orderRowRecordDb.WarehouseId,
            Marker = nameof(RowForOrderUpdateAsync),
        }];

        if (rowOfOrderDb is not null && rowOfOrderDb.OfferId != req.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowOfOrderDb.OfferId,
                LockerAreaId = orderRowRecordDb.WarehouseId,
                Marker = nameof(RowForOrderUpdateAsync),
            });
        }

        string msg;
        try
        {
            await context.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }
        int[] _offersIds = [.. lockers.Select(x => x.LockerId).Distinct()];
        OfferAvailabilityModelDB[] regsOfferAv = await context
            .OffersAvailability
            .Where(x => _offersIds.Any(y => y == x.OfferId))
            .Include(x => x.Offer)
            .ToArrayAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv = regsOfferAv
            .FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == orderRowRecordDb.WarehouseId);

        if (regOfferAv is null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
        {
            regOfferAv = new()
            {
                OfferId = req.OfferId,
                NomenclatureId = req.NomenclatureId,
                WarehouseId = orderRowRecordDb.WarehouseId,
            };
            await context.AddAsync(regOfferAv, token);
        }

        OfferAvailabilityModelDB? regOfferAvStorno = null;
        if (rowOfOrderDb is not null && rowOfOrderDb.OfferId != req.OfferId)
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
                await context.AddAsync(regOfferAvStorno, token);
            }
        }

        DateTime dtu = DateTime.UtcNow;
        await context.OrdersB2B
                .Where(x => x.Id == req.OrderId)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        if (req.Id < 1)
        {
            if (regOfferAv is not null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
            {
                if (regOfferAv.Quantity < req.Quantity)
                    res.AddError($"Количество '{regOfferAv.Offer?.GetName()}' недостаточно: [{regOfferAv.Quantity}] < [{req.Quantity}]");
                else
                {
                    regOfferAv.Quantity -= req.Quantity;
                    context.OffersAvailability.Update(regOfferAv);
                }
            }
            else if (regOfferAv is null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
                res.AddError($"Остаток ['{orderRowRecordDb.OfferName}' - '{orderRowRecordDb.GoodsName}'] отсутствует");

            if (regOfferAvStorno is not null)
            {
                regOfferAvStorno.Quantity += req.Quantity;
                if (regOfferAvStorno.Id > 0)
                    context.Update(regOfferAvStorno);
            }

            req.Version = Guid.NewGuid();
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.AddSuccess("Товар добавлен к заказу");
            res.Response = req.Id;
        }
        else
        {
            if (rowOfOrderDb!.Version != req.Version)
            {
                await transaction.RollbackAsync(token);
                msg = "Строка документа была уже кем-то изменена";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
                return res;
            }

            decimal _delta = req.Quantity - rowOfOrderDb.Quantity;
            if (_delta == 0)
                res.AddInfo("Количество не изменилось");
            else if (regOfferAv is not null && orderRowRecordDb.StatusDocument != StatusesDocumentsEnum.Canceled)
            {
                regOfferAv.Quantity += _delta;
                if (regOfferAv.Id > 0)
                    context.Update(regOfferAv);

                if (regOfferAvStorno is not null)
                {
                    regOfferAvStorno.Quantity -= _delta;
                    context.Update(regOfferAvStorno);
                }
            }

            res.Response = await context.RowsOrders
              .Where(x => x.Id == req.Id)
              .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Quantity, req.Quantity)
              .SetProperty(p => p.Amount, req.Amount)
              .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
        }

        if (!res.Success())
        {
            await transaction.RollbackAsync(token);
            return res;
        }

        context.RemoveRange(lockers);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        res.AddSuccess($"Обновление `строки документа-заказа` выполнено");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForOrderDeleteAsync(int[] req, CancellationToken token = default)
    {
        string msg;
        req = [.. req.Distinct()];
        TResponseModel<bool> res = new() { Response = req.Any(x => x > 0) };
        if (!res.Response)
        {
            res.AddError($"Пустой запрос > {nameof(RowsForOrderDeleteAsync)}");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfOrderDocumentModelDB> mainQuery = context.RowsOrders.Where(x => req.Any(y => y == x.Id));
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
        RowOrderDocumentRecord[] _allOffersOfDocuments = await q
           .ToArrayAsync(cancellationToken: token);

        if (_allOffersOfDocuments.Length == 0)
        {
            res.AddError($"Данные документа не найдены");
            return res;
        }

        DateTime dtu = DateTime.UtcNow;
        LockTransactionModelDB[] offersLocked = [.. _allOffersOfDocuments
            .DistinctBy(x => new { x.OfferId, x.WarehouseId })
            .Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.OfferId,
                LockerAreaId = x.WarehouseId,
                Marker = nameof(RowsForOrderDeleteAsync)
            })];

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: token);

        try
        {
            await context.AddRangeAsync(offersLocked);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(RowsForOrderDeleteAsync)}: ";
            res.AddError($"{msg}{ex.Message}");
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }

        int[] _offersIds = [.. _allOffersOfDocuments.Select(x => x.OfferId).Distinct()];

        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .ToListAsync(cancellationToken: token);

        int[] documents_ids = [.. _allOffersOfDocuments.Select(x => x.DocumentId).Distinct()];
        await context.OrdersB2B.Where(x => documents_ids.Any(y => y == x.Id)).ExecuteUpdateAsync(set => set.SetProperty(p => p.Version, Guid.NewGuid()).SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        foreach (RowOrderDocumentRecord rowOfOrderElementRecord in _allOffersOfDocuments.Where(x => x.DocumentStatus != StatusesDocumentsEnum.Canceled))
        {
            OfferAvailabilityModelDB? offerRegister = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfOrderElementRecord.OfferId && x.WarehouseId == rowOfOrderElementRecord.WarehouseId);
            loggerRepo.LogInformation($"{nameof(rowOfOrderElementRecord)}: {JsonConvert.SerializeObject(rowOfOrderElementRecord, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            if (offerRegister is not null)
            {
                offerRegister.Quantity += rowOfOrderElementRecord.Quantity;
                context.Update(offerRegister);
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
            context.RemoveRange(offersLocked);

        await context.SaveChangesAsync(token);
        res.Response = await context.RowsOrders.Where(x => req.Any(y => y == x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0;

        await transaction.CommitAsync(token);
        res.AddSuccess("Команда удаления выполнена");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusesOrdersChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<bool> res = new();

        if (req.Payload is null)
        {
            msg = "req.Payload is null";
            loggerRepo.LogError(msg);
            res.AddError(msg);
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        OrderDocumentModelDB[] ordersDb = await context
            .OrdersB2B
            .Where(x => x.HelpDeskId == req.Payload.DocumentId && x.StatusDocument != req.Payload.Step)
            .Include(x => x.OfficesTabs!)
            .ThenInclude(x => x.Rows)
            .ToArrayAsync(cancellationToken: token);

        if (ordersDb.Length == 0)
        {
            msg = "Изменение не требуется (документы для обновления отсутствуют)";
            loggerRepo.LogInformation($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddInfo($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию объекта");
            return res;
        }

        List<WarehouseRowDocumentRecord> _allRowsOfDocuments = [.. ordersDb.SelectMany(x => x.OfficesTabs!).SelectMany(x => x.Rows!.Select(y => new WarehouseRowDocumentRecord(x.WarehouseId, y)))];

        if (_allRowsOfDocuments.Count == 0)
        {
            res.Response = await context
                    .OrdersB2B
                    .Where(x => x.HelpDeskId == req.Payload.DocumentId)
                    .ExecuteUpdateAsync(set => set.SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token) != 0;

            res.AddSuccess("Запрос смены статуса заказа выполнен вхолостую (строки в документе отсутствуют)");
            return res;
        }

        LockTransactionModelDB[] offersLocked = [.. _allRowsOfDocuments
            .DistinctBy(x => new { x.WarehouseId, x.Row.OfferId })
            .Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.Row.OfferId,
                LockerAreaId = x.WarehouseId,
                Marker  = nameof(StatusesOrdersChangeByHelpDeskDocumentIdAsync),
            })];

        using IDbContextTransaction transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
        try
        {
            await context.AddRangeAsync(offersLocked);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(StatusesOrdersChangeByHelpDeskDocumentIdAsync)}: ";
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
                    await context.AddAsync(_newReg);
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
            context.RemoveRange(offersLocked);

        await context.SaveChangesAsync(token);
        res.Response = await context
                            .OrdersB2B
                            .Where(x => x.HelpDeskId == req.Payload.DocumentId)
                            .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.StatusDocument, req.Payload.Step)
                            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
                            .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token) != 0;

        await transaction.CommitAsync(token);
        res.AddSuccess("Запрос смены статуса заказа выполнен успешно");
        return res;
    }
    #endregion

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default)
    {

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>> GetOrderReportFileAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(req.SenderActionUserId))
            return new() { Messages = [new() { Text = "string.IsNullOrEmpty(req.SenderActionUserId)", TypeMessage = MessagesTypesEnum.Error }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<UserInfoModel[]> rest = default!;
        TResponseModel<OrderDocumentModelDB[]> orderData = default!;
        List<Task> _taskList = [
            Task.Run(async () => { rest = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]); }, token),
            Task.Run(async () => { orderData = await OrdersReadAsync(new(){ Payload = [req.Payload], SenderActionUserId = req.SenderActionUserId }); }, token)];

        await Task.WhenAll(_taskList);

        if (!rest.Success() || rest.Response is null || rest.Response.Length != 1)
            return new() { Messages = rest.Messages };

        TResponseModel<FileAttachModel> res = new();
        if (!orderData.Success() || orderData.Response is null || orderData.Response.Length != 1)
        {
            res.AddRangeMessages(orderData.Messages);
            return res;
        }

        OrderDocumentModelDB orderDb = orderData.Response[0];
        UserInfoModel actor = rest.Response[0];
        bool allowed = actor.IsAdmin || orderDb.AuthorIdentityUserId == actor.UserId || actor.UserId == GlobalStaticConstantsRoles.Roles.System;
        if (!allowed && orderDb.HelpDeskId.HasValue && orderDb.HelpDeskId.Value > 0)
        {
            TResponseModel<IssueHelpDeskModelDB[]> issueData = await HelpDeskRepo.IssuesReadAsync(new TAuthRequestStandardModel<IssuesReadRequestModel>()
            {
                SenderActionUserId = req.SenderActionUserId,
                Payload = new()
                {
                    IssuesIds = [orderDb.HelpDeskId.Value],
                    IncludeSubscribersOnly = true,
                }
            }, token);
            if (!issueData.Success() || issueData.Response is null || issueData.Response.Length != 1)
            {
                res.AddRangeMessages(issueData.Messages);
                return res;
            }
            IssueHelpDeskModelDB issueDb = issueData.Response[0];
            if (actor.UserId != issueDb.AuthorIdentityUserId && actor.UserId != issueDb.ExecutorIdentityUserId && !issueDb.Subscribers!.Any(s => s.UserId == actor.UserId))
            {
                res.AddError("У вас не доступа к этому документу");
                return res;
            }
        }
        else if (!allowed)
        {
            res.AddError("У вас не доступа к этому документу");
            return res;
        }

        string docName = $"Заказ {orderDb.Name} от {orderDb.CreatedAtUTC.GetHumanDateTime()}";

        try
        {
            res.Response = new()
            {
                Data = SaveOrderAsExcel(orderDb),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("xlsx")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.xlsx",
            };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка создания Excel документа: {docName}");
            div wrapDiv = new();
            wrapDiv.AddDomNode(new p(docName));

            orderDb.OfficesTabs!.ForEach(aNode =>
            {
                div addressDiv = new();
                addressDiv.AddDomNode(new p($"Адрес: `{aNode.Office?.Name}`"));

                table my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };
                my_table.THead.AddColumn("Наименование").AddColumn("Цена").AddColumn("Кол-во").AddColumn("Сумма");

                aNode.Rows?.ForEach(dr =>
                {
                    my_table.TBody.AddRow([dr.Offer!.GetName(), dr.Offer.Price.ToString(), dr.Quantity.ToString(), dr.Amount.ToString()]);
                });
                addressDiv.AddDomNode(my_table);
                addressDiv.AddDomNode(new p($"Итого: {aNode.Rows!.Sum(x => x.Amount)}") { css_style = "float: right;" });
                wrapDiv.AddDomNode(addressDiv);
            });

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string test_s = $"<style>table, th, td {{border: 1px solid black;border-collapse: collapse;}}</style>{wrapDiv.GetHTML()}";

            using MemoryStream ms = new();
            StreamWriter writer = new(ms);
            writer.Write(test_s);
            writer.Flush();
            ms.Position = 0;

            res.Response = new()
            {
                Data = ms.ToArray(),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetExcelAsync(CancellationToken token = default)
    {
        string docName = $"Прайс на {DateTime.Now.GetHumanDateTime()}";
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        List<OfferModelDB> offersAll = await context.Offers
            .Include(x => x.Nomenclature)
            .Include(x => x.Registers)
            .ToListAsync(cancellationToken: token);

        if (offersAll.Count == 0)
        {
            loggerRepo.LogWarning($"Пустой прайс: {docName}");
            return new()
            {
                Data = [],
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }

        int[] rubricsIds = offersAll.SelectMany(x => x.Registers!).Select(x => x.WarehouseId).Distinct().ToArray();
        TResponseModel<List<RubricStandardModel>> rubricsDb = await RubricsRepo.RubricsGetAsync(rubricsIds, token);
        List<IGrouping<NomenclatureModelDB?, OfferModelDB>> gof = offersAll.GroupBy(x => x.Nomenclature).Where(x => x.Any(y => y.Registers!.Any(z => z.Quantity > 0))).ToList();
        try
        {
            return new()
            {
                Data = ExportPrice(gof, rubricsDb.Response),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("xlsx")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.xlsx",
            };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка создания Excel документа: {docName}");
            return new()
            {
                Data = [],
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }
    }

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetJsonAsync(CancellationToken token = default)
    {
        string docName = $"Прайс на {DateTime.Now.GetHumanDateTime()}";
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        List<OfferModelDB> offersAll = await context.Offers
            .Include(x => x.Nomenclature)
            .Include(x => x.Registers)
            .ToListAsync(cancellationToken: token);

        if (offersAll.Count == 0)
        {
            loggerRepo.LogWarning($"Пустой прайс: {docName}");
            return new()
            {
                Data = [],
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }

        int[] rubricsIds = [.. offersAll.SelectMany(x => x.Registers!).Select(x => x.WarehouseId).Distinct()];
        TResponseModel<List<RubricStandardModel>> rubricsDb = await RubricsRepo.RubricsGetAsync(rubricsIds, token);

        List<NomenclatureScopeModel> _catalogData = [];
        foreach (IGrouping<NomenclatureModelDB?, OfferModelDB> _gn in offersAll.GroupBy(x => x.Nomenclature))
        {
            NomenclatureScopeModel _nom = new()
            {
                BaseUnit = _gn.Key!.BaseUnit.ToString(),
                IsDisabled = _gn.Key.IsDisabled,
                Description = _gn.Key.Description,
                Name = _gn.Key.Name,
                Offers = [.. _gn.Select(x => new OfferScopeModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    IsDisabled = x.IsDisabled,
                    Multiplicity = x.Multiplicity,
                    OfferUnit = x.OfferUnit.ToString(),
                    Price = x.Price,
                    QuantitiesTemplate = x.QuantitiesTemplate,
                    ShortName = x.ShortName,
                    Weight = x.Weight,
                    Registers = [.. x.Registers!.Select(y=> new OfferAvailabilityScopeModel()
                    {
                        Warehouse = rubricsDb.Response!.First(r=>r.Id == y.WarehouseId).Name!,
                         Quantity = y.Quantity,
                    })]
                })]
            };
            _catalogData.Add(_nom);
        }

        string inputString = JsonConvert.SerializeObject(_catalogData, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);

        try
        {
            return new()
            {
                Data = Encoding.UTF8.GetBytes(inputString),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("json")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.json",
            };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка создания Excel документа: {docName}");
            return new()
            {
                Data = [],
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }
    }

    #region static
    static byte[] SaveOrderAsExcel(OrderDocumentModelDB orderDb)
    {
        string docName = $"Заказ {orderDb.Name} от {orderDb.CreatedAtUTC.GetHumanDateTime()}";
        using MemoryStream XLSStream = new();
        WorkbookPart? wBookPart = null;

        using SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(XLSStream, SpreadsheetDocumentType.Workbook);

        wBookPart = spreadsheetDoc.AddWorkbookPart();
        wBookPart.Workbook = new Workbook();
        uint sheetId = 1;
        WorkbookPart workbookPart = spreadsheetDoc.WorkbookPart ?? spreadsheetDoc.AddWorkbookPart();

        WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

        wbsp.Stylesheet = GenerateExcelStyleSheet();
        wbsp.Stylesheet.Save();

        workbookPart.Workbook.Sheets = new Sheets();

        Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());

        foreach (var table in orderDb.OfficesTabs!)
        {
            WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
            Sheet sheet = new() { Id = workbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = table.Office?.Name };
            sheets.Append(sheet);

            SheetData sheetData = new();
            wSheetPart.Worksheet = new Worksheet(sheetData);

            Columns lstColumns = wSheetPart.Worksheet.GetFirstChild<Columns>()!;
            bool needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }

            lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 100, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 15, CustomWidth = true, BestFit = true, });

            if (needToInsertColumns)
                wSheetPart.Worksheet.InsertAt(lstColumns, 0);

            Row topRow = new() { RowIndex = 2 };
            InsertExcelCell(topRow, 1, $"Адрес доставки: {table.Office?.AddressUserComment}", CellValues.String, 0);
            sheetData!.Append(topRow);

            Row headerRow = new() { RowIndex = 4 };
            InsertExcelCell(headerRow, 1, "Наименование", CellValues.String, 1);
            InsertExcelCell(headerRow, 2, "Цена", CellValues.String, 1);
            InsertExcelCell(headerRow, 3, "Кол-во", CellValues.String, 1);
            InsertExcelCell(headerRow, 4, "Сумма", CellValues.String, 1);
            sheetData.AppendChild(headerRow);

            uint row_index = 5;
            foreach (RowOfOrderDocumentModelDB dr in table.Rows!)
            {
                Row row = new() { RowIndex = row_index++ };
                InsertExcelCell(row, 1, dr.Offer!.GetName(), CellValues.String, 0);
                InsertExcelCell(row, 2, dr.Offer.Price.ToString(), CellValues.String, 0);
                InsertExcelCell(row, 3, dr.Quantity.ToString(), CellValues.String, 0);
                InsertExcelCell(row, 4, dr.Amount.ToString(), CellValues.String, 0);
                sheetData.Append(row);
            }
            Row btRow = new() { RowIndex = row_index++ };
            InsertExcelCell(btRow, 1, "", CellValues.String, 0);
            InsertExcelCell(btRow, 2, "", CellValues.String, 0);
            InsertExcelCell(btRow, 3, "Итого:", CellValues.String, 0);
            InsertExcelCell(btRow, 4, table.Rows!.Sum(x => x.Amount).ToString(), CellValues.String, 0);
            sheetData.Append(btRow);
            sheetId++;
        }

        workbookPart.Workbook.Save();
        spreadsheetDoc.Save();

        XLSStream.Position = 0;
        return XLSStream.ToArray();
    }

    static byte[] ExportPrice(List<IGrouping<NomenclatureModelDB?, OfferModelDB>> sourceTable, List<RubricStandardModel>? rubricsDb)
    {
        WorkbookPart? wBookPart = null;
        using MemoryStream XLSStream = new();
        using SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(XLSStream, SpreadsheetDocumentType.Workbook);

        wBookPart = spreadsheetDoc.AddWorkbookPart();
        wBookPart.Workbook = new Workbook();
        uint sheetId = 1;
        WorkbookPart workbookPart = spreadsheetDoc.WorkbookPart ?? spreadsheetDoc.AddWorkbookPart();

        WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

        wbsp.Stylesheet = GenerateExcelStyleSheet();
        wbsp.Stylesheet.Save();

        workbookPart.Workbook.Sheets = new Sheets();

        Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());

        foreach (IGrouping<NomenclatureModelDB?, OfferModelDB> table in sourceTable)
        {
            WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
            Sheet sheet = new() { Id = workbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = table.Key?.Name };
            sheets.Append(sheet);

            SheetData sheetData = new();
            wSheetPart.Worksheet = new Worksheet(sheetData);

            Columns lstColumns = wSheetPart.Worksheet.GetFirstChild<Columns>()!;
            bool needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }

            lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 100, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 15, CustomWidth = true, BestFit = true, });

            if (needToInsertColumns)
                wSheetPart.Worksheet.InsertAt(lstColumns, 0);

            Row topRow = new() { RowIndex = 2 };
            InsertExcelCell(topRow, 1, $"Дата формирования: {DateTime.Now.GetHumanDateTime()}", CellValues.String, 0);
            sheetData!.Append(topRow);

            Row headerRow = new() { RowIndex = 4 };
            InsertExcelCell(headerRow, 1, "Наименование", CellValues.String, 1);
            InsertExcelCell(headerRow, 2, "Цена", CellValues.String, 1);
            InsertExcelCell(headerRow, 3, "Ед.изм.", CellValues.String, 1);
            InsertExcelCell(headerRow, 4, "Остаток/Склад", CellValues.String, 1);
            sheetData.AppendChild(headerRow);

            uint row_index = 5;
            foreach (OfferModelDB dr in table)
            {
                foreach (IGrouping<int, OfferAvailabilityModelDB> nodeG in dr.Registers!.GroupBy(x => x.WarehouseId))
                {
                    Row row = new() { RowIndex = row_index++ };
                    sheetData.AppendChild(row);
                    InsertExcelCell(row, 1, dr!.GetName(), CellValues.String, 0);
                    InsertExcelCell(row, 2, dr.Price.ToString(), CellValues.String, 0);
                    InsertExcelCell(row, 3, dr.OfferUnit.DescriptionInfo(), CellValues.String, 0);
                    InsertExcelCell(row, 4, $"{nodeG.Sum(x => x.Quantity)} /{rubricsDb?.FirstOrDefault(r => r.Id == nodeG.Key)?.Name}", CellValues.String, 0);
                }
                ;
            }
            Row btRow = new() { RowIndex = row_index++ };
            InsertExcelCell(btRow, 1, "", CellValues.String, 0);
            InsertExcelCell(btRow, 2, "", CellValues.String, 0);
            InsertExcelCell(btRow, 3, "Итого:", CellValues.String, 0);
            InsertExcelCell(btRow, 4, table!.Sum(x => x.Registers!.Sum(y => y.Quantity)).ToString(), CellValues.String, 0);
            sheetData.Append(btRow);
            sheetId++;
        }

        workbookPart.Workbook.Save();
        spreadsheetDoc.Save();
        XLSStream.Position = 0;

        return XLSStream.ToArray();
    }

    static Stylesheet GenerateExcelStyleSheet()
    {
        return new Stylesheet(
            new Fonts(
                new Font(                                                               // Стиль под номером 0 - Шрифт по умолчанию.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Calibri" }),
                new Font(                                                               // Стиль под номером 1 - Жирный шрифт Times New Roman.
                    new Bold(),
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Times New Roman" }),
                new Font(                                                               // Стиль под номером 2 - Обычный шрифт Times New Roman.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Times New Roman" }),
                new Font(                                                               // Стиль под номером 3 - Шрифт Times New Roman размером 14.
                    new FontSize() { Val = 14 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
        new FontName() { Val = "Times New Roman" })
        ),
        new Fills(
                new Fill(                                                           // Стиль под номером 0 - Заполнение ячейки по умолчанию.
        new PatternFill() { PatternType = PatternValues.None }),
                new Fill(                                                           // Стиль под номером 1 - Заполнение ячейки серым цветом
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFAAAAAA" } }
        )
                    { PatternType = PatternValues.Solid }),
        new Fill(                                                           // Стиль под номером 2 - Заполнение ячейки красным.
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFEFEFEF" } }
                    )
                    { PatternType = PatternValues.Solid })
            )
            ,
            new Borders(
                new Border(                                                         // Стиль под номером 0 - Грани.
                    new LeftBorder(),
                    new RightBorder(),
                    new TopBorder(),
                    new BottomBorder(),
                    new DiagonalBorder()),
                new Border(                                                         // Стиль под номером 1 - Грани
                    new LeftBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Medium },
                    new RightBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Medium },
                    new TopBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Medium },
                    new BottomBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Medium },
                    new DiagonalBorder()),
                new Border(                                                         // Стиль под номером 2 - Грани.
                    new LeftBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Thin },
                    new RightBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Thin },
                    new TopBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Thin },
                    new BottomBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            ),
            new CellFormats(
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Стиль под номером 0 - The default cell style.  (по умолчанию)
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 1, FillId = 2, BorderId = 1, ApplyFont = true },       // Стиль под номером 1 - Bold 
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true },       // Стиль под номером 2 - REgular
                new CellFormat() { FontId = 3, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 },       // Стиль под номером 3 - Times Roman
                new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Стиль под номером 4 - Yellow Fill
                new CellFormat(                                                                   // Стиль под номером 5 - Alignment
                    new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                )
                { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true },      // Стиль под номером 6 - Border
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 }       // Стиль под номером 7 - Задает числовой формат полю.
            )
        );
    }

    static void InsertExcelCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
    {
        Cell? refCell = null;
        Cell newCell = new() { CellReference = cell_num.ToString() + ":" + row.RowIndex?.ToString(), StyleIndex = styleIndex };
        row.InsertBefore(newCell, refCell);

        newCell.CellValue = new CellValue(val);
        newCell.DataType = new EnumValue<CellValues>(type);
    }
    #endregion
}