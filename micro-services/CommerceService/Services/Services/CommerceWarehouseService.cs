////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WarehouseDocumentUpdateAsync(WarehouseDocumentModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(req);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        req.DeliveryDate = req.DeliveryDate.SetKindUtc();
        if (req.Name is not null)
            req.Name = req.Name.Trim();
        req.NormalizedUpperName = req.Name?.ToUpper() ?? "";

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        string msg;
        DateTime dtu = DateTime.UtcNow;
        if (req.Id < 1)
        {
            req.Rows?.Clear();
            req.Id = 0;
            req.Version = Guid.NewGuid();
            req.CreatedAtUTC = dtu;
            req.LastUpdatedAtUTC = dtu;
            req.IsDisabled = true;
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);
            res.Response = req.Id;
            res.AddSuccess("Документ создан");
            return res;
        }

        WarehouseDocumentModelDB warehouseDocumentDb = await context
            .WarehouseDocuments
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Id, cancellationToken: token);

        if (warehouseDocumentDb.Version != req.Version)
        {
            msg = $"Документ #{warehouseDocumentDb.Id} был кем-то изменён (version concurrent)";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}. Перед редактированием обновите страницу (F5), что бы загрузить актуальную версию объекта");
            return res;
        }

        List<LockTransactionModelDB> offersLocked = [];
        if (warehouseDocumentDb.Rows!.Count != 0)
            warehouseDocumentDb.Rows.ForEach(x =>
            {
                offersLocked.Add(new LockTransactionModelDB() { LockerName = nameof(OfferAvailabilityModelDB), LockerId = x.OfferId, RubricId = req.WarehouseId });
                if (req.WritingOffWarehouseId > 0 && req.WritingOffWarehouseId != req.WarehouseId)
                    offersLocked.Add(new LockTransactionModelDB() { LockerName = nameof(OfferAvailabilityModelDB), LockerId = x.OfferId, RubricId = req.WritingOffWarehouseId });
            });

        offersLocked = [.. offersLocked.DistinctBy(x => x.RubricId)];

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        if (offersLocked.Count != 0)
        {
            try
            {
                await context.AddRangeAsync(offersLocked, token);
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось выполнить команду блокировки БД {nameof(WarehouseDocumentUpdateAsync)}: ";
                loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}{ex.Message}");
                return res;
            }
        }

        int[] _offersIds = [.. warehouseDocumentDb.Rows.Select(x => x.OfferId).Distinct()];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
            .Where(x => _offersIds.Any(y => y == x.OfferId))
            .ToListAsync(cancellationToken: token);

        if (warehouseDocumentDb.IsDisabled != req.IsDisabled)
        {
            if (req.IsDisabled)
            {
                warehouseDocumentDb.Rows.ForEach(async rowOfDocument =>
                {
                    OfferAvailabilityModelDB? registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId);
                    if (registerOffer is not null)
                    {
                        if (registerOffer.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество сторно [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.Name}'] больше баланса в БД: ";
                            loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                            res.AddError($"{msg}. Баланс не может быть отрицательным");
                            return;
                        }

                        await context.OffersAvailability.Where(y => y.Id == registerOffer.Id)
                         .ExecuteUpdateAsync(set => set
                         .SetProperty(p => p.Quantity, registerOffer.Quantity - rowOfDocument.Quantity));
                    }
                    else
                    {
                        msg = $"Количество сторно [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.Name}'] не может быть списано (остаток 0): ";
                        loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        return;
                    }
                });
                if (!res.Success())
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось выполнить обновить складской документ: ";
                    loggerRepo.LogError($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError(msg);
                    return res;
                }
            }
            else
            {
                List<OfferAvailabilityModelDB> offAvAdding = [];
                warehouseDocumentDb.Rows.ForEach(async rowOfDocument =>
                {
                    OfferAvailabilityModelDB? registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WarehouseId);

                    if (registerOffer is not null)
                        await context.OffersAvailability
                             .Where(y => y.Id == registerOffer.Id)
                             .ExecuteUpdateAsync(set => set.SetProperty(p => p.Quantity, registerOffer.Quantity + rowOfDocument.Quantity));
                    else
                        offAvAdding.Add(new OfferAvailabilityModelDB()
                        {
                            WarehouseId = warehouseDocumentDb.WarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        });

                    if (warehouseDocumentDb.WarehouseId != req.WarehouseId)
                    {
                        registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == warehouseDocumentDb.WarehouseId);

                        if (registerOffer is not null)
                            await context.OffersAvailability.Where(y => y.Id == registerOffer.Id).ExecuteUpdateAsync(set => set.SetProperty(p => p.Quantity, registerOffer.Quantity - rowOfDocument.Quantity));
                        else
                            offAvAdding.Add(new()
                            {
                                WarehouseId = warehouseDocumentDb.WarehouseId,
                                NomenclatureId = rowOfDocument.NomenclatureId,
                                OfferId = rowOfDocument.OfferId,
                                Quantity = -rowOfDocument.Quantity,
                            });
                    }
                });
                if (offAvAdding.Count != 0)
                    await context.AddRangeAsync(offAvAdding, token);
            }
        }

        res.Response = await context.WarehouseDocuments
                .Where(x => x.Id == req.Id)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.DeliveryDate, req.DeliveryDate)
                .SetProperty(p => p.IsDisabled, req.IsDisabled)
                .SetProperty(p => p.WarehouseId, req.WarehouseId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);

        if (offersLocked.Count != 0)
            context.RemoveRange(offersLocked);

        res.AddSuccess("Складской документ обновлён");
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForWarehouseDocumentDeleteAsync(int[] req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<bool> res = new() { Response = req.Any(x => x > 0) };
        if (!res.Response)
        {
            res.AddError("Пустой запрос");
            return res;
        }
        req = [.. req.Distinct()];
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfWarehouseDocumentModelDB> mainQuery = context.RowsWarehouses.Where(x => req.Any(y => y == x.Id));
        var q = from r in mainQuery
                join d in context.WarehouseDocuments on r.WarehouseDocumentId equals d.Id
                select new
                {
                    DocumentId = d.Id,
                    d.IsDisabled,
                    d.WarehouseId,
                    r.OfferId,
                    r.NomenclatureId,
                    r.Quantity
                };

        var _allOffersOfDocuments = await q
           .ToArrayAsync(cancellationToken: token);

        if (_allOffersOfDocuments.Length == 0)
        {
            res.AddWarning($"Данные не найдены. Метод удаления [{nameof(RowsForWarehouseDocumentDeleteAsync)}] не может выполнить команду.");
            return res;
        }
        LockTransactionModelDB[] offersLocked = [.. _allOffersOfDocuments
            .Select(x => new LockTransactionModelDB()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = x.OfferId,
                RubricId = x.WarehouseId
            })];

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        try
        {
            await context.AddRangeAsync(offersLocked);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду блокировки БД {nameof(WarehouseDocumentUpdateAsync)}: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError($"{msg}{ex.Message}");
            return res;
        }

        int[] _offersIds = [.. _allOffersOfDocuments.Select(x => x.OfferId)];
        List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
           .Where(x => _offersIds.Any(y => y == x.OfferId))
           .ToListAsync(cancellationToken: token);

        foreach (int doc_id in _allOffersOfDocuments.Select(x => x.DocumentId).Distinct())
            await context.WarehouseDocuments.Where(x => x.Id == doc_id).ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Version, Guid.NewGuid())
            .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        foreach (var rowEl in _allOffersOfDocuments.Where(x => !x.IsDisabled))
        {
            OfferAvailabilityModelDB? offerRegister = registersOffersDb.FirstOrDefault(x => x.OfferId == rowEl.OfferId && x.WarehouseId == rowEl.WarehouseId);
            if (offerRegister is not null)
            {
                offerRegister.Quantity -= rowEl.Quantity;
                context.Update(offerRegister);
            }
            else
                await context.OffersAvailability.AddAsync(new()
                {
                    WarehouseId = rowEl.WarehouseId,
                    NomenclatureId = rowEl.NomenclatureId,
                    OfferId = rowEl.OfferId,
                    Quantity = -rowEl.Quantity,
                }, token);
        }

        if (offersLocked.Length != 0)
            context.RemoveRange(offersLocked);

        res.Response = await context.RowsWarehouses.Where(x => req.Any(y => y == x.Id)).ExecuteDeleteAsync(cancellationToken: token) != 0;
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);

        res.AddSuccess("Команда удаления выполнена");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForWarehouseDocumentUpdateAsync(RowOfWarehouseDocumentModelDB req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<int> res = new();
        if (req.Quantity == 0)
        {
            res.AddError($"Количество не может быть нулевым");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WarehouseDocumentModelDB> queryDocumentDb = context.WarehouseDocuments.Where(x => x.Id == req.WarehouseDocumentId);

        WarehouseDocumentRecord whDoc = await queryDocumentDb
            .Select(x => new WarehouseDocumentRecord(x.WarehouseId, x.IsDisabled))
            .FirstAsync(cancellationToken: token);

        // List<Task> tasks = [];
        if (whDoc.WarehouseId == 0)
        {
            msg = "В документе не указан склад";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (await context.RowsWarehouses.AnyAsync(x => x.Id != req.Id && x.OfferId == req.OfferId && x.WarehouseDocumentId == req.WarehouseDocumentId, cancellationToken: token))
        {
            msg = "В документе уже существует этот офер. Установите ему требуемое количество";
            loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            res.AddError(msg);
            return res;
        }

        if (!res.Success())
            return res;

        RowOfWarehouseDocumentModelDB? rowDb = req.Id > 0
            ? await context.RowsWarehouses.FirstAsync(x => x.Id == req.Id, cancellationToken: token)
            : null;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken: token);
        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            RubricId = whDoc.WarehouseId,
        }];

        if (rowDb is not null && rowDb.OfferId != req.OfferId)
        {
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                RubricId = whDoc.WarehouseId,
            });
        }

        try
        {
            await context.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            res.AddError($"{msg}{ex.Message}");
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }

        OfferAvailabilityModelDB? regOfferAv = await context
            .OffersAvailability
            .FirstOrDefaultAsync(x => x.OfferId == req.OfferId && x.WarehouseId == whDoc.WarehouseId, cancellationToken: token);

        if (regOfferAv is null && !whDoc.IsDisabled)
        {
            regOfferAv = new()
            {
                OfferId = req.OfferId,
                NomenclatureId = req.NomenclatureId,
                WarehouseId = whDoc.WarehouseId,
            };

            await context.AddAsync(regOfferAv, token);
        }
        OfferAvailabilityModelDB? regOfferAvStorno = null;
        if (rowDb is not null && rowDb.OfferId != req.OfferId)
        {
            regOfferAvStorno = new()
            {
                OfferId = rowDb.OfferId,
                NomenclatureId = rowDb.NomenclatureId,
                WarehouseId = whDoc.WarehouseId,
            };
        }

        await queryDocumentDb.ExecuteUpdateAsync(set => set
             .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow)
             .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        if (req.Id < 1)
        {
            if (regOfferAv is not null && !whDoc.IsDisabled)
                regOfferAv.Quantity += req.Quantity;

            req.Version = Guid.NewGuid();
            await context.AddAsync(req, token);
            await context.SaveChangesAsync(token);

            res.AddSuccess("Товар добавлен к документу");
            res.Response = req.Id;
        }
        else
        {
            if (rowDb!.Version != req.Version)
            {
                await transaction.RollbackAsync(token);
                msg = "Строка документа была уже кем-то изменена";
                loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError($"{msg}. Обновите документ (F5), что бы получить актуальные данные");
                return res;
            }

            decimal _delta = req.Quantity - rowDb.Quantity;
            if (_delta == 0)
                res.AddInfo("Количество не изменилось");
            else if (regOfferAv is not null && !whDoc.IsDisabled)
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

            res.Response = await context.RowsWarehouses
                       .Where(x => x.Id == req.Id)
                       .ExecuteUpdateAsync(set => set
                       .SetProperty(p => p.Quantity, req.Quantity).SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
        }
        context.RemoveRange(lockers);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);

        res.AddSuccess($"Обновление `строки складского документа` выполнено");
        return res;
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehouseDocumentsReadAsync(int[] req, CancellationToken token = default)
    {
        TResponseModel<WarehouseDocumentModelDB[]> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WarehouseDocumentModelDB> q = context
            .WarehouseDocuments
            .Where(x => req.Any(y => x.Id == y));

        res.Response = await q
            .Include(x => x.Rows!)
            .ThenInclude(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .ToArrayAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WarehouseDocumentModelDB> q = context
            .WarehouseDocuments
            .AsQueryable();

        if (req.Payload.DisabledOnly.HasValue)
            q = q.Where(x => x.IsDisabled == req.Payload.DisabledOnly.Value);

        if (!string.IsNullOrWhiteSpace(req.Payload.SearchQuery))
            q = q.Where(x => x.NormalizedUpperName.Contains(req.Payload.SearchQuery.ToUpper()));

        if (req.Payload.OfferFilter is not null && req.Payload.OfferFilter.Length != 0)
            q = q.Where(x => context.RowsWarehouses.Any(y => y.WarehouseDocumentId == x.Id && req.Payload.OfferFilter.Any(i => i == y.OfferId)));

        if (req.Payload.NomenclatureFilter is not null && req.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => context.RowsWarehouses.Any(y => y.WarehouseDocumentId == x.Id && req.Payload.NomenclatureFilter.Any(i => i == y.NomenclatureId)));

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.AfterDateUpdate || (x.LastUpdatedAtUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.AfterDateUpdate));

        if (req.Payload.AfterDeliveryDate is not null)
            q = q.Where(x => x.DeliveryDate >= req.Payload.AfterDeliveryDate || (x.DeliveryDate == DateTime.MinValue && x.DeliveryDate >= req.Payload.AfterDeliveryDate));


        IOrderedQueryable<WarehouseDocumentModelDB> oq = req.SortingDirection == DirectionsEnum.Up
           ? q.OrderBy(x => x.CreatedAtUTC)
           : q.OrderByDescending(x => x.CreatedAtUTC);

        IQueryable<WarehouseDocumentModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WarehouseDocumentModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Rows!)
            .ThenInclude(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature);

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
    public async Task<TPaginationResponseModel<OfferAvailabilityModelDB>> RegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OfferAvailabilityModelDB> q = context
            .OffersAvailability
            .AsQueryable();

        if (req.Payload.MinQuantity.HasValue)
            q = q.Where(x => x.Quantity >= req.Payload.MinQuantity);

        if (req.Payload.OfferFilter is not null && req.Payload.OfferFilter.Length != 0)
            q = q.Where(x => req.Payload.OfferFilter.Any(y => y == x.OfferId));

        if (req.Payload.NomenclatureFilter is not null && req.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => req.Payload.NomenclatureFilter.Any(y => y == x.NomenclatureId));

        if (req.Payload.WarehouseId > 0)
            q = q.Where(x => req.Payload.WarehouseId == x.WarehouseId);

        var exQuery = from offerAv in q
                      join oj in context.Offers on offerAv.OfferId equals oj.Id
                      join gj in context.Nomenclatures on offerAv.NomenclatureId equals gj.Id
                      select new { Register = offerAv, Offer = oj, Good = gj };

        var dbRes = req.SortingDirection == DirectionsEnum.Up
           ? await exQuery.OrderBy(x => x.Offer.Name).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token)
           : await exQuery.OrderByDescending(x => x.Offer.Name).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = [.. dbRes.Select(x => x.Register)],
        };
    }
}